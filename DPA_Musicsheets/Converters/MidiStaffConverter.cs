using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.Builders;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using DPA_Musicsheets.Converters.State;

namespace DPA_Musicsheets.Converters
{
	public class MidiStaffConverter : IMidiStaffConverterContext
	{
		public IMidiStaffConverterState State { get; set; }
		public Sequence Sequence { get; set; }
		public int SequenceCount { get; set; }
		public Staff Staff { get; set; }
		public Track Track { get; set; }
		public Note CurrentNote { get; set; }
		public SymbolFactory SymbolFactory { get; set; }
		public double PercentageOfBarReached { get; set; }
		public int PreviousNoteAbsoluteTicks { get; set; }
		public bool StartedNoteIsClosed { get; set; }

		Track notesTrack;
		int absoluteTicks;


		public MidiStaffConverter()
		{
			SymbolFactory = new SymbolFactory();
		}

		public Staff Convert(Sequence sequence)
		{
			Sequence = sequence;
			SequenceCount = 0;
			CurrentNote = null;

			Staff = new Staff();
			Staff.AddBar(new Bar());

			State = new IterateSequenceState();
			State.Handle(this);
			return Staff;
		}

		public void Visit(Models.Note note)
		{
			// Calculate duration
			double absoluteLength = 1.0 / (1.0 / note.Length);
			if (note.HasDot)
			{
				absoluteLength += (absoluteLength / 2.0);
			}

			double relationToQuartNote = Staff.BeatNote / 4.0;
			double percentageOfBeatNote = (1.0 / Staff.BeatNote) / absoluteLength;
			double deltaTicks = (Sequence.Division / relationToQuartNote) / percentageOfBeatNote;

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

		public Sequence Convert(Staff staff)
		{
			Staff = staff;
			List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
			absoluteTicks = 0;

			Sequence = new Sequence();

			Track metaTrack = new Track();
            notesTrack = new Track();
            Sequence.Add(metaTrack);
            Sequence.Add(notesTrack);

            AddTempo(metaTrack, staff);
            AddTimeSignature(metaTrack, staff);


			foreach (Bar bar in staff.Bars)
			{
				foreach (Symbol symbol in bar.Symbols)

					symbol.Accept(this);
			}

			notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
			metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
			return Sequence;
		}

        private void AddTempo(Track metaTrack, Staff staff)
        {
            // Calculate tempo
            int speed = (60000000 / staff.Bpm);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));
        }

        private void AddTimeSignature(Track metaTrack, Staff staff)
        {
            byte[] timeSignature = new byte[4];
            timeSignature[0] = (byte)staff.BeatPerBar;
            timeSignature[1] = (byte)(Math.Log(staff.BeatNote) / Math.Log(2));
            metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
        }
	}
}
