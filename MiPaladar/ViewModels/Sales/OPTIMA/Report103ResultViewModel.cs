using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.MVVM;
using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class Report103ResultViewModel : ViewModelBase
    {
        public Report103ResultViewModel(List<Sale> saleList, string filePath)
        {
            FilePath = filePath;

            foreach (var item in saleList)
            {
                TotalClients += item.Persons;
                TotalSales += item.Total;
                TotalDiscount += item.Discount;

                VoidCount += item.Voids;
                CriticalVoidCount += item.VoidsAfterReceipt;

                //factura with voids after receipt
                if (item.VoidsAfterReceipt > 0)
                {
                    if (string.IsNullOrWhiteSpace(CriticalVoidFacturas))
                    {
                        CriticalVoidFacturas = item.Number.ToString();
                    }
                    else
                    {
                        CriticalVoidFacturas += ", " + item.Number;
                    }
                }                
            }
        }

        public string FilePath { get; set; }

        public decimal TotalClients { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalDiscount { get; set; }

        //voids
        public int VoidCount { get; set; }
        public int CriticalVoidCount { get; set; } //voids after printing a ticket
        public string CriticalVoidFacturas { get; set; }
    }
}
