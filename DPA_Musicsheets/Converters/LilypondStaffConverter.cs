using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters
{
    public class LilypondStaffConverter : StateContext
    {
        public string Convert(Staff staff)
        {
            throw new NotImplementedException();
        }

        public Staff Convert(string lilypond)
        {
            var staff = new Staff();
            staff.AddBar(new Bar());

            foreach (string s in lilypond.Split(' '))
            {
                this.Handle(staff, s);
            }

            return staff;
        }
    }
}
