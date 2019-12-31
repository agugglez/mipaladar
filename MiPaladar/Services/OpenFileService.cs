using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;

namespace MiPaladar.Services
{
    public interface IOpenFileDialogService 
    {
        bool? ShowDialog(string title, string filter);
        string FileName { get; set; }
    }

    public class OpenFileDialogService : IOpenFileDialogService
    {
        public bool? ShowDialog(string title, string filter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = title;
            fileDialog.Filter = filter;
            

            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                FileName = fileDialog.FileName;
            }
            else FileName = null;
            
            return result;
        }

        public string FileName { get; set; }
    }
}
