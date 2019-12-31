using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Services;
using MiPaladar.Entities;

using System.ComponentModel;
using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class ImportDialogViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public ImportDialogViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        #region Back Command

        RelayCommand backCommand;
        public ICommand BackCommand 
        {
            get 
            {
                if (backCommand == null)
                    backCommand = new RelayCommand(x => this.GoBack(), x => this.CanGoBack);
                return backCommand;
            }
        }

        bool CanGoBack { get { return !busy; } }

        void GoBack() 
        {
            appvm.GoBack();
        }

        #endregion

        #region Select Folder Command

        string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }

        RelayCommand selectFolderCommand;
        public ICommand SelectFolderCommand
        {
            get
            {
                if (selectFolderCommand == null)
                    selectFolderCommand = new RelayCommand(x => this.SelectFolder(), x => this.CanSelectFolder);
                return selectFolderCommand;
            }
        }

        bool CanSelectFolder { get { return !busy; } }

        void SelectFolder()
        {
            var folderBrowser = base.GetService<IBrowseFolderService>();
            if (folderBrowser.ShowDialog("Seleccione la desde donde quiere exportar los datos") == true)
            {
                FolderPath = folderBrowser.SelectedPath;
            }
        }

        #endregion    
    
        bool dataError;

        public bool DataError
        {
            get { return dataError; }
            set 
            {
                dataError = value;
                OnPropertyChanged("DataError");
            }
        }

        #region Import Command

        BackgroundWorker importWorker;

        RelayCommand importCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (importCommand == null)
                {
                    importCommand = new RelayCommand(x => this.Import(), x => this.CanImport);
                }
                return importCommand;
            }
        }

        bool CanImport 
        {
            get { return !busy && !string.IsNullOrWhiteSpace(folderPath) && !dataError; }
        }

        void Import()
        {
            var importSvc = base.GetService<IXmlSerializationSvc>();
            if (!importSvc.CheckFilesForImport(folderPath))
            {
                var msgBox = base.GetService<IMessageBoxService>();
                string message = "Los datos en esta carpeta no se corresponden con esta versión del programa.";

                msgBox.ShowMessage(message);

                return;
            }
            //var msgBox = base.GetService<IMessageBoxService>();
            //string message = "Está seguro que desea IMPORTAR? Perderá todos los datos actuales";

            //if (msgBox.ShowYesNoDialog(message) != true) return;

            //var folderBrowser = base.GetService<IBrowseFolderService>();
            //if (folderBrowser.ShowDialog("Seleccione la carpeta que desea importar") == true)
            {
                //string parentFolder = folderBrowser.SelectedPath;

                var windowManager = base.GetService<IWindowManager>();
                windowManager.CloseAllWindowsButMain();

                appvm.CloseWorkspaces();

                appvm.EmptyWrappers();

                Progress = 0; Busy = true;

                if (importWorker == null)
                {
                    importWorker = new BackgroundWorker();
                    importWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    importWorker.WorkerReportsProgress = true;
                    importWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                    importWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                }

                importWorker.RunWorkerAsync(folderPath);
            }
        }

        bool CheckFiles()
        {
            var importSvc = base.GetService<IXmlSerializationSvc>();


            return true;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {            
            string parentFolder = (string)e.Argument;
            BackgroundWorker bw = sender as BackgroundWorker;

            appvm.Import(parentFolder, bw);

            //var serializer = base.GetService<IXmlSerializationSvc>();
            //serializer.Deserialize(parentFolder, importWorker);
            ////ExportImport.Import(context, parentFolder, importWorker);

            //appvm.Context = new RestaurantDBEntities();

            //appvm.LoadData();

            ////pre-load data to improve performance
            //foreach (var item in appvm.Context.Products)
            //{
            //    item.RelatedCategories.Load();
            //    item.Ingredients.Load();
            //}
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Busy = false;

            //LoginViewModel lvm = new LoginViewModel(appvm);

            //appvm.ShowAppWideDialog(lvm);

            appvm.Logout();
        }

        #endregion

        bool busy;
        public bool Busy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged("Busy");
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
    }
}
