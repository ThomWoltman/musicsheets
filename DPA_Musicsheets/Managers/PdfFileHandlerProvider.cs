using System.IO;

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
