namespace DPA_Musicsheets.Models.State
{
	public interface IState
    {
        void Handle(StateContext context, string content, Staff staff);
    }
}
