using System;
using System.Windows;
using System.Windows.Controls;

//using MiPaladar.PrintControls;
using MiPaladar.ViewModels;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;

namespace MiPaladar.Services
{
    public class Printer
    {
        //public static bool PrintVale(SaleViewModel vale)
        //{
        //    PrintDialog pDialog = new PrintDialog();
        //    //pDialog.PageRangeSelection = PageRangeSelection.AllPages;
        //    pDialog.UserPageRangeEnabled = false;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        ValeToPrint control = new ValeToPrint();
        //        control.DataContext = vale;
                
        //        Size sz = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);

        //        // arrange
        //        control.Measure(sz);
        //        control.Arrange(new Rect(sz));
        //        control.UpdateLayout();

        //        Size finalSize = new Size(control.DesiredSize.Width, control.DesiredSize.Height);

        //        control.Measure(finalSize);
        //        control.Arrange(new Rect(finalSize));
        //        control.UpdateLayout();

        //        pDialog.PrintVisual(control, "Imprimiendo vale " + vale.Number);

        //        return true;
        //    }

        //    return false;

        //    //XpsDocument xpsd = new XpsDocument("vale.xps", FileAccess.Write);
        //    //XpsDocumentWriter xpsdw = XpsDocument.CreateXpsDocumentWriter(xpsd);

        //    //xpsdw.Write(control);
        //    //xpsd.Close(); 
        //}

        //public static bool PrintVale(OfflineSaleViewModel vale)
        //{
        //    PrintDialog pDialog = new PrintDialog();
        //    pDialog.UserPageRangeEnabled = false;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        ValeToPrint control = new ValeToPrint();
        //        control.DataContext = vale;

        //        Size sz = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);

        //        // arrange
        //        control.Measure(sz);
        //        control.Arrange(new Rect(sz));
        //        control.UpdateLayout();

        //        Size finalSize = new Size(control.DesiredSize.Width, control.DesiredSize.Height);

        //        control.Measure(finalSize);
        //        control.Arrange(new Rect(finalSize));
        //        control.UpdateLayout();

        //        pDialog.PrintVisual(control, "Imprimiendo vale");

        //        return true;
        //    }

        //    return false;
        //}

        //public static bool PrintProductionVale(object vale, int? number)
        //{
        //    PrintDialog pDialog = new PrintDialog();
        //    //pDialog.PageRangeSelection = PageRangeSelection.AllPages;
        //    pDialog.UserPageRangeEnabled = false;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        ValeDeCocinaToPrint control = new ValeDeCocinaToPrint();
        //        control.DataContext = vale;

        //        Size sz = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);

        //        // arrange
        //        control.Measure(sz);
        //        control.Arrange(new Rect(sz));
        //        control.UpdateLayout();

        //        Size finalSize = new Size(control.DesiredSize.Width, control.DesiredSize.Height);

        //        control.Measure(finalSize);
        //        control.Arrange(new Rect(finalSize));
        //        control.UpdateLayout();

        //        pDialog.PrintVisual(control, "Imprimiendo vale de cocina" + number);

        //        return true;
        //    }

        //    return false;

        //    //XpsDocument xpsd = new XpsDocument("vale.xps", FileAccess.Write);
        //    //XpsDocumentWriter xpsdw = XpsDocument.CreateXpsDocumentWriter(xpsd);

        //    //xpsdw.Write(control);
        //    //xpsd.Close(); 
        //}

        //public static bool PrintAlmacen(object totalsAlmacen)
        //{
        //    PrintDialog pDialog = new PrintDialog();
        //    //pDialog.PageRangeSelection = PageRangeSelection.AllPages;
        //    pDialog.UserPageRangeEnabled = false;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        AlmacenReport control = new AlmacenReport();
        //        control.DataContext = totalsAlmacen;

        //        Size sz = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);

        //        // arrange
        //        control.Measure(sz);
        //        control.Arrange(new Rect(sz));
        //        control.UpdateLayout();

        //        Size finalSize = new Size(control.DesiredSize.Width, control.DesiredSize.Height);

        //        control.Measure(finalSize);
        //        control.Arrange(new Rect(finalSize));
        //        control.UpdateLayout();

        //        pDialog.PrintVisual(control, "Imprimiendo IPV ");

        //        return true;
        //    }

        //    return false;
        //}

        //public static bool PrintDayReport(DayReportViewModel drvm)
        //{
        //    PrintDialog pDialog = new PrintDialog();
        //    //pDialog.PageRangeSelection = PageRangeSelection.AllPages;
        //    pDialog.UserPageRangeEnabled = false;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        DaySalesReport control = new DaySalesReport();
        //        control.DataContext = drvm;

        //        Size sz = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);

        //        // arrange
        //        control.Measure(sz);
        //        control.Arrange(new Rect(sz));
        //        control.UpdateLayout();

        //        Size finalSize = new Size(control.DesiredSize.Width, control.DesiredSize.Height);

        //        control.Measure(finalSize);
        //        control.Arrange(new Rect(finalSize));
        //        control.UpdateLayout();

        //        pDialog.PrintVisual(control, "Imprimiendo resumen del día ");

        //        return true;
        //    }

        //    return false;
        //}

        //public static bool PrintValeDocument(SaleViewModel vale)
        //{
        //    PrintDialog pDialog = new PrintDialog();
        //    //pDialog.PageRangeSelection = PageRangeSelection.AllPages;
        //    pDialog.UserPageRangeEnabled = false;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        FlowDocument flowDoc = CreateFlowDocument(vale);

        //        pDialog.PrintDocument(((IDocumentPaginatorSource)flowDoc).DocumentPaginator, "Imprimiendo vale " + vale.Number);

        //        return true;
        //    }

        //    return false;
        //}

        //public static bool PrintValeDocument(OfflineSaleViewModel vale)
        //{
        //    PrintDialog pDialog = new PrintDialog();
        //    //pDialog.PageRangeSelection = PageRangeSelection.AllPages;
        //    pDialog.UserPageRangeEnabled = false;

        //    // Display the dialog. This returns true if the user presses the Print button.
        //    Nullable<Boolean> print = pDialog.ShowDialog();
        //    if (print == true)
        //    {
        //        FlowDocument flowDoc = CreateFlowDocument(vale);

        //        pDialog.PrintDocument(((IDocumentPaginatorSource)flowDoc).DocumentPaginator, "Imprimiendo vale");

        //        return true;
        //    }

        //    return false;
        //}


        //static FlowDocument CreateFlowDocument(SaleViewModel svm) 
        //{
        //    // Create the parent FlowDocument...

        //    FlowDocument flowDoc = new FlowDocument();

        //    // Create the Table...

        //    Table table1 = new Table();
        //    // ...and add it to the FlowDocument Blocks collection.

        //    flowDoc.Blocks.Add(table1);

        //    // Set some global formatting properties for the table.

        //    //table1.CellSpacing = 10;
        //    //table1.Background = Brushes.White;

        //    // Create 6 columns and add them to the table's Columns collection.

        //    int numberOfColumns = 3;
        //    for (int x = 0; x < numberOfColumns; x++)
        //    {
        //        table1.Columns.Add(new TableColumn());

        //        // Set alternating background colors for the middle colums.

        //        //if (x % 2 == 0)
        //        //    table1.Columns[x].Background = Brushes.Beige;
        //        //else
        //        //    table1.Columns[x].Background = Brushes.LightSteelBlue;
        //    }

        //    foreach (LineItemViewModel item in svm.OrderItems)
        //    {
        //        TableRowGroup rowGroup = new TableRowGroup();
        //        table1.RowGroups.Add(rowGroup);
        //        TableRow tableRow = new TableRow();
        //        rowGroup.Rows.Add(tableRow);

        //        TableCell qttyCell = new TableCell(new Paragraph(new Run(item.Quantity.ToString())));
        //        qttyCell.Background = Brushes.LightGreen;
        //        tableRow.Cells.Add(qttyCell);
        //        TableCell prodCell = new TableCell(new Paragraph(new Run(item.Product.Name)));
        //        prodCell.Background = Brushes.LightBlue;
        //        tableRow.Cells.Add(prodCell);
        //        TableCell priceCell = new TableCell(new Paragraph(new Run(item.Price.ToString("c"))));
        //        priceCell.Background = Brushes.Red;
        //        tableRow.Cells.Add(priceCell);
        //    }

        //    return flowDoc;
        //}

        //static FlowDocument CreateFlowDocument(OfflineSaleViewModel svm)
        //{
        //    // Create the parent FlowDocument...

        //    FlowDocument flowDoc = new FlowDocument();

        //    // Create the Table...

        //    Table table1 = new Table();
        //    // ...and add it to the FlowDocument Blocks collection.

        //    flowDoc.Blocks.Add(table1);

        //    // Set some global formatting properties for the table.

        //    //table1.CellSpacing = 10;
        //    //table1.Background = Brushes.White;

        //    // Create 6 columns and add them to the table's Columns collection.

        //    int numberOfColumns = 3;
        //    for (int x = 0; x < numberOfColumns; x++)
        //    {
        //        table1.Columns.Add(new TableColumn());

        //        // Set alternating background colors for the middle colums.

        //        //if (x % 2 == 0)
        //        //    table1.Columns[x].Background = Brushes.Beige;
        //        //else
        //        //    table1.Columns[x].Background = Brushes.LightSteelBlue;
        //    }

        //    foreach (OfflineLineItemViewModel item in svm.OrderItems)
        //    {
        //        TableRowGroup rowGroup = new TableRowGroup();
        //        table1.RowGroups.Add(rowGroup);
        //        TableRow tableRow = new TableRow();
        //        rowGroup.Rows.Add(tableRow);

        //        TableCell qttyCell = new TableCell(new Paragraph(new Run(item.Quantity.ToString())));
        //        qttyCell.Background = Brushes.LightGreen;
        //        tableRow.Cells.Add(qttyCell);
        //        TableCell prodCell = new TableCell(new Paragraph(new Run(item.Product.Name)));
        //        prodCell.Background = Brushes.LightBlue;
        //        tableRow.Cells.Add(prodCell);
        //        TableCell priceCell = new TableCell(new Paragraph(new Run(item.Price.ToString("c"))));
        //        priceCell.Background = Brushes.Red;
        //        tableRow.Cells.Add(priceCell);
        //    }

        //    return flowDoc;
        //}
    }
}
