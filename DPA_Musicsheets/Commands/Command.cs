using System;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands
{
	public class Command
	{
		private Key[] _keys;
		private Action _action;
		public int NumberOfKeys { get { return _keys.Length; } }

		public Command(Key[] keys, Action action)
		{
			_keys = keys;
			_action = action;
		}

		public void Execute() { _action(); }

		public bool CanExecute()
		{
			foreach (Key key in _keys)
				if (!Keyboard.IsKeyDown(key)) return false;

			return true;
		}
	}
}
