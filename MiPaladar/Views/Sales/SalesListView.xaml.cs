using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MiPaladar.ViewModels;

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

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SalesListViewModel slvm = (SalesListViewModel)this.DataContext;

                slvm.FindSales();
            }
        }        
    }
}
