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

using MiPaladar.Classes;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for PtsVentaPage.xaml
    /// </summary>
    public partial class TablesList : UserControl
    {
        public TablesList()
        {
            InitializeComponent();
        }

        //private void add_listaPrecio_Click(object sender, RoutedEventArgs e)
        //{
        //    if (tbListaPrecio.Text.Trim() == String.Empty)
        //        MessageBox.Show("no ha escrito un nombre para la nueva lista de precios");
        //    else
        //    {
        //        PriceList lp = new PriceList();
        //        lp.Name = tbListaPrecio.Text.Trim();
        //        App.Ctx.PriceListsOC.Add(lp);

        //        tbListaPrecio.Clear();
        //        //save the changes
        //        //SerializeDependientes();
        //    }
        //}
    }
}
