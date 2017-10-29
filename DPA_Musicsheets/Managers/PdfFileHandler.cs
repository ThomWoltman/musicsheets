using System;
using DPA_Musicsheets.Models;
using System.IO;
using DPA_Musicsheets.Converters;
using System.Diagnostics;

namespace DPA_Musicsheets.Managers
{
	public class PdfFileHandler : IFileHandler
    {
        public Staff OpenFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void SaveFile(string fileName, Staff staff)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string tmpFileName = $"{fileName}-tmp.ly";

            using (StreamWriter outputFile = new StreamWriter(tmpFileName))
            {
                outputFile.Write(new LilypondStaffConverter().Convert(staff));
                outputFile.Close();
            }

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited)
            { /* Wait for exit */
            }
            if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }
    }
}
