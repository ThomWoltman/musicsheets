using System;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.Builders;
using Sanford.Multimedia.Midi;
using System.Linq;
using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Managers
{
    public class MidiFileHandler : IFileHandler
    {       
            public Staff Staff { get; set; }
            public SymbolFactory SymbolFactory { get; set; }
            public Note CurrentNote { get; set; }
            public MidiFileHandler(SymbolFactory symbolfactory)
            {
                this.Staff = new Staff();
                this.SymbolFactory = symbolfactory;
            }
        public Staff OpenFile(string fileName)
        {
            var sequence = new Sequence();
            sequence.Load(fileName);

            return new MidiStaffConverter().Convert(sequence);
        }
        
    }
}


