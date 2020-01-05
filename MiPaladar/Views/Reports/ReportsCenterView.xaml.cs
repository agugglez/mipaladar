using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    public partial class ReportsCenterView : UserControl
    {
        public ReportsCenterView()
        {
            InitializeComponent();
        }

        //private void Hyperlink_Click(object sender, RoutedEventArgs e)
        //{
        //    ReportsCenterViewModel rCenter = (ReportsCenterViewModel)DataContext;
        //    ReportsWindowViewModel vm = new ReportsWindowViewModel(rCenter.AppVM, "Prueba", ReportType.Product);
        //    Form1 form1 = new Form1(vm);

        //    form1.Show();
        //}
    }
}
