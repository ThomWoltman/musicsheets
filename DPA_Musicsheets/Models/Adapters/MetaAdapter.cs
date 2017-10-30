using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Models.Adapters
{
	public class MetaAdapter : IMidiAdapter
	{
		public Staff GetStaff(MidiEvent midiEvent, Sequence sequence, Staff staff)
		{
			MetaMessage metaMessage = midiEvent.MidiMessage as MetaMessage;

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
			
			return staff;
		}
	}
}
