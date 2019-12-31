using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace MiPaladar.Classes
{
    public class TotalByDependiente : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }        

        string dependiente;
        public string Dependiente
        {
            get { return dependiente; }
            set
            {
                dependiente = value;
                OnPropertyChanged("Dependiente");
            }
        }

        int totalmesas;
        public int TotalMesas
        {
            get
            {
                return totalmesas;
            }
            set 
            {
                totalmesas = value;
                OnPropertyChanged("TotalMesas");
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

        public int TotalClients
        {
            get { return clientesAtendidos + clientesAtendiendo; }
        }

        int clientesAtendidos;
        public int ClientesAtendidos
        {
            get
            {
                return clientesAtendidos;
            }
            set
            {
                clientesAtendidos = value;
                //OnPropertyChanged("ClientesAtendidos");
            }
        }        

        int clientesAtendiendo;
        public int ClientesSinAtender 
        {
            get { return clientesAtendiendo; }
            set 
            {
                clientesAtendiendo = value;
                //OnPropertyChanged("ClientesAtendiendo");
            }
        }

        public decimal VentasPorCliente
        {
            get 
            {
                if (TotalClients == 0) return 0;
                return totalventas / TotalClients; 
            }
        }

        public decimal VentasPorMesa 
        {
            get 
            {
                if (totalmesas == 0) return 0;
                return totalventas / totalmesas; 
            }
        }
    }
}
