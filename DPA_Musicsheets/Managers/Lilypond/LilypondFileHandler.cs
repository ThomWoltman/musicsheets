﻿using System.Text;
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

        public void SaveFile(string fileName, Staff staff)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write(new LilypondStaffConverter().Convert(staff));
                outputFile.Close();
            }
        }
    }
}
