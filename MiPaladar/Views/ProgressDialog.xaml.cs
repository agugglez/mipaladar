using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.ViewModels;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for ImportExportDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window
    {
        public ProgressDialog()
        {
            InitializeComponent();

            this.Closing += ProgressDialog_Closing;
        }

        void ProgressDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProgressDialogViewModel pdvm = (ProgressDialogViewModel)this.DataContext;

            if (pdvm.IsBusy) { e.Cancel = true; }
        }
    }
}
