using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Classes;
using MiPaladar.Entities;

using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class TotalsByWaiterViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public TotalsByWaiterViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }

        ObservableCollection<TotalByDependiente> shortTotalsWaiter = new ObservableCollection<TotalByDependiente>();

        public void ShortUpdateTotalsByWaiter(DateTime date)
        {
            ShortTotalsByWaiter.Clear();

            DateTime fromDate = date.Date;
            DateTime toDate = fromDate.AddDays(1);

            var prodCountQuery = from vale in appvm.Context.Orders.OfType<Sale>()
                                 where vale.Date >= fromDate && vale.Date < toDate
                                 group vale by vale.Employee into grouping
                                 select new TotalByDependiente
                                 {
                                     Dependiente = grouping.Key == null ? string.Empty : grouping.Key.Name,
                                     //TotalVentas = grouping.Sum(v => v.TotalPrice),
                                     TotalMesas = grouping.Count(),
                                     ClientesAtendidos = grouping.Sum(v => v.Paid ? v.Persons : 0),
                                     ClientesSinAtender = grouping.Sum(v => !v.Paid ? v.Persons : 0)
                                 };

            foreach (var tbd in prodCountQuery)
            {
                shortTotalsWaiter.Add(tbd);
            }
        }

        public ObservableCollection<TotalByDependiente> ShortTotalsByWaiter
        {
            get { return shortTotalsWaiter; }
        }
    }
}
