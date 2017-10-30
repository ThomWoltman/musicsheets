namespace DPA_Musicsheets.Models.Builders
{
	public class SymbolFactory
    {
        public Rest createRest()
        {
            return new Rest();
        }
        public Note create(int midikey)
        {
            Note note = new Note();

            note.Octave = (midikey / 12) - 1;
            NoteAdjust noteAdjust;
            note.NoteType = getNote(midikey, out noteAdjust);
            note.NoteAdjust = noteAdjust;

            return note;
        }

        public NoteType getNote(int midiKey, out NoteAdjust noteAdjust)
        {
            noteAdjust = NoteAdjust.None;
            switch (midiKey % 12)
            {
                case 0:
                    return NoteType.C;
                case 1:
                    noteAdjust = NoteAdjust.Sharp;
                    return NoteType.C;
                case 2:
                    return NoteType.D;
                case 3:
                    noteAdjust = NoteAdjust.Sharp;
                    return NoteType.D;
                case 4:
                    return NoteType.E;
                case 5:
                    return NoteType.F;
                case 6:
                    noteAdjust = NoteAdjust.Sharp;
                    return NoteType.F;
                case 7:
                    return NoteType.G;
                case 8:
                    noteAdjust = NoteAdjust.Sharp;
                    return NoteType.G;
                case 9:
                    return NoteType.A;
                case 10:
                    noteAdjust = NoteAdjust.Sharp;
                    return NoteType.A;
                case 11:
                    return NoteType.B;
            }
            return NoteType.Unknown;
        }
    }
}
