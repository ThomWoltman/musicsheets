using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using System.IO;
using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Managers
{
    public class LilypondFileHandler : IFileHandler
    {
        public Staff OpenFile(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadAllLines(fileName))
            {
                sb.AppendLine(line);
            }
            var LilypondText = sb.ToString();
            LilypondText = LilypondText.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");

            var converter = new LilypondStaffConverter();
            return converter.Convert(LilypondText);
        }

        public Staff SaveFile(string fileName, Staff staff)
        {
            throw new NotImplementedException();
        }
    }
}
