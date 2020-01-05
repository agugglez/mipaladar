using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPaladar.Classes
{
    public class LineOfWork
    {
        public DateTime StartDate { get; set; }
        public DateTime? PrintKitchenDate { get; set; }

        public DateTime? PrintTicketDate { get; set; }
        public bool TicketPrinted { get; set; }

        public DateTime? FacturaDate { get; set; }        

        public int ServiceTime 
        {
            get
            {
                DateTime dateToUse;
                if (PrintTicketDate > StartDate) dateToUse = PrintTicketDate.Value;
                else if (FacturaDate > StartDate) dateToUse = FacturaDate.Value;
                else return 0;                

                return (int)(dateToUse - StartDate).TotalMinutes;
            }
        }

        List<Report101Operation> ot = new List<Report101Operation>();
        public List<Report101Operation> OperationTrack
        {
            get { return ot; }
        }

        public int FacturaNumber { get; set; }
        public int TableNumber { get; set; }
        public int Clients { get; set; }        
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        //VOIDS
        public int VoidCount { get; set; }
        public int CriticalVoidCount { get; set; }
    }

    public enum Report101Operation
    {
        First, Void, Print, Factura
    }
}
