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
    /// Interaction logic for InventoryOperationsView.xaml
    /// </summary>
    public partial class InventoryOperationsListView : UserControl
    {
        public InventoryOperationsListView()
        {
            InitializeComponent();
        }

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InventoryOperationsListViewModel ivm = (InventoryOperationsListViewModel)this.DataContext;

                ivm.FindOperations();
            }
        }
    }
}
