using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MiPaladar.ViewModels;

using System.ComponentModel;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for AdjustmentView.xaml
    /// </summary>
    public partial class AdjustmentView : Window
    {
        public AdjustmentView()
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
            if (e.NewValue is AdjustmentViewModel)
            {
                AdjustmentViewModel viewmodel = (AdjustmentViewModel)e.NewValue;
                viewmodel.NewItemAdded += new EventHandler(viewmodel_NewItemAdded);
            }
            //if (e.OldValue is AdjustmentViewModel)
            //{
            //    AdjustmentViewModel viewmodel = (AdjustmentViewModel)e.OldValue;
            //    viewmodel.LineItemAdded -= viewmodel_LineItemAdded;
            //}
        }

        void viewmodel_NewItemAdded(object sender, EventArgs e)
        {
            //Keyboard.Focus(tbQuantity);
            tbQuantity.Focus();
            tbQuantity.SelectAll();

            acbProduct.Text = "";
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
