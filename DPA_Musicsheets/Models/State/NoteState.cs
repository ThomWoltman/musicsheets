using System;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.Models.State
{
    internal class NoteState : IState
    {
        public void Handle(StateContext context, string content, Staff staff)
        {
            if (content.Contains("\\"))
            {
                context.NextState(new CommandState());
                context.Handle(staff, content);
            }

            else if(new Regex(@"[a-g][,'eis]*[0-9]+[.]*").IsMatch(content)) // = note
            {
                var note = new Note();
                var list = content.ToCharArray();

                note.Length = 1.0 / Int32.Parse(Regex.Match(content, @"\d+").Value);
                // Crosses and Moles
                int alter = 0;
                alter += Regex.Matches(content, "is").Count;
                alter -= Regex.Matches(content, "es|as").Count;
                note.NoteAdjust = (NoteAdjust)alter;

                note.HasDot = content.Contains(".");

                if (content.Contains("'"))
                {
                    context.Octave = context.Octave + 1;
                }
                if (content.Contains(","))
                {
                    context.Octave = context.Octave - 1;
                }

                note.Octave = context.Octave;

                note.NoteType = (NoteType)content.Substring(0, 1).ToUpper().ToCharArray()[0];

                staff.AddSymbol(note);

                double absoluteLength = 1.0 / (1.0 / note.Length);
                if (note.HasDot)
                {
                    absoluteLength += (absoluteLength / 2.0);
                }

                double percentageOfBeatNote = absoluteLength / (1.0 / staff.BeatNote);
                context.percentageOfBar += percentageOfBeatNote / staff.BeatPerBar;

                if(context.percentageOfBar >= 1)
                {
                    staff.AddBar(new Bar());
                    context.percentageOfBar = 0;
                }
            }
            else if (new Regex(@"r.*?[0-9][.]*").IsMatch(content)) // == rest
            {

            }
        }
    }
}