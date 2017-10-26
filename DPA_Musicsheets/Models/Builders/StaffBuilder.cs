﻿using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Builders
{
    public class StaffBuilder
    {
        public Staff Staff { get; set; }
        public SymbolFactory SymbolFactory{ get; set; }
        public Note CurrentNote { get; set; }

        public StaffBuilder(SymbolFactory symbolfactory)
        {
            this.Staff = new Staff();
            this.SymbolFactory = symbolfactory;
        }

        public Staff Parse(Sequence sequence)
        {

            int division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;

            Staff.AddBar(new Bar());

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
                                    Staff.BeatNote = timeSignatureBytes[0];
                                    Staff.BeatPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    Staff.Bpm = 60000000 / tempo;
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
                                    this.CurrentNote = SymbolFactory.create(channelMessage.Data1);

                                    //previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    // Finish the previous note with the length.
                                    bool hasDot = false;
                                    double percentageOfBar;

                                    CurrentNote.Length = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, this.Staff.BeatNote, this.Staff.BeatPerBar, out hasDot, out percentageOfBar);
                                    CurrentNote.HasDot = hasDot;

                                    percentageOfBarReached += percentageOfBar;
                                    

                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                                    
                                    startedNoteIsClosed = true;

                                    Staff.AddSymbol(this.CurrentNote);
                                    this.CurrentNote = null;

                                    if (percentageOfBarReached >= 1)
                                    {
                                        percentageOfBarReached -= 1;
                                        Staff.AddBar(new Bar());
                                    }
                                }
                                else
                                {
                                    var symbol = SymbolFactory.createRest();
                                    Staff.AddSymbol(symbol);
                                }
                            }
                            break;
                    }
                }
            }
            return Staff;
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
            
            return percentageOfBeatNote;
    }

    public Staff Parse(string lilypond)
        {
            throw new NotImplementedException();
        }
    }
}
