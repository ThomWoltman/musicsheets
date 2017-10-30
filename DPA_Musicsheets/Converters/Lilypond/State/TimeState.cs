namespace DPA_Musicsheets.Models.State
{
	public class TimeState : IState
    {
        public void Handle(StateContext context, string content, Staff staff)
        {
            var time = content.Split('/');
            staff.BeatNote = int.Parse(time[0]);
            staff.BeatPerBar = int.Parse(time[1]);

            context.NextState(new CommandState());
        }
    }
}
