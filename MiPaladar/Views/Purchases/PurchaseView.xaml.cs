using System;
using System.Windows.Input;
using System.Windows;

using System.ComponentModel;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for CompraView.xaml
    /// </summary>
    public partial class PurchaseView : Window
    {
        public PurchaseView()
        {
            InitializeComponent();

            this.DataContextChanged += this.UserControl_DataContextChanged;
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

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is PurchaseViewModel)
            {
                PurchaseViewModel viewmodel = (PurchaseViewModel)e.NewValue;
                viewmodel.LineItemAdded += new EventHandler(viewmodel_LineItemAdded);
            }
            else if (e.OldValue is PurchaseViewModel)
            {
                PurchaseViewModel viewmodel = (PurchaseViewModel)e.OldValue;
                viewmodel.LineItemAdded -= viewmodel_LineItemAdded;
            }
        }

        void viewmodel_LineItemAdded(object sender, EventArgs e)
        {
            //Keyboard.Focus(tbQuantity);
            tbQuantity.Focus();
            tbQuantity.SelectAll();

            acbProduct.Text = string.Empty;
        }

        bool quantityDown;

        private void tbQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                quantityDown = true;
            }
        }

        private void tbQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (quantityDown && e.Key == Key.Return)
            {
                quantityDown = false;
                acbProduct.Focus();
            }
        } 

        private void AutoCompleteBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //Keyboard.Focus(btnAdd);
                btnAdd.Focus();
            }
        }
    }
}
