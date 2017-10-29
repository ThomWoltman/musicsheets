using DPA_Musicsheets.Models;
using System;

namespace DPA_Musicsheets.Managers
{
	public class StaffEventArgs : EventArgs
    {
        public Staff Staff { get; set; }
        public string Message { get; set; }
    }
}
