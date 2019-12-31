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
using MiPaladar.Entities;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class SalesByItemByShift : Window
    {

        public SalesByItemByShift()
        {
            InitializeComponent();

            Loaded += SalesByItemByShift_Loaded;
        }

        void SalesByItemByShift_Loaded(object sender, RoutedEventArgs e)
        {
            SalesByItemByShiftViewModel vm = (SalesByItemByShiftViewModel)this.DataContext;
            
            foreach (var invArea in vm.Shifts)
            {
                AddColumn(invArea);
            }
        }

        void AddColumn(Shift sh)
        {
            DataGridTextColumn dgtc = new DataGridTextColumn();
            dgtc.MinWidth = 100;

            dgtc.Header = sh.Name;
            dgtc.SortMemberPath = string.Format("QuantityItems[{0}].Quantity", sh.Id);

            MultiBinding mb = new MultiBinding();
            mb.StringFormat = "{0:0.##} {1}";
            Binding b1 = new Binding(string.Format("QuantityItems[{0}].Quantity", sh.Id));
            Binding b2 = new Binding(string.Format("QuantityItems[{0}].UnitMeasure.Caption", sh.Id));
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);

            dgtc.Binding = mb;

            myDG.Columns.Add(dgtc);
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            if (!this.IsInitialized || !this.IsLoaded) return;

            DataGridColumn column1 = myDG.Columns[1];
            DataGridColumn column2 = myDG.Columns[2];

            if (rbShift1.IsChecked == true) column1.Visibility = System.Windows.Visibility.Visible;
            else column1.Visibility = System.Windows.Visibility.Collapsed;

            if (rbShift2.IsChecked == true) column2.Visibility = System.Windows.Visibility.Visible;
            else column2.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
