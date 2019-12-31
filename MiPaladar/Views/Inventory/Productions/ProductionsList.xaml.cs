using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentsListView.xaml
    /// </summary>
    public partial class ProductionsList : Window
    {
        public ProductionsList()
        {
            InitializeComponent();
        }

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ProductionsListViewModel cpvm = (ProductionsListViewModel)this.DataContext;

                cpvm.FindProductions();
            }
        }
    }
}
