using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ChargeDialog.xaml
    /// </summary>
    public partial class ChargeDialog : Window
    {
        public ChargeDialog()
        {
            InitializeComponent();
        }

        private void tbxCash_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BindingExpression be = tbxCash.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }

        private void tbxTips_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BindingExpression be = tbxTips.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
