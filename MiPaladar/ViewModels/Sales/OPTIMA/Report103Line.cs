using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPaladar.Classes
{
    public class Report103Line
    {
        public int EJournalId { get; set; }
        public DateTime DateAndTime { get; set; }
        public int ReceiptNumber { get; set; }
        public int SalesPerson { get; set; }
        public int ClerkNumber { get; set; }
        public int FunctionType { get; set; }
        public int FunctionNumber { get; set; }
        public string FunctionText { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
