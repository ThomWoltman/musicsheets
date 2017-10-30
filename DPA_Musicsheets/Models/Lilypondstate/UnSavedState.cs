using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Models.Lilypondstate
{
    public class UnSavedState : ILilypondstate
    {
        public void Handle(LilypondViewModel vm)
        {
            vm.SaveAsCommand.Execute(null);
        }
    }
}
