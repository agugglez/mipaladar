using System;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.Stuff;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ProductViewQuick.xaml
    /// </summary>
    public partial class ProductView : WindowWithClose
    {
        public ProductView()
            : base()
        {
            InitializeComponent();

            tbxName.Focus();
            //this.Closing += this.HandleClosing;
        }

        //#region Closing

        //void HandleClosing(object sender, CancelEventArgs e)
        //{
        //    var screen = base.DataContext as IScreen;
        //    if (screen != null && !screen.IsSelfClosing()) e.Cancel = !screen.TryToClose();
        //}

        //#endregion

        //bool firstTime = true;
        //private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (firstTime)
        //    {
        //        ViewModelWithClose vm = (ViewModelWithClose)e.NewValue;

        //        this.Closing += vm.HandleClosing;

        //        firstTime = false;
        //    }
            
        //}

    }
}
