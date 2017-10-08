using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.State
{
    public class CommandState : IState
    {
        public void Handle(StateContext context, string content, Staff staff)
        {
            switch (content)
            {
                case "\\relative": context.NextState(new OctaveEntryState()); break;
                case "\\clef": break;
                case "\\repeat": break;
                case "\\alternative": break;
                case "\\time": context.NextState(new TimeState()); break;
                case "\\tempo": context.NextState(new TempoState()); break;
                case "|": break;
                case "{": break;
                case "}": break;
                default: context.NextState(new NoteState());
                         context.Handle(staff, content);
                         break;
            }
        }
    }
}
