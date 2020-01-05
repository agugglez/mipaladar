using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace MiPaladar.Classes
{
    public class TotalByMesa : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        string mesa;
        public string Mesa
        {
            get { return mesa; }
            set
            {
                mesa = value;
                OnPropertyChanged("Mesa");
            }
        }
                
        decimal totalventas;
        public decimal TotalVentas
        {
            get
            {
                return totalventas;
            }
            set
            {
                totalventas = value;
                OnPropertyChanged("TotalVentas");
            }
        }
        int totalclientes;
        public int TotalClientes
        {
            get
            {
                return totalclientes;
            }
            set
            {
                totalclientes = value;
                OnPropertyChanged("TotalClientes");
            }
        }        
    }
}
