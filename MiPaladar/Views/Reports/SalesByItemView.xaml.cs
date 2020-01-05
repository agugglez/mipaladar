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
using MiPaladar.Enums;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class SalesByItemView : Window
    {
        public SalesByItemView()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            ReportsWindowViewModel wvm = (ReportsWindowViewModel)DataContext;

            //var adasd = dateOptionCbx.SelectedItem;

            DateOption newValue = (DateOption)(sender as TextBlock).DataContext;
            wvm.SetDateOption(newValue);
        }
    }
}
