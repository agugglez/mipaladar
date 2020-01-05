using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class CategoryRowViewModel : ViewModelBase
    {
        //MainWindowViewModel appvm;

        public CategoryRowViewModel() 
        {
        }

        //source category Id
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

        public int Level { get; set; }

        public bool IsAddNew { get; set; }

        public bool IsChecked { get; set; }

    }
}
