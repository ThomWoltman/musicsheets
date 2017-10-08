using System;
using System.Linq;

namespace DPA_Musicsheets.Models.State
{
    internal class OctaveEntryState : IState
    {
        public void Handle(StateContext context, string content, Staff staff)
        {
            int octave = 2;
            octave += content.Count(c => c == '\'');
            octave -= content.Count(c => c == ',');

            context.Octave = octave;

            context.NextState(new CommandState());
        }
    }
}