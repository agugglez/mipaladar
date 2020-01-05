using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for AddLineDialog.xaml
    /// </summary>
    public partial class AddLineDialog : Window
    {
        public AddLineDialog()
        {
            InitializeComponent();
        }        

        private void AutoCompleteBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tbQuantity.SelectAll();
                tbQuantity.Focus();                
            }
        }

        private void tbQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (cbxUM.IsVisible) cbxUM.Focus();
                else btnAdd.Focus();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            acbProduct.Focus();
        }

        private void cbxUM_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnAdd.Focus();
            }            
        }
    }
}
