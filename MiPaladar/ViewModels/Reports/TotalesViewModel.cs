using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Services;
using MiPaladar.Classes;

using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MiPaladar.Classes 
{
    public enum Dimensions { Producto, Dependiente, Mesa, Centro}
}

namespace MiPaladar.ViewModels
{
    public class TotalesViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        TotalesPorProductoViewModel totalesProductoVM;
        TotalesPorDependienteViewModel totalesDependienteVM;
        TotalesPorMesaViewModel totalesMesaVM;
        TotalesPorCentroViewModel totalesCentroVM;
        TotalesCompraViewModel totalesCompraVM;
        TotalesProductosPorDependienteViewModel totalesProductosDependientesVM;

        public TotalesViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            fromDate = DateTime.Today;
            toDate = DateTime.Today;

            CreateTotals();

            ShowTotalsByProduct();
        }        

        public override string DisplayName
        {
            get
            {
                return "Totales";
            }
        }

        private void CreateTotals()
        {
            totalesProductoVM = new TotalesPorProductoViewModel(this, appvm);

            totalesDependienteVM = new TotalesPorDependienteViewModel(this, appvm);

            totalesMesaVM = new TotalesPorMesaViewModel(this, appvm);

            totalesCentroVM = new TotalesPorCentroViewModel(this, appvm);

            totalesCompraVM = new TotalesCompraViewModel(this, appvm);

            totalesProductosDependientesVM = new TotalesProductosPorDependienteViewModel(this, appvm);
        }

        DateTime fromDate;
        public DateTime FromDate
        {
            get
            {
                if (fromDate == null)
                    fromDate = DateTime.Today;
                return fromDate;
            }
            set { fromDate = value; }
        }

        DateTime toDate;
        public DateTime ToDate
        {
            get
            {
                if (toDate == null)
                    toDate = DateTime.Today;
                return toDate;
            }
            set { toDate = value; }
        }        

        #region Commands

        RelayCommand updateTotalsCommand;
        public ICommand UpdateTotalsCommand 
        {
            get 
            {
                if (updateTotalsCommand == null)
                    updateTotalsCommand = new RelayCommand(x => this.UpdateTotals());
                return updateTotalsCommand;
            }
        }

        private void UpdateTotals()
        {
            totalesProductoVM.UpdateTotals();
            totalesDependienteVM.UpdateTotals();
            totalesMesaVM.UpdateTotals();
            totalesCentroVM.UpdateTotals();
            totalesCompraVM.UpdateTotals();
            totalesProductosDependientesVM.UpdateTotals();
        }

        ObservableCollection<CommandViewModel> commands;

        public ObservableCollection<CommandViewModel> Commands
        {
            get
            {
                if (commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    commands = new ObservableCollection<CommandViewModel>(cmds);
                }
                return commands;
            }
        }

        List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Productos",
                    new RelayCommand(param => this.ShowTotalsByProduct())),                

                new CommandViewModel(
                    "Dependientes",
                    new RelayCommand(param => this.ShowTotalsByWaiter())),

                new CommandViewModel(
                    "Productos Por Dependiente",
                    new RelayCommand(param => this.ShowTotalsProductosDependiente())),

                new CommandViewModel(
                    "Mesas",
                    new RelayCommand(param => this.ShowTotalsByTable())),

                new CommandViewModel(
                    "Centro",
                    new RelayCommand(param => this.ShowTotalsByCentro())),

                new CommandViewModel(
                    "Compras",
                    new RelayCommand(param => this.ShowTotalsCompra()))
            };
        }

        ViewModelBase selectedWorkspace;
        public ViewModelBase SelectedWorkspace
        {
            get { return selectedWorkspace; }
            set 
            {
                selectedWorkspace = value;
                OnPropertyChanged("SelectedWorkspace");
            }
        }

        void ShowTotalsByProduct()
        {
            //if (totalesProductoVM == null) totalesProductoVM = new TotalesPorProductoViewModel(this, appvm);
            if (SelectedWorkspace != totalesProductoVM) SelectedWorkspace = totalesProductoVM;
        }

        void ShowTotalsByWaiter()
        {
            //if (totalesDependienteVM == null) totalesDependienteVM = new TotalesPorDependienteViewModel(this, appvm);
            if (SelectedWorkspace != totalesDependienteVM) SelectedWorkspace = totalesDependienteVM;
        }

        void ShowTotalsByTable()
        {
            //if (totalesMesaVM == null) totalesMesaVM = new TotalesPorMesaViewModel(this, appvm);
            if (SelectedWorkspace != totalesMesaVM) SelectedWorkspace = totalesMesaVM;
        }
        void ShowTotalsByCentro()
        {
            //if (totalesCentroVM == null) totalesCentroVM = new TotalesPorCentroViewModel(this, appvm);
            if (SelectedWorkspace != totalesCentroVM) SelectedWorkspace = totalesCentroVM;
        }

        void ShowTotalsCompra()
        {
            //if (totalesCompraVM == null) totalesCompraVM = new TotalesCompraViewModel(this, appvm);
            if (SelectedWorkspace != totalesCompraVM) SelectedWorkspace = totalesCompraVM;
        }

        void ShowTotalsProductosDependiente()
        {
            //if (totalesCompraVM == null) totalesCompraVM = new TotalesCompraViewModel(this, appvm);
            if (SelectedWorkspace != totalesCompraVM) SelectedWorkspace = totalesProductosDependientesVM;
        }

        #endregion
        
    }
}