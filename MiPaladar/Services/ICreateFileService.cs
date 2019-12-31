using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MiPaladar.Services
{
    public interface ICreateFileService
    {
        void CreateFile(string fileName, string content);
    }

    public class CreateFileService : ICreateFileService
    {
        public void CreateFile(string fileName, string content)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(content);
            }
        }
    }
     
}
