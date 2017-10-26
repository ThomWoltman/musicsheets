using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Builders;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Models.Adapters
{
	public class ChannelAdpater : IMidiAdapter
	{
		public Staff GetStaff(MidiEvent midiEvent, Sequence sequence, Staff staff)
		{
			ChannelMessage channelMessage = midiEvent.MidiMessage as ChannelMessage;
			Note currentNote = null;
			Rest currentRest = null;
			int division = sequence.Division;
			int previousMidiKey = 60; // Central C;
			int previousNoteAbsoluteTicks = 0;
			double percentageOfBarReached = 0;
			bool startedNoteIsClosed = true;
			var symbolFactory = new SymbolFactory();

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
					if (currentNote != null)
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

			return staff;
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
	}
}
