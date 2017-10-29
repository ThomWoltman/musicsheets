using PSAMControlLibrary;
using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Builders
{
	public class PSAMStaffBuilder
    {
        public List<MusicalSymbol> symbols { get; set; }

        public PSAMStaffBuilder()
        {
            symbols = new List<MusicalSymbol>();
        }

        public List<MusicalSymbol> build(Staff myStaff)
        {
            var currentClef = new Clef(ClefType.GClef, 2);
            symbols.Add(currentClef);
           
            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(myStaff.BeatNote+""), UInt32.Parse(myStaff.BeatPerBar+"")));            

            foreach (var bar in myStaff.Bars)
            {
                foreach (var symbol in bar.Symbols)
                {
                    //symbol.Accept(this);
                }
                symbols.Add(new Barline());
            }

            return symbols;
        }

        public void Visit(Note note)
        {
            var psamNote = new PSAMControlLibrary.Note(note.NoteType.ToString(), (int)note.NoteAdjust, note.Octave, getMusicalSymbolDuration(note.Length), NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
            if (note.HasDot)
            {
                psamNote.NumberOfDots = 1;
            }
            symbols.Add(psamNote);
        }

        public MusicalSymbolDuration getMusicalSymbolDuration(double length)
        {
            //TODO: add relative to beatnote
            switch (length)
            {
                case 4:
                    return MusicalSymbolDuration.Whole;
                case 2:
                    return MusicalSymbolDuration.Half;
                case 1:
                    return MusicalSymbolDuration.Quarter;
                case 0.5:
                    return MusicalSymbolDuration.Eighth;
                case 0.25:
                    return MusicalSymbolDuration.Sixteenth;
                case 0.125:
                    return MusicalSymbolDuration.d32nd;

                default: return MusicalSymbolDuration.Unknown;
            }
        }

        public void Visit(Rest rest)
        {
            symbols.Add(new PSAMControlLibrary.Rest(getMusicalSymbolDuration(rest.Length)));
        }

        public void Visit(Repeat repeat)
        {
            throw new NotImplementedException("repeat is not yet implemented");
        }
    }
}
