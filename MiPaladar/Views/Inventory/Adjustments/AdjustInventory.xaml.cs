using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for AdjustInventory.xaml
    /// </summary>
    public partial class AdjustInventory : Window
    {
        public AdjustInventory()
        {
            InitializeComponent();

            this.Closing += this.HandleClosing;
        }

        #region Closing

        void HandleClosing(object sender, CancelEventArgs e)
        {
            var aivm = base.DataContext as AdjustInventoryViewModel;
            if (aivm != null && !aivm.SelfClosing)
            {
                var screen = base.DataContext as IScreen;
                if (screen != null) 
                    e.Cancel = !screen.TryToClose();
            }
        }

        #endregion
    }
}
