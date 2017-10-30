using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Shortcuts
{
	public interface IShortcutListener
	{
		void AddShortcut(Key[] keys, Action command);
		bool Listen();
	}
}
