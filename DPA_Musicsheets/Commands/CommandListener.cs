using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.Views;
using MidiPlayerTest;

namespace DPA_Musicsheets.Commands
{
	public class CommandListener : ICommandListener
	{
		private IList<Command> commands;

		public CommandListener()
		{
			commands = new List<Command>();
		}

		public void AddCommand(Key[] keys, Action action)
		{
			commands.Add(new Command(keys, action));
		}

		public void Handle()
		{
			var orderedCommands = commands.OrderByDescending(c => c.NumberOfKeys);

			foreach (Command command in orderedCommands)
			{
				if (command.CanExecute())
				{
					command.Execute();
					break;
				}
			}
		}
	}
}
