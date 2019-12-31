using System;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.ViewModels;
using System.Windows.Input;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for InventoryHistory.xaml
    /// </summary>
    public partial class InventoryHistory : Window
    {
        public InventoryHistory()
        {
            InitializeComponent();
        }

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InventoryHistoryViewModel cpvm = (InventoryHistoryViewModel)this.DataContext;

                cpvm.UpdateSearch();
            }
        }
    }
}
