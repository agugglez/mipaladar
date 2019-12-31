using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MiPaladar.ViewModels;
using MiPaladar.Entities;

using System.ComponentModel;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentView.xaml
    /// </summary>
    public partial class FaenaView : Window
    {
        public FaenaView()
        {
            InitializeComponent();

            this.Loaded += this.Window_Loaded;
            this.Closing += this.HandleClosing;
        }

        #region Closing

        void HandleClosing(object sender, CancelEventArgs e)
        {
            var screen = base.DataContext as IScreen;
            if (screen != null)
                e.Cancel = !screen.TryToClose();
        }

        #endregion

        private void tbQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                acbProduct.Focus();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //FaenaViewModel fvm = (FaenaViewModel)this.DataContext;
            //Product usedProduct = fvm.ProductToUse;
            //if (usedProduct != null) acbProduct.Text = usedProduct.Name;
        }
    }
}
