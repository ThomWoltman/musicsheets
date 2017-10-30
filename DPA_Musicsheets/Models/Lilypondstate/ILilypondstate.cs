using DPA_Musicsheets.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Lilypondstate
{
    public interface ILilypondstate
    {
        void Handle(LilypondViewModel vm);
    }
}
