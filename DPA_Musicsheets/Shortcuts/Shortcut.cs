using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Shortcuts
{
	public class Shortcut
	{
		public int NumberOfKeys { get { return _keys.Length; } }

		public bool IsActive
		{
			get
			{
				foreach (Key key in _keys.Reverse())
					if (!Keyboard.IsKeyDown(key)) return false;

				return true;
			}
		}

		private Key[] _keys;

		public Shortcut(Key[] keys)
		{
			_keys = keys;
		}
	}
}
