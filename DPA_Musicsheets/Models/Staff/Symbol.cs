using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models
{
	public abstract class Symbol
    {
        public Symbol NextSymbol { get; set; }

        public virtual double Length { get; set; }

        public abstract void Accept(IConverter converter);
    }
}
