using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MiPaladar.Services
{
    public interface IFileReaderService
    {
        List<string> ReadLines(string file_path);
    }

    public class FileReaderService : IFileReaderService 
    {
        public List<string> ReadLines(string file_path) 
        {
            List<string> lines = new List<string>();

            using (FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }
    }
}
