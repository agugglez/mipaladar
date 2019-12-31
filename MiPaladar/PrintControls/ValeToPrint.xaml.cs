using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiPaladar.PrintControls
{
    /// <summary>
    /// Interaction logic for ValeToPrint.xaml
    /// </summary>
    public partial class ValeToPrint : UserControl
    {
        public ValeToPrint()
        {
            InitializeComponent();

            //this.DataContextChanged += new DependencyPropertyChangedEventHandler(ValeToPrint_DataContextChanged);
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            DataGridColumn column1 = itemsDG.Columns[0];
            DataGridColumn column2 = itemsDG.Columns[1];
            DataGridColumn column3 = itemsDG.Columns[2];

            column2.Width = this.ActualWidth - column1.Width.DisplayValue - column3.Width.DisplayValue;
        }

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
