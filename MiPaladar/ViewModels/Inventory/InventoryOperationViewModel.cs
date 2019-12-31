using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class InventoryOperationViewModel : ViewModelBase
    {
        DateTime date;
        public DateTime Date 
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public DateTime DateCreated { get; set; }

        public string OperationType { get; set; }

        string inventory;
        public string Inventory 
        {
            get { return inventory; }
            set
            {
                inventory = value;
                OnPropertyChanged("Inventory");
            }
        }

        Employee responsible;
        public Employee Responsible 
        {
            get { return responsible; }
            set
            {
                responsible = value;
                OnPropertyChanged("Responsible");
            }
        }

        string memo;
        public string Memo 
        {
            get { return memo; }
            set
            {
                memo = value;
                OnPropertyChanged("Memo");
            }
        }

        public Order Order { get; set; }
    }
}
