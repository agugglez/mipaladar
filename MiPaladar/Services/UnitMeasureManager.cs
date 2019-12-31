using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.ViewModels;
using MiPaladar.Entities;

namespace MiPaladar.Services
{
    public class UnitMeasureManager
    {
        MainWindowViewModel appvm;

        public UnitMeasureManager(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            InitUnitMeasures();
        }

        /// <summary>
        /// returns a quantity expressed in the base unit measure of the UMFamily
        /// i.e. 5kg=5000g
        /// </summary>
        public static double GetQuantityInUMFamilyBase(double qtty, UnitMeasure um)
        {
            return um.IsFamilyBase ? qtty : qtty * um.ToBaseConversion;
        }

        //public static UnitMeasure GetBestFittingUM(Product prod, double qtty, out double new_qtty) 
        //{
        //    UnitMeasure baseUM = prod.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
        //    //note the minus, it will order in descending order
        //    //var ordered = baseUM.UMFamily.UnitMeasures.OrderBy(x => -x.ToBaseConversion);
        //    var ordered = from um in baseUM.UMFamily.UnitMeasures
        //              orderby um.ToBaseConversion descending
        //              select um;

        //    foreach (var um in ordered)
        //    {
        //        double qtty_in_um = qtty / um.ToBaseConversion;

        //        if (Math.Abs(qtty_in_um) >= 1)
        //        {
        //            new_qtty = qtty_in_um;
        //            return um;
        //        }
        //    }

        //    new_qtty = qtty;
        //    return baseUM;
        //}

        void InitUnitMeasures() 
        {
            var um_families = appvm.Context.UMFamilies;
            foreach (var item in um_families)
            {
                if (item.Name == "Quantity") Quantity = item;
                if (item.Name == "Weight") Weight = item;
            }

            var unit_measures = appvm.Context.UnitMeasures;
            foreach (var item in unit_measures)
            {
                if (item.Caption == string.Empty || item.Caption=="u") Unit = item;
                else if (item.Caption == "g") G = item;
                else if (item.Caption == "kg") Kg = item;
            }
        }

        public UnitMeasure GetUnitMeasureFromCaption(string caption)
        {            
            string mycopy = caption.ToLower();

            if (mycopy == string.Empty || mycopy == "u") return this.Unit;
            if (mycopy == "g") return this.G;
            if (mycopy == "kg") return this.Kg;

            throw new ArgumentException("dont't know the specified Unit Measure");
        }

        //unit measure families
        public UMFamily Quantity { get; set; }
        public UMFamily Weight { get; set; }

        //unit measures
        public UnitMeasure Unit { get; set; }
        public UnitMeasure G { get; set; }
        public UnitMeasure Kg { get; set; }
    }
}
