using System.IO;

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
