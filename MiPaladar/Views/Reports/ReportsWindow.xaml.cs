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
    public partial class ReportsWindow : Window
    {

        public ReportsWindow()
        {
            InitializeComponent();
        }

        //Grid chartAndTotalsGrid;

        //bool firstime = true;
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //if (firstime)
        //    //{
        //    //    AddAux();
        //    //    AddGraph();
        //    //    AddTotalItems();
        //    //    AddColumns();

        //    //    firstime = false;
        //    //}            
        //}

        //private void AddAux()
        //{
        //    ReportsWindowViewModel vm = (ReportsWindowViewModel)DataContext;

        //    //projections
        //    if (vm.ReportType == ReportType.SalesProjectionsByItem ||
        //        vm.ReportType == ReportType.WIPProjectionsByItem ||
        //        vm.ReportType == ReportType.CostProjectionsByItem)
        //    {
        //        TextBlock tb = new TextBlock();
        //        tb.Text = "% Proyección";

        //        auxBarItems.Children.Add(tb);

        //        TextBox tBox = new TextBox();
        //        Binding bind = new Binding("ProjectionPercent");
        //        bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        //        tBox.SetBinding(TextBox.TextProperty, bind);

        //        auxBarItems.Children.Add(tBox);
        //    }
        //    else
        //    {
        //        TextBlock tb = new TextBlock();
        //        tb.Text = "Fecha";

        //        auxBarItems.Children.Add(tb);

        //        //dates combobox
        //        ComboBox cbx = new ComboBox();
        //        cbx.MinWidth = 100;
        //        cbx.ItemsSource = vm.DateOptions;
        //        cbx.SetBinding(ComboBox.SelectedItemProperty, new Binding("SelectedDateOption"));

        //        auxBarItems.Children.Add(cbx);

        //        //selected date tb
        //        tb = new TextBlock();
        //        MultiBinding mb = new MultiBinding();
        //        mb.Bindings.Add(new Binding("FromDate"));
        //        mb.Bindings.Add(new Binding("ToDate"));
        //        mb.StringFormat = "{0:d} -> {1:d}";
        //        tb.SetBinding(TextBlock.TextProperty, mb);

        //        auxBarItems.Children.Add(tb);
        //    }
        //}

        //private void AddGraph()
        //{
        //    ReportsWindowViewModel vm = (ReportsWindowViewModel)DataContext;

        //    if (vm.ChartDef.ChartSeries.Count == 0) return;

        //    //add chart grid
        //    AddChartAndTotalsGrid();

        //    Chart chart = new Chart();
        //    chart.BorderBrush = Brushes.Transparent;
        //    chart.BorderThickness = new Thickness(0);
        //    chart.FontSize = 14;

        //    if (vm.ChartDef.ChartSeries[0].ChartType == ChartSeriesType.Column)
        //    {
        //        chart.Axes.Add(new LinearAxis() { Orientation = AxisOrientation.Y, Minimum = 0 });
        //    }

        //    ////title style
        //    //Style titleStyle = new System.Windows.Style();
        //    //Setter heightSetter = new Setter(FrameworkElement.ActualHeightProperty, 1);
        //    //titleStyle.Setters.Add(heightSetter);
        //    //chart.TitleStyle = titleStyle;

        //    ////legend style
        //    //Style legendStyle = new System.Windows.Style();
        //    //Setter widthSetter = new Setter(FrameworkElement.ActualWidthProperty, 1);
        //    //legendStyle.Setters.Add(widthSetter);
        //    //chart.LegendStyle = legendStyle;

        //    foreach (var serie in vm.ChartDef.ChartSeries)
        //    {
        //        if (serie.ChartType == ChartSeriesType.Column)
        //        {
        //            ColumnSeries cs = new ColumnSeries();
        //            cs.Title = serie.Title;
        //            cs.ItemsSource = vm.GraphData;
        //            cs.IndependentValuePath = serie.XBinding;
        //            cs.DependentValuePath = serie.YBinding;

        //            chart.Series.Add(cs);
        //        }
        //        else if (serie.ChartType == ChartSeriesType.Pie)
        //        {
        //            PieSeries ps = new PieSeries();
        //            ps.Title = serie.Title;
        //            ps.ItemsSource = vm.GraphData;
        //            ps.IndependentValuePath = serie.XBinding;
        //            ps.DependentValuePath = serie.YBinding;

        //            chart.Series.Add(ps);
        //        }
        //    }

        //    chartAndTotalsGrid.Children.Add(chart);
        //}

        //private void AddChartAndTotalsGrid()
        //{
        //    chartAndTotalsGrid = new Grid();
        //    mainGrid.RowDefinitions.Add(new RowDefinition());
        //    mainGrid.Children.Add(chartAndTotalsGrid);
        //    Grid.SetRow(chartAndTotalsGrid, 2);
        //}

        //private void AddTotalItems()
        //{
        //    ReportsWindowViewModel vm = (ReportsWindowViewModel)DataContext;

        //    if (vm.TotalItems.Count == 0) return;

        //    if (chartAndTotalsGrid == null) AddChartAndTotalsGrid();

        //    //add column definitions
        //    chartAndTotalsGrid.ColumnDefinitions.Add(new ColumnDefinition());
        //    chartAndTotalsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

        //    //stackpanel
        //    StackPanel sp = new StackPanel();
        //    sp.MinWidth = 200;
        //    sp.Margin = new Thickness(0, 25, 10, 25);

        //    //border
        //    Border brd = new Border();
        //    brd.BorderThickness = new Thickness(2);
        //    brd.BorderBrush = Brushes.Gray;
        //    brd.CornerRadius = new CornerRadius(3);

        //    //add border
        //    sp.Children.Add(brd);

        //    //add stackpanel
        //    chartAndTotalsGrid.Children.Add(sp);
        //    Grid.SetColumn(sp, 1);

        //    //totals grid
        //    Grid totalsGrid = new Grid();
        //    brd.Child = totalsGrid;

        //    //totals grid columns
        //    ColumnDefinition cd = new ColumnDefinition();
        //    cd.Width = new GridLength(1, GridUnitType.Auto);
        //    totalsGrid.ColumnDefinitions.Add(cd);

        //    cd = new ColumnDefinition();
        //    totalsGrid.ColumnDefinitions.Add(cd);

        //    int rowCount = 0;
        //    foreach (var item in vm.TotalItems)
        //    {
        //        //add row
        //        RowDefinition rd = new RowDefinition();
        //        rd.Height = new GridLength(1, GridUnitType.Auto);

        //        totalsGrid.RowDefinitions.Add(rd);

        //        //header
        //        TextBlock headerTB = new TextBlock();
        //        headerTB.Text = item.Header;

        //        totalsGrid.Children.Add(headerTB);
        //        Grid.SetRow(headerTB, rowCount);

        //        //value
        //        TextBlock valueTB = new TextBlock();
        //        valueTB.FontSize = 16;
        //        valueTB.FontWeight = FontWeights.Bold;
        //        Binding b = new Binding(item.BindingString);
        //        if (!string.IsNullOrWhiteSpace(item.FormatString)) b.StringFormat = item.FormatString;
        //        valueTB.SetBinding(TextBlock.TextProperty, b);

        //        totalsGrid.Children.Add(valueTB);
        //        Grid.SetRow(valueTB, rowCount++);
        //        Grid.SetColumn(valueTB, 1);
        //    }
        //}

        //private void AddColumns()
        //{
        //    ReportsWindowViewModel rw = (ReportsWindowViewModel)DataContext;

        //    //add columns
        //    foreach (var h in rw.Columns)
        //    {
        //        DataGridTextColumn tc = new DataGridTextColumn();
        //        tc.Header = h.Header;

        //        if (h.MultiBinding != null)
        //        {
        //            MultiBinding mb = new MultiBinding();
        //            foreach (var item in h.MultiBinding)
        //            {
        //                mb.Bindings.Add(new Binding(item));
        //            }
        //            tc.Binding = mb;
        //        }
        //        else
        //        {
        //            tc.Binding = new Binding(h.BindingString);
        //        }

        //        if (!string.IsNullOrWhiteSpace(h.FormatString))
        //        {
        //            if (h.FormatString == "c") tc.MinWidth = 90;//money columns
        //            else if (h.FormatString == "p") tc.MinWidth = 80;//percent columns
        //            tc.Binding.StringFormat = h.FormatString;
        //        }
        //        if (!string.IsNullOrWhiteSpace(h.SortPathString)) tc.SortMemberPath = h.SortPathString;

        //        myDG.Columns.Add(tc);
        //    }
        //}
    }
}
