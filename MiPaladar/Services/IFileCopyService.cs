using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MiPaladar.Services
{
    public interface IFileCopyService
    {
        void Copy(string sourcePath, string copyPath);
        void CopyImage(string original_full_path, string target_subfolder_name);
        void SaveReport(string sourceFile, DateTime dateTime, string reportsFolder);
    }
    public class FileCopyService : IFileCopyService
    {
        public void CopyImage(string original_full_path, string target_subfolder_name)
        {
            // The photo file being copied
            FileInfo fi = new FileInfo(original_full_path);

            // Absolute path to the application folder
            string appLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                App.ApplicationFolderName);

            // Absolute path to the photos folder
            string photoLocation = Path.Combine(appLocation, target_subfolder_name);

            // Fully qualified path to the new photo file
            string photoFullPath = Path.Combine(photoLocation, fi.Name);

            // Create the appLocation directory if it doesn't exist
            if (!Directory.Exists(appLocation))
                Directory.CreateDirectory(appLocation);

            // Create the photos directory if it doesn't exist
            if (!Directory.Exists(photoLocation))
                Directory.CreateDirectory(photoLocation);

            // Copy the photo.
            try
            {
                fi.CopyTo(photoFullPath, true);
            }
            catch
            {
                // Could not copy the photo. Handle all exceptions 
                // the same, ignore and continue.
            }
        }

        public void Copy(string sourcePath, string copyPath)
        {
            // The photo file being copied
            FileInfo fi = new FileInfo(sourcePath);

            if (!Directory.Exists(Path.GetDirectoryName(copyPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(copyPath));

            // Copy the photo.
            try
            {
                fi.CopyTo(copyPath, true);
            }
            catch
            {
                // Could not copy the photo. Handle all exceptions 
                // the same, ignore and continue.
            }
        }

        public void SaveReport(string sourceFile, DateTime dateTime, string reportsFolder)
        {
            string month_spanish = ((MiPaladar.Enums.Meses)(dateTime.Month - 1)).ToString();
            string folder_name = Path.Combine(reportsFolder, string.Format("{0:yyyy}\\{0:M.}{1}\\", dateTime, month_spanish));

            int file_count = 1;

            if (!Directory.Exists(folder_name))
            {
                Directory.CreateDirectory(folder_name);
            }
            else
            {
                string date_part = string.Format("{0:yyyy}-{0:MM}-{0:dd}", dateTime);

                var files = from f in Directory.EnumerateFiles(folder_name, "*.csv")
                            where Path.GetFileName(f).StartsWith(date_part)
                            select f;

                foreach (var file in files)
                {
                    string nameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                    int pos = nameWithoutExtension.IndexOf('_');

                    if (pos < 0 || pos == nameWithoutExtension.Length - 1) continue; //contains '_' and there is something after '_'

                    string fileNumber = nameWithoutExtension.Substring(pos + 1);

                    int number;

                    if (int.TryParse(fileNumber, out number))
                    {
                        if (number >= file_count) file_count = number + 1;
                    }
                }
            }

            string target_file_path = folder_name + string.Format("{0:yyyy}-{0:MM}-{0:dd}_{1}.csv", dateTime, file_count);
            Copy(sourceFile, target_file_path);
        }
    }
}
