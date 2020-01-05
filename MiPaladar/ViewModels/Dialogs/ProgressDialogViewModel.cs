using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using MiPaladar.Services;
using MiPaladar.MVVM;

using System.Windows.Input;
using System.IO;

namespace MiPaladar.ViewModels
{
    public class ProgressDialogViewModel : ViewModelBase
    {
        //MainWindowViewModel appvm;
        //public ProgressDialogViewModel(string message)
        //{
        //    this.message = message;
        //}

        //#region Select Folder Command

        //RelayCommand selectFolderCommand;
        //public ICommand SelectFolderCommand 
        //{
        //    get 
        //    {
        //        if (selectFolderCommand == null)
        //            selectFolderCommand = new RelayCommand(x => this.SelectFolder(), x => this.CanSelectFolder);
        //        return selectFolderCommand; 
        //    }
        //}

        //bool CanSelectFolder { get { return !IsBusy; } }

        //void SelectFolder() 
        //{
        //    var folderBrowser = base.GetService<IBrowseFolderService>();
        //    if (folderBrowser.ShowDialog("Seleccione la carpeta a donde quiere exportar los datos") == true)
        //    {
        //        FolderPath = folderBrowser.SelectedPath;
        //    }
        //}

        //#endregion

        //#region Export Command

        //string folderPath;
        //public string FolderPath
        //{
        //    get { return folderPath; }
        //    set 
        //    {
        //        folderPath = value;
        //        OnPropertyChanged("FolderPath");
        //    }
        //}

        string message;
        public string Message 
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        bool isbusy;
        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                isbusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        int progress;
        public int Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged("Progress");
                }
            }
        }

        //BackgroundWorker exportWorker;

        //RelayCommand exportCommand;
        //public ICommand ExportCommand
        //{
        //    get
        //    {
        //        if (exportCommand == null)
        //        {
        //            exportWorker = new BackgroundWorker();
        //            exportWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
        //            exportWorker.WorkerReportsProgress = true;
        //            exportWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        //            exportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

        //            exportCommand = new RelayCommand(o => this.Export(), o => this.CanExport);
        //        }
        //        return exportCommand;
        //    }
        //}

        //bool CanExport { get { return !string.IsNullOrWhiteSpace(folderPath) && !IsBusy; } }

        //void Export()
        //{
        //    string newFolder = folderPath + "\\Salva " + DateTime.Now.ToString("yyyy-MM-dd, h 'y' mm tt");

        //    if (Directory.Exists(newFolder))
        //    {
        //        var msgBox = base.GetService<IMessageBoxService>();
        //        msgBox.ShowMessage("No se pueden exportar los datos, ya existe una carpeta con la misma fecha.");
        //        return;
        //    }

        //    Directory.CreateDirectory(newFolder);

        //    Progress = 0;

        //    IsBusy = true;

        //    if (exportWorker == null)
        //    {
        //        exportWorker = new BackgroundWorker();
        //        exportWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
        //        exportWorker.WorkerReportsProgress = true;
        //        exportWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        //        exportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        //    }

        //    exportWorker.RunWorkerAsync(newFolder);
        //}

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    string folder = (string)e.Argument;
        //    appvm.XMLSerializer.Serialize(folder, exportWorker);
        //    //ExportImport.Export(context, folder, exportWorker);
        //}
        //private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    Progress = e.ProgressPercentage;
        //}
        //void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    IsBusy = false;
        //    //messageBoxService.ShowMessage("Terminado");
        //}

        //#endregion

        
    }
}