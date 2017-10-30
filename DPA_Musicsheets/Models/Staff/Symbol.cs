using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models
{
	public abstract class Symbol
    {
        public Symbol NextSymbol { get; set; }

        public virtual double Length { get; set; }

        public abstract void Accept(WPFStaffConverter converter);

        public abstract void Accept(MidiStaffConverter converter);

        public abstract void Accept(LilypondStaffConverter converter);
    }
}
