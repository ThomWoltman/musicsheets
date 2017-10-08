using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.State
{
    public abstract class StateContext
    {
        protected IState _currentState;
        public int Octave;
        public double percentageOfBar = 0;

        public void Handle(Staff staff, string content)
        {
            if(_currentState == null)
            {
                _currentState = new CommandState();
            }
            _currentState.Handle(this, content, staff);
        }

        public void NextState(IState state)
        {
            _currentState = state;
        }
    }
}
