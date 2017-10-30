using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Commands
{
    public interface ICommandListener
    {
		void AddCommand(Key[] keys, Action action);
		void Handle();
    }
}
