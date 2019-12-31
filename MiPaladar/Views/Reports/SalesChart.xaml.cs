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

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class SalesChart : Window
    {
        //DataGridColumn dgDate;
        ////DataGridColumn dgOrderType;
        //DataGridColumn dgOrderName;
        //DataGridColumn dgArea;
        //DataGridColumn dgTable;
        //DataGridColumn dgWaiter;
        //DataGridColumn dgResponsible;
        //DataGridColumn dgPurchaseType;
        //DataGridColumn dgTitle;
        //DataGridColumn dgCount;
        //DataGridColumn dgClients;
        //DataGridColumn dgTotal;
        //DataGridColumn dgTotalPurchase;
        //DataGridColumn dgSalesByClient;
        //DataGridColumn dgDiscount;
        ////DataGridColumn dgNotes;

        public SalesChart()
        {
            InitializeComponent();

            //dgDate = dgOrders.Columns.Single(x => x.Header.ToString() == "Fecha");
            ////dgOrderType = dgOrders.Columns.Single(x => x.Header.ToString() == "Tipo");
            //dgOrderName = dgOrders.Columns.Single(x => x.Header.ToString() == "Vale");
            //dgArea = dgOrders.Columns.Single(x => x.Header.ToString() == "Area");
            //dgTable = dgOrders.Columns.Single(x => x.Header.ToString() == "Mesa");
            //dgWaiter = dgOrders.Columns.Single(x => x.Header.ToString() == "Dependiente");
            //dgResponsible = dgOrders.Columns.Single(x => x.Header.ToString() == "Responsable");
            //dgPurchaseType = dgOrders.Columns.Single(x => x.Header.ToString() == "Tipo");
            //dgTitle = dgOrders.Columns.Single(x => x.Header.ToString() == "Título");
            //dgCount = dgOrders.Columns.Single(x => x.Header.ToString() == "# vales");
            //dgClients = dgOrders.Columns.Single(x => x.Header.ToString() == "Clientes");
            //dgTotal = dgOrders.Columns.Single(x => x.Header.ToString() == "Importe");
            //dgTotalPurchase = dgOrders.Columns.Single(x => x.Header.ToString() == "Compras");
            //dgSalesByClient = dgOrders.Columns.Single(x => x.Header.ToString() == "Importe/Cliente");
            //dgDiscount = dgOrders.Columns.Single(x => x.Header.ToString() == "Descuento");
            ////dgNotes = dgOrders.Columns.Single(x => x.Header.ToString() == "Notas");
        }

        //void ResetColumnVisibility() 
        //{
        //    dgOrders.Columns.Clear();

        //    bool showAll = rbShowAll.IsChecked == true;
        //    bool showSales = rbShowSales.IsChecked == true;
        //    bool showPurchases = rbShowPurchases.IsChecked == true;

        //    bool noGrouping = rbNoGrouping.IsChecked == true;
        //    bool groupingByDate = rbGroupByDate.IsChecked == true;
        //    bool groupingByWaiter = rbGroupByWaiter.IsChecked == true;
        //    bool groupingByTable = rbGroupByTable.IsChecked == true;
        //    bool groupingByArea = rbGroupingByArea.IsChecked == true;

        //    if (noGrouping || groupingByDate) dgOrders.Columns.Add(dgDate);

        //    if (showSales && (noGrouping || groupingByArea)) dgOrders.Columns.Add(dgArea);

        //    if (showSales && (noGrouping || groupingByTable)) dgOrders.Columns.Add(dgTable);

        //    if (showSales && (noGrouping || groupingByWaiter)) dgOrders.Columns.Add(dgWaiter);

        //    if (showPurchases && noGrouping) dgOrders.Columns.Add(dgPurchaseType);

        //    if (showPurchases && noGrouping) dgOrders.Columns.Add(dgTitle);

        //    if (showPurchases && noGrouping) dgOrders.Columns.Add(dgResponsible);

        //    if (showSales && !noGrouping) dgOrders.Columns.Add(dgCount);

        //    if (showSales) dgOrders.Columns.Add(dgClients);

        //    if (!(showPurchases && groupingByDate)) dgOrders.Columns.Add(dgTotal);

        //    if (groupingByDate && !showSales) dgOrders.Columns.Add(dgTotalPurchase);

        //    if (showSales && groupingByWaiter) dgOrders.Columns.Add(dgSalesByClient);

        //    if (showSales && (noGrouping || groupingByDate)) dgOrders.Columns.Add(dgDiscount);

        //    //if (noGrouping) dgOrders.Columns.Add(dgNotes);

        //    if (noGrouping) dgOrders.Columns.Add(dgOrderName);
            
        //}

        //Visibility ConvertToVisibility(bool value)
        //{
        //    return value ? Visibility.Visible : Visibility.Hidden;
        //}

        //private void cbxGroupBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ResetColumnVisibility();
        //    ////no grouping
        //    //if (cbxGrouping.SelectedIndex == 0) 
        //    //{
        //    //    dgDate.Visibility = Visibility.Visible;
        //    //    dgSale.Visibility = Visibility.Visible;                
        //    //    dgTable.Visibility = Visibility.Visible;
        //    //    dgWaiter.Visibility = Visibility.Visible;
        //    //    dgCount.Visibility = Visibility.Hidden;
        //    //    dgClients.Visibility = Visibility.Visible;
        //    //    dgTotal.Visibility = Visibility.Visible;
        //    //    dgSalesByClient.Visibility = Visibility.Hidden;
        //    //    dgArea.Visibility = Visibility.Visible;
        //    //    dgDiscount.Visibility = Visibility.Visible;
        //    //}
        //    //    //by waiter
        //    //else if (cbxGrouping.SelectedIndex == 1) 
        //    //{
        //    //    dgDate.Visibility = Visibility.Hidden;
        //    //    dgSale.Visibility = Visibility.Hidden;
        //    //    dgArea.Visibility = Visibility.Hidden;
        //    //    dgTable.Visibility = Visibility.Hidden;

        //    //    dgWaiter.Visibility = Visibility.Visible;
        //    //    dgCount.Visibility = Visibility.Visible;
        //    //    dgClients.Visibility = Visibility.Visible;
        //    //    dgTotal.Visibility = Visibility.Visible;
        //    //    dgSalesByClient.Visibility = Visibility.Visible;
        //    //    dgDiscount.Visibility = Visibility.Hidden;
        //    //}
        //    //    //by table
        //    //else if (cbxGrouping.SelectedIndex == 2)
        //    //{
        //    //    dgDate.Visibility = Visibility.Hidden;
        //    //    dgSale.Visibility = Visibility.Hidden;

        //    //    dgArea.Visibility = Visibility.Hidden;
        //    //    dgTable.Visibility = Visibility.Visible;
        //    //    dgWaiter.Visibility = Visibility.Hidden;

        //    //    dgCount.Visibility = Visibility.Visible;
        //    //    dgClients.Visibility = Visibility.Visible;
        //    //    dgTotal.Visibility = Visibility.Visible;
        //    //    dgSalesByClient.Visibility = Visibility.Hidden;
        //    //    dgDiscount.Visibility = Visibility.Hidden;
        //    //}
        //    //    //by pricelist
        //    //else if (cbxGrouping.SelectedIndex == 3)
        //    //{
        //    //    dgDate.Visibility = Visibility.Hidden;
        //    //    dgSale.Visibility = Visibility.Hidden;

        //    //    dgArea.Visibility = Visibility.Visible;
        //    //    dgTable.Visibility = Visibility.Hidden;
        //    //    dgWaiter.Visibility = Visibility.Hidden;

        //    //    dgCount.Visibility = Visibility.Visible;
        //    //    dgClients.Visibility = Visibility.Visible;
        //    //    dgTotal.Visibility = Visibility.Visible;
        //    //    dgSalesByClient.Visibility = Visibility.Hidden;
        //    //    dgDiscount.Visibility = Visibility.Hidden;
        //    //}
        //}

        //private void radiobutton_Checked(object sender, RoutedEventArgs e)
        //{
        //    ResetColumnVisibility();
        //}
    }
}
