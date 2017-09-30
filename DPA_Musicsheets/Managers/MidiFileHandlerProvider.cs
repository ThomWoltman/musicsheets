using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class MidiFileHandlerProvider : IFileHandlerProvider
    {
        public IFileHandler CreateFileHandler(string fileName)
        {
            if (Path.GetExtension(fileName).EndsWith(".mid"))
            {
                return new MidiFileHandler(new Models.Builders.SymbolFactory());
            }

            return null;
        }
    }
}
