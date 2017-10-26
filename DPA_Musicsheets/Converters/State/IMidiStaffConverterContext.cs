using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.Builders;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters.State
{
	public interface IMidiStaffConverterContext
	{
		IMidiStaffConverterState State { get; set; }
		Sequence Sequence { get; }
		int SequenceCount { get; set; }
		Staff Staff { get; set; }
		Note CurrentNote { get; set; }
		SymbolFactory SymbolFactory { get; }
		Track Track { get; set; }
	}
}
