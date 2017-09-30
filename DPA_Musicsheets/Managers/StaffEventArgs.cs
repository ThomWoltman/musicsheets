using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class StaffEventArgs : EventArgs
    {
        public Staff Staff { get; set; }
        public string Message { get; set; }
    }
}
