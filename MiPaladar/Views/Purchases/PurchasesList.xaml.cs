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

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ComprasControl.xaml
    /// </summary>
    public partial class PurchasesList : UserControl
    {
        public PurchasesList()
        {
            InitializeComponent();

            //valeControl.Content = dgTotals.ItemsSource = App.Compras.Vales;

            //dpDate.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dpDate_SelectedDateChanged);

            //dpDate.SelectedDate = DateTime.Today;
        }

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Return)
            //{
            //    PurchasesListViewModel slvm = (PurchasesListViewModel)this.DataContext;

            //    slvm.LoadCompras();
            //}
        }

        //public void ChangeDate(DateTime date) 
        //{
        //    dpDate.SelectedDate = date;
        //}

        //private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (dpDate.SelectedDate.HasValue)             
        //    {
        //        App.StartDay(dpDate.SelectedDate.Value);
        //    }            
        //}

        //private void NewVale_Click(object sender, RoutedEventArgs e)
        //{
        //    App.Ctx.CreateNewCompra();
        //}

        //private void RemoveVale_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!(MessageBox.Show("Esta seguro que desea eliminar el vale actual?", "",
        //        MessageBoxButton.YesNo) == MessageBoxResult.Yes))
        //    {
        //        e.Handled = true;
        //        //App.Ctx.RemoveCurrentCompra();
        //    } 
        //}

        //private void Print_Click(object sender, RoutedEventArgs e)
        //{

        //}        
    }
}
