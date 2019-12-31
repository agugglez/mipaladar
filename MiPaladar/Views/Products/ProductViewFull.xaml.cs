using System;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.ViewModels;

using System.ComponentModel;
using System.Windows.Input;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ProductViewQuick.xaml
    /// </summary>
    public partial class ProductViewFull : Window
    {
        public ProductViewFull()
        {
            InitializeComponent();

            tbxName.Focus();

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
    }
}
