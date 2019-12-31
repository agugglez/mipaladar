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
using System.ComponentModel;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for Vale.xaml
    /// </summary>
    public partial class OfflineSaleView : Window
    {
        public OfflineSaleView()
        {
            InitializeComponent();

            this.DataContextChanged += UserControl_DataContextChanged; 
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

        #region Filter

        private void KitchenProductsFilter(object sender, FilterEventArgs e)
        {
            OfflineLineItemViewModel lineitem = (OfflineLineItemViewModel)e.Item;

            e.Accepted = lineitem.Product.IsProduced;          
        }

        #endregion

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is OfflineSaleViewModel)
            {
                OfflineSaleViewModel viewmodel = (OfflineSaleViewModel)e.NewValue;
                viewmodel.LineItemAdded += new EventHandler(viewmodel_LineItemAdded);
            }
            else if (e.OldValue is OfflineSaleViewModel)
            {
                OfflineSaleViewModel viewmodel = (OfflineSaleViewModel)e.OldValue;
                viewmodel.LineItemAdded -= viewmodel_LineItemAdded;
            }
        }

        void viewmodel_LineItemAdded(object sender, EventArgs e)
        {
            //Keyboard.Focus(tbQuantity);            
            tbQuantity.SelectAll();
            tbQuantity.Focus();
            
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
