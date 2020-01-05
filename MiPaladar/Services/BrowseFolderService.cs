using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace MiPaladar.Services
{
    public interface IBrowseFolderService
    {
        bool? ShowDialog(string description);
        string SelectedPath { get; set; }
    }
    public class BrowseFolderService : IBrowseFolderService
    {
        public bool? ShowDialog(string description) 
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            folderDialog.Description = description;
            
            if(folderDialog.ShowDialog()==DialogResult.OK)
            {
                SelectedPath = folderDialog.SelectedPath;
                return true;
            }

            return false;
        }
        public string SelectedPath { get; set; }
    }
}
