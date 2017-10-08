using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.State
{
    public interface IState
    {
        void Handle(StateContext context, string content, Staff staff);
    }
}
