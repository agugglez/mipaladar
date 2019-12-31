using System;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;

namespace MiPaladar.ViewModels
{
    public class LoadReportDialogViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public LoadReportDialogViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            SelectedDate = DateTime.Today;
        }

        //complete path to file
        public string FilePath { get; set; }

        //file name
        string fileName;

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        #region Search File Command

        RelayCommand searchFileCommand;
        public ICommand SearchFileCommand
        {
            get
            {
                if (searchFileCommand == null)
                    searchFileCommand = new RelayCommand(x => this.SearchFile());
                return searchFileCommand;
            }
        }

        void SearchFile()
        {
            var open_file_dialog = base.GetService<IOpenFileDialogService>();

            string title = "Buscar fichero de reporte";
            string filter = "Reportes (.csv)|*.csv";

            if (open_file_dialog.ShowDialog(title, filter) == true)
            {
                FilePath = open_file_dialog.FileName;
                FileName = Path.GetFileName(FilePath);
            }
        }

        #endregion

        //date
        public DateTime SelectedDate { get; set; }

        //shifts
        //public ObservableCollection<Shift> Shifts 
        //{
        //    get { return new ObservableCollection<Shift>(appvm.Context.Shifts); }
        //}

        //public Shift SelectedShift { get; set; }


        RelayCommand loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (loadCommand == null)
                    loadCommand = new RelayCommand(x => this.DoLoad(), x => this.CanLoad);

                return loadCommand;
            }
        }

        bool CanLoad
        {
            get { return FileName != null; }
        }

        void DoLoad()
        {
            //do nothing
        }
    }
}
