namespace DPA_Musicsheets.Models.Memento
{
	public class Memento
    {
        private string _lilypond;

        public Memento(string lilypond)
        {
            _lilypond = lilypond;
        }

        public string GetLilypond()
        {
            return _lilypond;
        }
    }
}
