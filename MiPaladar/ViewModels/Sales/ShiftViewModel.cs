using System;

using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class ShiftViewModel : ViewModelBase
    {
        //MainWindowViewModel appvm;

        public ShiftViewModel() 
        {
        }

        int id;
        public int Id { 
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }


    }
}
