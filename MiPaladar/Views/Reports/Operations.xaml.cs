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
    public partial class Operations : Window
    {
        //DataGridColumn dateColumn;
        ////DataGridColumn typeColumn;        
        //DataGridColumn productColumn;

        //DataGridColumn quantityColumn;
        //DataGridColumn priceColumn;

        //DataGridColumn quantitySoldColumn;
        //DataGridColumn salePriceColumn;
        //DataGridColumn quantityPurchasedColumn;        
        //DataGridColumn purchasePriceColumn;

        //DataGridColumn ingredientQuantityColumn;
        //DataGridColumn ingredientColumn;
        
        //DataGridColumn waiterColumn;
        //DataGridColumn orderColumn;
        //DataGridColumn areaColumn;

        //DataGridColumn responsibleColumn;
        //DataGridColumn purchaseTypeColumn;

        //bool productosDesglosados;

        public Operations()
        {
            InitializeComponent();

            //dateColumn = myDG.Columns.Single(x => x.Header.ToString() == "Fecha");
            ////typeColumn = myDG.Columns.Single(x => x.Header.ToString() == "Tipo");   
            //productColumn = myDG.Columns.Single(x => x.Header.ToString() == "Producto");
            //quantityColumn = myDG.Columns.Single(x => x.Header.ToString() == "Cantidad");
            //priceColumn = myDG.Columns.Single(x => x.Header.ToString() == "Precio");

            //quantitySoldColumn = myDG.Columns.Single(x => x.Header.ToString() == "Venta (Cant.)");
            //salePriceColumn = myDG.Columns.Single(x => x.Header.ToString() == "Venta ($)");
            //quantityPurchasedColumn = myDG.Columns.Single(x => x.Header.ToString() == "Compra (Cant.)");
            //purchasePriceColumn = myDG.Columns.Single(x => x.Header.ToString() == "Compra ($)");

            //ingredientQuantityColumn = myDG.Columns.Single(x => x.Header.ToString() == "Cant. Ing.");
            //ingredientColumn = myDG.Columns.Single(x => x.Header.ToString() == "Ingrediente");
            
            //waiterColumn = myDG.Columns.Single(x => x.Header.ToString() == "Dependiente");
            //orderColumn = myDG.Columns.Single(x => x.Header.ToString() == "Vale");
            //areaColumn = myDG.Columns.Single(x => x.Header.ToString() == "Area");

            //responsibleColumn = myDG.Columns.Single(x => x.Header.ToString() == "Responsable");
            //purchaseTypeColumn = myDG.Columns.Single(x => x.Header.ToString() == "Tipo");
        }

        //void ResetColumnsVisibility() 
        //{
        //    myDG.Columns.Clear();

        //    bool noGrouping = rbNoGrouping.IsChecked == true;
        //    bool groupingByProduct = rbGroupByProduct.IsChecked == true;
        //    bool groupingByDate = rbGroupByDate.IsChecked == true;
        //    bool groupingByEmployee = rbGroupByEmployee.IsChecked == true;

        //    bool filteringByIngredient = cbFilteringByIngredient.IsChecked == true;

        //    bool showAll = rbShowAll.IsChecked == true;
        //    bool showSales = rbShowSales.IsChecked == true;
        //    bool showPurchases = rbShowPurchases.IsChecked == true;

        //    if(noGrouping || groupingByDate)myDG.Columns.Add(dateColumn);
            
        //    //if(noGrouping) myDG.Columns.Add(typeColumn);

        //    if (!showAll || noGrouping) myDG.Columns.Add(quantityColumn);
        //    //quantityColumn.Visibility = ConvertToVisibility();
        //    //always visible
        //    myDG.Columns.Add(productColumn);
        //    //productColumn.Visibility                        
        //    if (!productosDesglosados && !filteringByIngredient &&
        //        (!showAll || noGrouping)) myDG.Columns.Add(priceColumn);
        //    //priceColumn.Visibility = ConvertToVisibility(!showingIngredients);

        //    if (showAll && !noGrouping) myDG.Columns.Add(quantitySoldColumn);
        //    if (showAll && !noGrouping && !productosDesglosados && !filteringByIngredient)
        //        myDG.Columns.Add(salePriceColumn);
        //    if (showAll && !noGrouping) myDG.Columns.Add(quantityPurchasedColumn);
        //    if (showAll && !noGrouping && !productosDesglosados && !filteringByIngredient)
        //        myDG.Columns.Add(purchasePriceColumn);

        //    if (showSales && filteringByIngredient) myDG.Columns.Add(ingredientQuantityColumn);
        //    //ingredientQuantityColumn.Visibility = ConvertToVisibility(filteringByIngredient);
        //    if (showSales && filteringByIngredient) myDG.Columns.Add(ingredientColumn);
        //    //ingredientColumn.Visibility = ConvertToVisibility(filteringByIngredient);
            
        //    if (showSales && (noGrouping || groupingByEmployee)) myDG.Columns.Add(waiterColumn);
        //    //waiterColumn.Visibility = ConvertToVisibility(noGrouping);            
        //    if (showSales &&  noGrouping) myDG.Columns.Add(areaColumn);
        //    //areaColumn.Visibility = ConvertToVisibility(noGrouping);            

        //    if (showPurchases && noGrouping) myDG.Columns.Add(purchaseTypeColumn);
        //    if (showPurchases && noGrouping) myDG.Columns.Add(responsibleColumn);

        //    if (noGrouping) myDG.Columns.Add(orderColumn);
        //    //orderColumn.Visibility = ConvertToVisibility(noGrouping);
        //}

        //private void ResetFootersVisibility()
        //{
        //    bool showAll = rbShowAll.IsChecked == true;
        //    bool showSales = rbShowSales.IsChecked == true;
        //    bool showPurchases = rbShowPurchases.IsChecked == true;

        //    tbQuantity.Visibility = ConvertToVisibility(showSales || showPurchases);
        //    tbTotal.Visibility = ConvertToVisibility(showSales || showPurchases);

        //    tbSalesQuantity.Visibility = ConvertToVisibility(showAll);
        //    tbSalesTotal.Visibility = ConvertToVisibility(showAll);

        //    tbPurchasesQuantity.Visibility = ConvertToVisibility(showAll);
        //    tbPurchasesTotal.Visibility = ConvertToVisibility(showAll);
        //}

        //Visibility ConvertToVisibility(bool value) 
        //{
        //    if (value) return Visibility.Visible;
        //    return Visibility.Collapsed;
        //}

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ResetColumnsVisibility();
        //}

        //private void filterByIngredient_Checked(object sender, RoutedEventArgs e)
        //{
        //    ResetColumnsVisibility();
        //}

        //private void filterByIngredient_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    ResetColumnsVisibility();
        //}

        //private void convertToIngredients_Click(object sender, RoutedEventArgs e)
        //{
        //    productosDesglosados = true;

        //    ResetColumnsVisibility();
        //}

        //private void findButton_Click(object sender, RoutedEventArgs e)
        //{
        //    productosDesglosados = false;

        //    ResetColumnsVisibility();
        //}

        //private void radiobutton_Checked(object sender, RoutedEventArgs e)
        //{
        //    //ResetColumnsVisibility();

        //    ResetFootersVisibility();
        //}        

        //public static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    myDG grid = d as myDG;
        //    if (grid != null)
        //    {
        //        foreach (myDGColumn col in grid.Columns)
        //        {
        //            col.SetValue(FrameworkElement.DataContextProperty, e.NewValue);
        //        }
        //    }
        //}

    }
}
