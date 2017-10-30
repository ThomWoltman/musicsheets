using System.Linq;

namespace DPA_Musicsheets.Converters.State
{
	public class IterateSequenceState : IMidiStaffConverterState
	{
		public void Handle(IMidiStaffConverterContext context)
		{
			for (; context.SequenceCount < context.Sequence.Count();)
			{
				context.Track = context.Sequence[context.SequenceCount++];
				context.State = new IterateTrackState();
				context.State.Handle(context);
			}
		}
	}
}
