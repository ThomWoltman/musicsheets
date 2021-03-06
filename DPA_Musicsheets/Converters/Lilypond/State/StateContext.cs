﻿using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.State
{
	public abstract class StateContext
    {
        protected IState _currentState;
        public int Octave;
        public List<Char> notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };
        public char previousnote = 'c';
        public double percentageOfBar = 0;
        public bool _isValid = true;

        public void Handle(Staff staff, string content)
        {
            if (_isValid)
            {
                if (_currentState == null)
                {
                    _currentState = new CommandState();
                }
                _currentState.Handle(this, content, staff);
            }    
        }

        public void NextState(IState state)
        {
            _currentState = state;
        }
    }
}
