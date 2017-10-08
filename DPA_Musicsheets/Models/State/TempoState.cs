using System;

namespace DPA_Musicsheets.Models.State
{
    internal class TempoState : IState
    {
        public void Handle(StateContext context, string content, Staff staff)
        {
            var tempo = content.Split('=');
            staff.Bpm = int.Parse(tempo[1]);

            context.NextState(new CommandState());
        }
    }
}