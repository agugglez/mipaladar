using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using MiPaladar.Entities;

namespace MiPaladar.Classes
{
    public class TotalByCentro : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        PriceList centro;
        public PriceList Centro
        {
            get { return centro; }
            set
            {
                centro = value;
                OnPropertyChanged("Centro");
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

        int totalMesas;
        public int TotalMesas
        {
            get
            {
                return totalMesas;
            }
            set
            {
                totalMesas = value;
                OnPropertyChanged("TotalMesas");
            }
        }
    }
}
