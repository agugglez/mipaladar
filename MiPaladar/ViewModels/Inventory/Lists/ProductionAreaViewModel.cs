//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Services;
//using MiPaladar.Entities;

//using System.Windows.Input;

//namespace MiPaladar.ViewModels
//{
//    public class ProductionAreaViewModel : ViewModelBase
//    {
//        ProductionArea productionArea;
//        IConfirmator confirmator;

//        MainWindowViewModel appvm;

//        bool creating;

//        //creating a new one
//        public ProductionAreaViewModel(MainWindowViewModel appvm)
//        {
//            this.appvm = appvm;
//            this.confirmator = appvm.Confirmator;

//            creating = true;

//            HasPendingChanges = true;
//        }

//        public ProductionAreaViewModel(MainWindowViewModel appvm, ProductionArea c)
//        {
//            productionArea = c;

//            this.appvm = appvm;
//            this.confirmator = appvm.Confirmator;

//            name = c.Name;
//        }

//        public override string DisplayName
//        {
//            get { return creating ? "Nueva Area" : "Areas de Producción: " + name; }
//        }

//        public ProductionArea WrappedProductionArea
//        {
//            get { return productionArea; }
//        }

//        string name;
//        public string Name
//        {
//            get { return name; }
//            set
//            {
//                name = value;
//                OnPropertyChanged("Name");
//                HasPendingChanges = true;
//            }
//        }

//        bool hasPendingChanges;
//        public bool HasPendingChanges
//        {
//            get { return hasPendingChanges; }
//            set
//            {
//                hasPendingChanges = value;
//                OnPropertyChanged("HasPendingChanges");
//            }
//        }

//        #region Remove Command

//        RelayCommand removeCommand;
//        public ICommand RemoveCommand
//        {
//            get
//            {
//                if (removeCommand == null)
//                    removeCommand = new RelayCommand(x => this.Remove(), x => this.CanRemove);
//                return removeCommand;
//            }
//        }

//        bool CanRemove { get { return !creating; } }

//        void Remove()
//        {
//            var msgBox = base.GetService<IMessageBoxService>();

//            string message = "¿Está seguro que desea eliminar esta área?";

//            if (msgBox.ShowYesNoDialog(message) == true)
//            {
//                appvm.ProductionAreasOC.Remove(productionArea);
//                appvm.Context.ProductionAreas.DeleteObject(productionArea);

//                appvm.SaveChanges();

//                //close this window
//                var windowManager = base.GetService<IWindowManager>();

//                windowManager.Close(this);
//            }
//        }

//        #endregion

//        #region Cancel Command

//        RelayCommand cancelCommand;
//        public ICommand CancelCommand
//        {
//            get
//            {
//                if (cancelCommand == null)
//                    cancelCommand = new RelayCommand(x => this.Cancel());
//                return cancelCommand;
//            }
//        }

//        void Cancel()
//        {
//            if (creating)
//            {
//                //close this window
//                var windowManager = base.GetService<IWindowManager>();

//                windowManager.Close(this);
//            }
//            //saving changes
//            else
//            {
//                Name = productionArea.Name;

//                HasPendingChanges = false;
//            }
//        }

//        #endregion

//        #region Save Command

//        RelayCommand saveCommand;
//        public ICommand SaveCommand
//        {
//            get
//            {
//                if (saveCommand == null)
//                    saveCommand = new RelayCommand(x => this.Save(), x => this.CanSave);
//                return saveCommand;
//            }
//        }

//        bool CanSave
//        {
//            get { return !string.IsNullOrWhiteSpace(name); }
//        }

//        void Save()
//        {
//            if (ThereIsAnAreaWithTheSameName())
//            {
//                var msgBox = base.GetService<IMessageBoxService>();
//                string message = "Ya existe un área con ese nombre, elija otro.";

//                msgBox.Show(message, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//                return;
//            }

//            if (creating)
//            {
//                productionArea = new ProductionArea();
//                productionArea.Name = name;

//                appvm.Context.ProductionAreas.AddObject(productionArea);
//                appvm.ProductionAreasOC.Add(productionArea);

//                creating = false;
//            }
//            else
//            {
//                if (productionArea.Name != name) productionArea.Name = name;
//            }

//            appvm.SaveChanges();

//            HasPendingChanges = false;
//        }

//        private bool ThereIsAnAreaWithTheSameName()
//        {
//            //look for another category with the same name
//            foreach (var pa in appvm.ProductionAreasOC)
//            {
//                if (pa.Name == name && pa != productionArea) return true;
//            }

//            return false;
//        }

//        #endregion       
//    }
//}
