using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSTable
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public bool IsBar { get; set; }
        public int? AreaId { get; set; }
    }
}
