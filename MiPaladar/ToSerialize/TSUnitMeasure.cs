using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSUMnitMeasure
    {
        public int Id { get; set; }
        public int UMFamilyId { get; set; }

        public string Caption { get; set; }
        public string Name { get; set; }

        public bool IsFamilyBase { get; set; }
        public double ToBaseConversion { get; set; }
    }
}
