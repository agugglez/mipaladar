using System;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace MiPaladar.ViewModels
{
    public class LoadFromRegisterDialogViewModel : ViewModelBase
    {
        public LoadFromRegisterDialogViewModel()
        {
            SelectedDate = DateTime.Today;

            XMode = true;
        }        

        //date
        public DateTime SelectedDate { get; set; }

        //shifts
        //public ObservableCollection<Shift> Shifts 
        //{
        //    get { return new ObservableCollection<Shift>(appvm.Context.Shifts); }
        //}

        //public Shift SelectedShift { get; set; }
        
        //public string IP { get; set; }

        //public bool IsValidIP(string ip_string)
        //{
        //    string pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

        //    Regex rgx = new Regex(pattern);

        //    Match m = rgx.Match(ip_string);

        //    return m.Success;
        //}

        bool xMode;
        public bool XMode 
        {
            get { return xMode; }
            set 
            {
                xMode = value;
                OnPropertyChanged("XMode");
            }
        }

        bool zMode;
        public bool ZMode
        {
            get { return zMode; }
            set
            {
                zMode = value;
                OnPropertyChanged("ZMode");
            }
        }

        //#region OK Command

        //RelayCommand okCommand;
        //public ICommand OKCommand
        //{
        //    get
        //    {
        //        if (okCommand == null)
        //            okCommand = new RelayCommand(x => { }, x => this.CanLoad);

        //        return okCommand;
        //    }
        //}

        //bool CanLoad
        //{
        //    get { return !string.IsNullOrWhiteSpace(IP) && IsValidIP(IP); }
        //}

        //#endregion        
    }
}
