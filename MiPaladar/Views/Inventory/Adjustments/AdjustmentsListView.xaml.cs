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
    public partial class AdjustmentsListView : Window
    {
        public AdjustmentsListView()
        {
            InitializeComponent();
        }

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AdjustmentsListViewModel cpvm = (AdjustmentsListViewModel)this.DataContext;

                cpvm.FindAdjustments();
            }
        }
    }
}
