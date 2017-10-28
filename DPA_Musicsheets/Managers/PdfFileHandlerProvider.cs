using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class PdfFileHandlerProvider : IFileHandlerProvider
    {
        public IFileHandler CreateFileHandler(string fileName)
        {
            if (Path.GetExtension(fileName).EndsWith(".pdf"))
            {
                return new PdfFileHandler();
            }

            return null;
        }
    }
}
