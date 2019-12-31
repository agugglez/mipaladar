using System;
using System.Windows;
using System.Windows.Controls;

namespace MiPaladar.Views
{
    /// <summary>
    /// Interaction logic for MyCompanyView.xaml
    /// </summary>
    public partial class MyCompanyView : UserControl
    {
        public MyCompanyView()
        {
            InitializeComponent();

            //this.DataContextChanged += new DependencyPropertyChangedEventHandler(MyCompanyView_DataContextChanged);
        }

        //void MyCompanyView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue is MainWindowViewModel)
        //    {
        //        MyCompanyViewModel viewmodel = (MyCompanyViewModel)e.NewValue;
        //        viewmodel.ImportFinished += new EventHandler(viewmodel_ImportFinished);
        //    }
        //    else if (e.OldValue is MainWindowViewModel)
        //    {
        //        MyCompanyViewModel viewmodel = (MyCompanyViewModel)e.OldValue;
        //        viewmodel.ImportFinished -= viewmodel_ImportFinished;
        //    }
        //}

        //void viewmodel_ImportFinished(object sender, EventArgs e)
        //{
        //    GoToLoginWindow();
        //}

        //void GoToLoginWindow()
        //{
        //    MainWindow mw = (MainWindow)Application.Current.MainWindow;
        //    //MyCompanyViewModel mcvm = (MyCompanyViewModel)this.DataContext;
        //    MainWindowViewModel appvm = (MainWindowViewModel)mw.DataContext;

        //    Login login = new Login(appvm);

        //    mw.Close();

        //    login.Show();
        //} 
    }    
}
