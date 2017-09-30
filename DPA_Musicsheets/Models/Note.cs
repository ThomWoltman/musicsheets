using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class Note : Symbol
    {
        public NoteType NoteType { get; set; }
        public NoteAdjust NoteAdjust { get; set; }
        public int Octave { get; set; } 
        public bool HasDot { get; set; }
        public Note()
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
    }
}
