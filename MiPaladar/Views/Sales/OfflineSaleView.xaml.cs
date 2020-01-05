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
using MiPaladar.MVVM;

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
            if (screen != null && !screen.IsSelfClosing())
                e.Cancel = !screen.TryToClose();
        }

        #endregion

        //#region Filter

        //private void KitchenProductsFilter(object sender, FilterEventArgs e)
        //{
        //    OfflineLineItemViewModel lineitem = (OfflineLineItemViewModel)e.Item;

        //    e.Accepted = lineitem.Product.IsProduced;          
        //}

        //#endregion

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OfflineSaleViewModel vale = (OfflineSaleViewModel)this.DataContext;
            PrintVale(vale);
        }

        public bool PrintVale(OfflineSaleViewModel vale)
        {
            PrintDialog pDialog = new PrintDialog();
            pDialog.UserPageRangeEnabled = false;

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                ValeToPrint control = new ValeToPrint();
                control.DataContext = vale;

                Size sz = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);

                // arrange
                control.Measure(sz);
                control.Arrange(new Rect(sz));
                control.UpdateLayout();

                Size finalSize = new Size(control.DesiredSize.Width, control.DesiredSize.Height);

                control.Measure(finalSize);
                control.Arrange(new Rect(finalSize));
                control.UpdateLayout();

                pDialog.PrintVisual(control, "Imprimiendo vale");

                return true;
            }

            return false;
        }
    }
}
