using System;
using System.Windows;
using System.Windows.Controls;

using MiPaladar.ViewModels;
using MiPaladar.Services;

using System.ComponentModel;

namespace MiPaladar
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window//, INotifyPropertyChanged
    {
        //MainWindowViewModel appvm;

        public LoadingWindow()
        {
            //this.appvm = new MainWindowViewModel(); 

            InitializeComponent();

            //this.ContentRendered += new EventHandler(Window_ContentRendered);
        }

        //private void Window_ContentRendered(object sender, EventArgs e)
        //{
        //    DoWork();           
        //}

        //void DoWork()
        //{
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        //    //worker.WorkerReportsProgress = true;
        //    //worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        //    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

        //    worker.RunWorkerAsync();
        //}

        //void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    appvm.Load();
        //}

        //void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    var windowManager = ServiceContainer.GetService<IWindowManager>();

        //    Hide();

        //    LoginViewModel lvm = new LoginViewModel(appvm);

        //    appvm.ShowAppWideDialog(lvm);

        //    windowManager.Show(appvm);

        //    Close();           
        //}

    }
}
