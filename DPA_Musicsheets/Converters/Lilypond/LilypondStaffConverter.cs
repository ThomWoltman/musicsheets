using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.State;
using System.Linq;
using System.Text;

namespace DPA_Musicsheets.Converters
{
	public class LilypondStaffConverter : StateContext, IConverter
    {
        public StringBuilder _result = new StringBuilder();
        public int _octave = 0;
        public string Convert(Staff staff)
        {
            _result = new StringBuilder();

            _result.Append("\\relative c");
            var note = (Note)staff.Bars.First().Symbols.First();
            _octave = note.Octave;
            int relative = note.Octave - 3;
            while(relative > 0)
            {
                _result.Append("'");
                relative--;
            }
            while (relative < 0)
            {
                _result.Append(",");
                relative++;
            }

            _result.Append(" {");
            _result.Append("\r\n");

            _result.Append("\\clef treble\r\n");

            _result.Append($"\\time {staff.BeatNote}/{staff.BeatPerBar}\r\n");

            _result.Append($"\\tempo 4={staff.Bpm}\r\n");

            foreach (var bar in staff.Bars)
            {
                foreach (var symbol in bar.Symbols)
                {
                    symbol.Accept(this);
                }
                _result.Append("\r\n");
            }

            _result.Append("}");

            return _result.ToString();
        }

        public void Visit(Note note)
        {
            string noteType = note.NoteType.ToString().ToLower();
            string noteAdjust = GetIsAs((int)note.NoteAdjust);
            string octaveAdjust = OctaveAdjust(note.Octave, note.NoteType.ToString().ToLower()[0]);
            string noteLength = 1.0 / note.Length + "";
            string dot = GetDot(note.HasDot);

            _result.Append($"{noteType}{noteAdjust}{octaveAdjust}{noteLength}{dot} ");
        }

        private string OctaveAdjust(int octave, char note)
        {
            int distanceWithPreviousNote = notesorder.IndexOf(note) - notesorder.IndexOf(previousnote);

            if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
            {
                distanceWithPreviousNote -= 7; // The number of notes in an octave
            }
            else if (distanceWithPreviousNote < -3)
            {
                distanceWithPreviousNote += 7; // The number of notes in an octave
            }

            if (distanceWithPreviousNote + notesorder.IndexOf(previousnote) >= 7)
            {
                _octave++;
            }
            else if (distanceWithPreviousNote + notesorder.IndexOf(previousnote) < 0)
            {
                _octave--;
            }

            previousnote = note;

            string result = "";
            while(_octave < octave)
            {
                _octave++;
                result += "'";
            }

            while (_octave > octave)
            {
                _octave--;
                result += ",";
            }

            return result;
        }

        private string GetDot(bool hasdot)
        {
            if (hasdot) return ".";
            return "";
        }

        private string GetIsAs(int adjust)
        {
            if(adjust == 1)
            {
                return "is";
            }
            if (adjust == -1)
            {
                return "es";
            }
            return "";
        }

        public void Visit(Rest note)
        {
            _result.Append($"r{1.0 / note.Length} ");
        }

        public Staff Convert(string lilypond)
        {
            var staff = new Staff();
            staff.AddBar(new Bar());

            foreach (string s in lilypond.Split(' '))
            {
                this.Handle(staff, s);
            }

            if (this._isValid)
            {
                return staff;
            }
            return null;
        }
    }
}
