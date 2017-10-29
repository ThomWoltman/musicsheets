using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models
{
	public class Rest : Symbol
    {
        public bool HasDot { get; set; }
        public Rest()
        {

        }

        public override void Accept(WPFStaffConverter converter)
        {
            converter.Visit(this);
        }

        public override void Accept(MidiStaffConverter converter)
        {
            converter.Visit(this);
        }

        public override void Accept(LilypondStaffConverter converter)
        {
            converter.Visit(this);
        }
    }
}
