using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Managers
{
	public class FileHandler
    {
        public Staff MyStaff { get; set; }
        public event EventHandler<StaffEventArgs> StaffChanged;

        private IEnumerable<IFileHandlerProvider> _fileHandlerProviders;

        public FileHandler(IEnumerable<IFileHandlerProvider> fileHandlerProviders)
        {
            this._fileHandlerProviders = fileHandlerProviders;
        }

        public void OpenFile(string fileName)
        {
            var filehandler = CreateFileHandler(fileName);
            MyStaff = filehandler?.OpenFile(fileName);
            StaffChanged?.Invoke(this, new StaffEventArgs() { Staff = MyStaff, Message = "staff changed" });
        }

        public void ChangeStaff(Staff staff)
        {
            MyStaff = staff;
            StaffChanged?.Invoke(this, new StaffEventArgs() { Staff = MyStaff, Message = "staff changed" });
        }

        private IFileHandler CreateFileHandler(string fileName)
        {
            foreach (var provider in _fileHandlerProviders)
            {
                var filehandler = provider.CreateFileHandler(fileName);
                if(filehandler != null)
                {
                    return filehandler;
                }
            }
            return null;
        }  
        
        #region Saving to files
        internal bool SaveFile(string fileName)
        {
            var filehandler = CreateFileHandler(fileName);
            filehandler?.SaveFile(fileName, this.MyStaff);
            return filehandler != null;   
        }

        #endregion Saving to files
    }
}
