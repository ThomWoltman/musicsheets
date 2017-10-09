using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.Builders;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters
{
    public class MidiStaffConverter
    {
        Sequence sequence;
        Staff staff;
        Track notesTrack;
        int absoluteTicks;

        public Staff Convert(Sequence sequence)
        {
            Staff staff = new Staff();
            Models.Note currentNote = null;
            Models.Rest currentRest = null;
            var symbolFactory = new SymbolFactory();

            int division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;

            staff.AddBar(new Bar());

            for (int i = 0; i < sequence.Count(); i++)
            {
                Track track = sequence[i];

                foreach (var midiEvent in track.Iterator())
                {
                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    switch (midiMessage.MessageType)
                    {
                        case MessageType.Meta:
                            var metaMessage = midiMessage as MetaMessage;
                            switch (metaMessage.MetaType)
                            {
                                case MetaType.TimeSignature:
                                    byte[] timeSignatureBytes = metaMessage.GetBytes();
                                    staff.BeatNote = timeSignatureBytes[0];
                                    staff.BeatPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    staff.Bpm = 60000000 / tempo;
                                    break;
                                case MetaType.EndOfTrack:
                                    break;
                                default: break;
                            }
                            break;
                        case MessageType.Channel:

                            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                            if (channelMessage.Command == ChannelCommand.NoteOn)
                            {
                                if (channelMessage.Data2 > 0) // Data2 = loudness
                                {
                                    //create note with height
                                    currentNote = symbolFactory.create(channelMessage.Data1);

                                    //previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    if(currentNote != null)
                                    {
                                        // Finish the previous note with the length.
                                        bool hasDot = false;
                                        double percentageOfBar;

                                        currentNote.Length = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, staff.BeatNote, staff.BeatPerBar, out hasDot, out percentageOfBar);
                                        currentNote.HasDot = hasDot;

                                        percentageOfBarReached += percentageOfBar;


                                        previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;

                                        startedNoteIsClosed = true;

                                        staff.AddSymbol(currentNote);
                                        currentNote = null;

                                        if (percentageOfBarReached >= 1)
                                        {
                                            percentageOfBarReached -= 1;
                                            staff.AddBar(new Bar());
                                        }
                                    }
                                }
                            }
                            
                            break;
                    }
                }
            }
            return staff;
        }

        public void Visit(Models.Note note)
        {
            // Calculate duration
            double absoluteLength = 1.0 / (1.0/note.Length);
            if (note.HasDot)
            {
                absoluteLength += (absoluteLength / 2.0);
            }

            double relationToQuartNote = staff.BeatNote / 4.0;
            double percentageOfBeatNote = (1.0 / staff.BeatNote) / absoluteLength;
            double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            // Calculate height
            int noteHeight = notesOrderWithCrosses.IndexOf(note.NoteType.ToString().ToLower()) + ((note.Octave + 1) * 12);
            noteHeight += (int)note.NoteAdjust;
            notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

            absoluteTicks += (int)deltaTicks;
            notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume
        }

        public void Visit(Models.Rest rest)
        {
            //throw new NotImplementedException();
        }

        private double GetNoteLength(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar, out bool hasDot, out double percentageOfBar)
        {
            var lengths = new int[] { 1, 2, 4, 8, 16, 32 };

            percentageOfBar = 0;
            int duration = 0;
            int dots = 0;
            hasDot = false;

            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                return 0;
            }

            double percentageOfBeatNote = deltaTicks / division;
            percentageOfBar = (1.0 / beatsPerBar) * percentageOfBeatNote;

            if (!(percentageOfBeatNote == 4 || percentageOfBeatNote == 2 || percentageOfBeatNote == 1 || percentageOfBeatNote == 0.5 || percentageOfBeatNote == 0.25 || percentageOfBeatNote == 0.125))
            {
                hasDot = true;
                percentageOfBeatNote = percentageOfBeatNote / 3 * 2;
            }
            percentageOfBeatNote = percentageOfBeatNote / 4;

            return percentageOfBeatNote;
        }

        public Sequence Convert(Staff staff)
        {
            this.staff = staff;
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            absoluteTicks = 0;

            sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);

            // Calculate tempo
            int speed = (60000000 / staff.Bpm);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

            notesTrack = new Track();
            sequence.Add(notesTrack);

            byte[] timeSignature = new byte[4];
            timeSignature[0] = (byte)staff.BeatPerBar;
            timeSignature[1] = (byte)(Math.Log(staff.BeatNote) / Math.Log(2));
            metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));

            foreach (Bar bar in staff.Bars)
            {
                foreach (Symbol symbol in bar.Symbols)

                symbol.Accept(this);           
            }

            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            return sequence;
        }
    } 
}
