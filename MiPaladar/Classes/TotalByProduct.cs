using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows;

using MiPaladar.Entities;

namespace MiPaladar.Classes
{
    public class TotalByProduct : INotifyPropertyChanged
    {
        //total[0] -> inicio
        //total[1] -> final
        //total[2] -> total de compras
        //total[3] -> total de ventas
        //float[] total = new float[4];
        //List<Vale2> foundin = new List<Vale2>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        int productID;
        [XmlAttribute]
        public int ProductID
        {
            get { return productID; }
            set
            {
                productID = value;                
            }
        }
        //Product3 product;
        //[XmlIgnore]
        //public Product3 ProductOld
        //{
        //    get
        //    {
        //        return product;
        //    }
        //    set
        //    {
        //        product = value;
        //        productID = product.ID;
        //    }
        //}

        Product prod;
        [XmlIgnore]
        public Product Product
        {
            get
            {
                return prod;
            }
            set
            {
                prod = value;
            }
        }

        //float[] totalsArray;
        //[XmlIgnore]
        //public float[] TotalsArray
        //{
        //    get { return totalsArray; }
        //    set { totalsArray = value; }
        //}

        //[XmlIgnore]
        //public float Now
        //{
        //    get { return totalsArray[totalsArray.Length - 1]; }
            
        //}

        double total;
        [XmlAttribute]
        public double Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        } 

        string vales_tostring;
        [XmlIgnore]
        public string FoundIn
        {
            get
            {
                return vales_tostring;
            }
            set 
            {
                vales_tostring = value;
                //OnPropertyChanged("FoundIn");
            }
        }
        decimal totalCosto;
        [XmlIgnore]
        public decimal Costo
        {
            get { return totalCosto; }
            set
            {
                totalCosto = value;
                OnPropertyChanged("Costo");
            }
        }        
    }
}