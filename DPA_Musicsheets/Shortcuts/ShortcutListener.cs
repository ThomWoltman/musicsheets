using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Shortcuts
{
	public class ShortcutListener: IShortcutListener
	{
		private IDictionary<Shortcut, Action> shortcuts;

		public ShortcutListener()
		{
			shortcuts = new Dictionary<Shortcut, Action>();
		}

		public void AddShortcut(Key[] keys, Action command)
		{
			shortcuts[new Shortcut(keys)] = command;
		}

		public bool Listen()
		{
			var sortedShortCuts = shortcuts.OrderByDescending(pair => pair.Key.NumberOfKeys);

			foreach (var pair in sortedShortCuts)
			{
				if (pair.Key.IsActive)
				{
					shortcuts[pair.Key]();
					return true;
				}
			}

			return false;
		}
	}
}
