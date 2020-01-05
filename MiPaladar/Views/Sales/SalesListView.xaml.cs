using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MiPaladar.ViewModels;
using MiPaladar.Enums;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for VentasControl.xaml
    /// </summary>
    public partial class SalesListView : UserControl
    {
        enum MenuOption { MiniMenu, MiniInventory, TotalsWaiter};

        public SalesListView()
        {
            InitializeComponent(); 
        }

        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            SalesListViewModel wvm = (SalesListViewModel)DataContext;

            //var adasd = dateOptionCbx.SelectedItem;

            DateOption newValue = (DateOption)(sender as TextBlock).DataContext;
            wvm.SetDateOption(newValue);
        }

        //private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Return)
        //    {
        //        SalesListViewModel slvm = (SalesListViewModel)this.DataContext;

        //        slvm.FindSales();
        //    }
        //}        
    }
}
