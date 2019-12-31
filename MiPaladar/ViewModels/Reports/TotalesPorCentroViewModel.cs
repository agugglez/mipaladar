using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.Entities;

using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class TotalesPorCentroViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        TotalesViewModel totalesParent;

        public TotalesPorCentroViewModel(TotalesViewModel totalesParent, MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            this.totalesParent = totalesParent;
        }

        public void UpdateTotals()
        {
            //totals by place
            var orders = from o in appvm.Context.Orders.OfType<Sale>()
                         where o.DateCreated >= totalesParent.FromDate && o.DateCreated <= totalesParent.ToDate
                         select o;

            totalsCentro.Clear();
            foreach (var item in Accounter.UpdateTotalsByCentro(orders))
            {
                totalsCentro.Add(item);
            }
        }

        ObservableCollection<TotalByCentro> totalsCentro = new ObservableCollection<TotalByCentro>();
        public ObservableCollection<TotalByCentro> TotalsCentro
        {
            get { return totalsCentro; }
        }
    }
}
