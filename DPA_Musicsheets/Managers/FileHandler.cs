
using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.Builders;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
