using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Converters
{
	public class WPFStaffConverter
    {
        private List<MusicalSymbol> symbols;

        public Staff Convert(List<MusicalSymbol> symbols)
        {
            throw new NotImplementedException();
        }

        public List<MusicalSymbol> Convert(Staff staff)
        {
            symbols = new List<MusicalSymbol>();
            var currentClef = new Clef(ClefType.GClef, 2);
            symbols.Add(currentClef);

            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(staff.BeatNote + ""), UInt32.Parse(staff.BeatPerBar + "")));

            foreach (var bar in staff.Bars)
            {
                foreach (var symbol in bar.Symbols)
                {
                    symbol.Accept(this);
                }
                symbols.Add(new Barline());
            }

            return symbols;
        }

        public void Visit(Models.Note note)
        {
            var psamNote = new PSAMControlLibrary.Note(note.NoteType.ToString(), (int)note.NoteAdjust, note.Octave, getMusicalSymbolDuration(note.Length), NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
            if (note.HasDot)
            {
                psamNote.NumberOfDots = 1;
            }
            symbols.Add(psamNote);
        }

        private MusicalSymbolDuration getMusicalSymbolDuration(double length)
        {
            int musicalsymbollength = (int)(1.0 / length);
            return (MusicalSymbolDuration)musicalsymbollength;
        }

        public void Visit(Models.Rest rest)
        {
            symbols.Add(new PSAMControlLibrary.Rest(getMusicalSymbolDuration(rest.Length)));
        }

        public void Visit(Repeat repeat)
        {
            throw new NotImplementedException("repeat is not yet implemented");
        }
    }
}
