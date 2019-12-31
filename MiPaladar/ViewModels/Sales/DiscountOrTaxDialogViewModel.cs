using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPaladar.ViewModels
{
    public class DiscountOrTaxDialogViewModel : ViewModelBase
    {
        public DiscountOrTaxDialogViewModel(string title, string message, decimal quantity, bool inPercent)
        {
            this.title = title;
            this.message = message;
            this.quantity = quantity;
            this.inPercent = inPercent;
        }

        string title;

        public string Title
        {
            get { return title; }
            set 
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

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

        private decimal quantity;

        public decimal Quantity
        {
            get { return quantity; }
            set 
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        bool inPercent;

        public bool InPercent
        {
            get { return inPercent; }
            set 
            {
                inPercent = value;
                OnPropertyChanged("InPercent");
            }
        }

    }
}
