using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.Managers
{
	public interface IFileHandler
    {
        Staff OpenFile(string fileName);

        void SaveFile(string fileName, Staff staff);
    }
}
