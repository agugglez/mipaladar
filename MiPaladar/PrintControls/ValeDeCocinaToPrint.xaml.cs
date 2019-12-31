using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

using MiPaladar.ViewModels;

namespace MiPaladar.PrintControls
{
    /// <summary>
    /// Interaction logic for ValeToPrint.xaml
    /// </summary>
    public partial class ValeDeCocinaToPrint : UserControl
    {
        public ValeDeCocinaToPrint()
        {
            InitializeComponent();

            //this.DataContextChanged += new DependencyPropertyChangedEventHandler(ValeToPrint_DataContextChanged);
        }

        #region Filter

        private void KitchenProductsFilter(object sender, FilterEventArgs e)
        {
            LineItemViewModel lineitem = (LineItemViewModel)e.Item;

            e.Accepted = lineitem.Product.IsProduced && !lineitem.Printed;
        }

        #endregion

        //void ValeToPrint_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    Vale3 vale = e.NewValue as Vale3;

        //    foreach (ValeItem vi in vale.ValeItems) 
        //    {
        //        dgPrintvale.Items.Add(vi);
        //    }

        //    ValeItem ajuste = new ValeItem();
        //    string ad = vale.Ajuste <= 0 ? "descuento" : "gravamen";

        //    ajuste.Product = new classes.Product2() { Name = ad };
        //    ajuste.Price = vale.Ajuste;

        //    dgPrintvale.Items.Add(ajuste);            

        //    ValeItem total = new ValeItem();

        //    total.Product = new classes.Product2() { Name = "Total" };
        //    total.Price = vale.TotalPrice;

        //    dgPrintvale.Items.Add(total);
        //}
    }
}
