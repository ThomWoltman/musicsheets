using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Bar
    {
        public List<Symbol> Symbols { get; set; }
        public Bar()
        {
            Symbols = new List<Symbol>();
        }

        public void AddSymbol(Symbol symbol)
        {
            Symbols.Add(symbol);
        }
    }
}
