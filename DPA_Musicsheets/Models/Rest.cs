using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Builders;
using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models
{
    public class Rest : Symbol
    {
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
    }
}
