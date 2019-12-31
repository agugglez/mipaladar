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
    public class TotalesPorMesaViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        TotalesViewModel totalesParent;

        public TotalesPorMesaViewModel(TotalesViewModel totalesParent, MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
            this.totalesParent = totalesParent;
        }

        public void UpdateTotals()
        {
            //totals by table
            var orders = from o in appvm.Context.Orders.OfType<Sale>()
                         where o.DateCreated >= totalesParent.FromDate && o.DateCreated <= totalesParent.ToDate
                         select o;

            totalsMesa.Clear();
            foreach (var item in Accounter.UpdateTotalsByMesa(orders))
            {
                totalsMesa.Add(item);
            }
        }

        ObservableCollection<TotalByMesa> totalsMesa = new ObservableCollection<TotalByMesa>();
        public ObservableCollection<TotalByMesa> TotalsMesa
        {
            get { return totalsMesa; }
        } 
    }
}
