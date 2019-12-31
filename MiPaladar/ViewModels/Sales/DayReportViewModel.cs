using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Extensions;

using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class DayReportViewModel : ViewModelBase
    {
        public DayReportViewModel(MainWindowViewModel appvm, DateTime workingDate)
        {
            DoMath(appvm, workingDate);
        }

        private void DoMath(MainWindowViewModel appvm, DateTime workingDate) 
        {
            DateTime fromDate = workingDate.Date;
            DateTime toDate = fromDate.AddDays(1);

            var query = from sale in appvm.Context.Orders.OfType<Sale>()
                        where sale.Date >= fromDate && sale.Date < toDate
                        orderby sale.DateCreated
                        select sale;
            
            //MinVale = int.MaxValue; MaxVale = int.MinValue;

            foreach (var item in query)
            {
                //if (MinVale > item.Number) MinVale = item.Number;
                //if (MaxVale < item.Number) MaxVale = item.Number;

                if (item.Paid) 
                {
                    TotalMesas++;
                    TotalClients += item.Persons;

                    //RawTotal += item.SubTotal;
                    SalesTotal += item.Total;

                    TaxesTotal += item.TaxToMoney();
                    DiscountsTotal += item.DiscountToMoney();                    

                    Tips += item.Tips;
                }                
            }

            if (query.Count() > 0)
            {
                MinVale = query.First().Number;
                MaxVale = query.ToList().Last().Number;
            }

            //if (MinVale == int.MaxValue) MinVale = null;
            //if (MaxVale == int.MinValue) MaxVale = null;

            Date = workingDate;
            User = appvm.LoggedInUser;

            Misc companyInfo = appvm.Context.Miscs.First();
            Fondo = companyInfo.StartingShiftAmount;

            SMT = SalesTotal - TaxesTotal;
            Entregar = SMT + Fondo;
        }

        public DateTime Date { get; set; }
        public Employee User { get; set; }

        public int TotalMesas { get; set; }
        public int TotalClients { get; set; }

        public int? MinVale { get; set; }
        public int? MaxVale { get; set; }

        //public decimal RawTotal { get; set; }
        public decimal DiscountsTotal { get; set; }
        //sales minus taxes
        public decimal SMT { get; set; }
        public decimal TaxesTotal { get; set; }
        public decimal SalesTotal { get; set; }
        public decimal Fondo { get; set; }        
        public decimal Entregar { get; set; }
        public decimal Tips { get; set; }

        #region PrintCommand

        RelayCommand printCommand;
        public ICommand PrintCommand
        {
            get
            {
                if (printCommand == null)
                    printCommand = new RelayCommand(x => this.Print());
                return printCommand;
            }
        }

        void Print()
        {
            Printer.PrintDayReport(this);
        }

        #endregion
    }
}
