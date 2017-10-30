using System.Text.RegularExpressions;

namespace DPA_Musicsheets.Models.State
{
	public class CommandState : IState
    {
        public void Handle(StateContext context, string content, Staff staff)
        {
            switch (content)
            {
                case "": break;
                case "2": break;
                case "volta": break;
                case "treble": break;
                case "\\relative": context.NextState(new OctaveEntryState()); break;
                case "\\clef": break;
                case "\\repeat": break;
                case "\\alternative": break;
                case "\\time": context.NextState(new TimeState()); break;
                case "\\tempo": context.NextState(new TempoState()); break;
                case "|": break;
                case "{": break;
                case "}": break;
                case var someValue when new Regex(@"[a-g][,'eis]*[0-9]+[.]*").IsMatch(content): //note
                    context.NextState(new NoteState());
                    context.Handle(staff, content);
                    break;
                case var someValue when new Regex(@"r.*?[0-9][.]*").IsMatch(content): //rest
                    context.NextState(new NoteState());
                    context.Handle(staff, content);
                    break;
                default:
                    context._isValid = false;
                    break;
            }
        }
    }
}
