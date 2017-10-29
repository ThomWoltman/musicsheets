using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Memento
{
	public class CareTaker
    {
        private Stack<Memento> _undos;
        private Stack<Memento> _redos;

        public CareTaker()
        {
            _undos = new Stack<Memento>();
            _redos = new Stack<Memento>();
        }

        public string Redo(string lilypond)
        {
            _undos.Push(new Memento(lilypond));
            return _redos.Pop().GetLilypond();
        }

        public void Save(string lilypond)
        {
            _undos.Push(new Memento(lilypond));
        }

        public string Undo(string lilypond)
        {
            _redos.Push(new Memento(lilypond));
            return _undos.Pop().GetLilypond();
        }

        public bool CanUndo()
        {
            return _undos.Count > 0;
        }

        public bool CanRedo()
        {
            return _redos.Count > 0;
        }
    }
}
