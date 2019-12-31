using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool CanSell { get; set; }
        public bool CanPurchase { get; set; }
        public string ImageFileName { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
    }
}
