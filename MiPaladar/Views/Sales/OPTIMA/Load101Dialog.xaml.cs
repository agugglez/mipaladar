using System;
using System.Windows;
using System.Windows.Controls;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for SelectTable.xaml
    /// </summary>
    public partial class Load101Dialog : Window
    {
        public Load101Dialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
