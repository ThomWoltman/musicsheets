namespace DPA_Musicsheets.Managers
{
	public interface IFileHandlerProvider
    {
        IFileHandler CreateFileHandler(string fileName);
    }
}
