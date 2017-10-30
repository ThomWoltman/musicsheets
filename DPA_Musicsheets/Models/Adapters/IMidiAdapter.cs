using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Models.Adapters
{
	public interface IMidiAdapter
	{
		Staff GetStaff(MidiEvent midiEvent, Sequence sequence, Staff staff);
	}
}
