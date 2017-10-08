using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class LilypondFileHandlerProvider : IFileHandlerProvider
    {
        public IFileHandler CreateFileHandler(string fileName)
        {
            if (Path.GetExtension(fileName).EndsWith(".ly"))
            {
                return new LilypondFileHandler();
            }

            return null;
        }
    }
}
