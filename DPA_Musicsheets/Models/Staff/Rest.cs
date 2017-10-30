using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models
{
	public class Rest : Symbol
    {
        public Rest()
        {

        }

        public override void Accept(IConverter converter)
        {
            converter.Visit(this);
        }
    }
}
