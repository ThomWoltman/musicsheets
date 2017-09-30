using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Staff
    {
        public int BeatNote { get; set; }
        public int BeatPerBar { get; set; }
        public int Bpm { get; set; }

        //public List<Symbol> Symbols { get; set; }

        public List<Bar> Bars { get; set; }

        public Staff()
        {
            //Symbols = new List<Symbol>();
            Bars = new List<Bar>();
        }

        public void AddSymbol(Symbol symbol)
        {
            Bars.Last().AddSymbol(symbol);
        }

        public void AddBar(Bar bar)
        {
            Bars.Add(bar);
        }
    }
}