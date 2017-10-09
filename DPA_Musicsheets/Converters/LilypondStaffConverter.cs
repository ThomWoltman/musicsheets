using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters
{
    public class LilypondStaffConverter : StateContext
    {
        public StringBuilder _result = new StringBuilder();
        public int _octave = 0;
        public string Convert(Staff staff)
        {
            _result = new StringBuilder();

            _result.Append("\\relative c");
            var note = (Note)staff.Bars.First().Symbols.First();
            _octave = note.Octave;
            int relative = note.Octave - 2;
            while(relative > 0)
            {
                _result.Append("'");
                relative--;
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
            _result.Append($"{note.NoteType.ToString().ToLower()}{GetIsAs((int)note.NoteAdjust)}{OctaveAdjust(note.Octave)}{1.0/note.Length}{GetDot(note.HasDot)} ");
        }

        public string OctaveAdjust(int octave)
        {
            int adjust = octave - _octave;
            _octave = octave;
            if(adjust > 0)
            {
                return "'";
            }

            if(adjust < 0)
            {
                return ",";
            }

            return "";
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

            return staff;
        }
    }
}
