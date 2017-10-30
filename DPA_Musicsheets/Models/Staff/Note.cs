using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models
{
	public class Note : Symbol
    {
        public NoteType NoteType { get; set; }
        public NoteAdjust NoteAdjust { get; set; }
        public int Octave { get; set; } 
        public bool HasDot { get; set; }
        public Note()
        {
            
        }
        public override void Accept(IConverter converter)
        {
            converter.Visit(this);
        }
    }
}
