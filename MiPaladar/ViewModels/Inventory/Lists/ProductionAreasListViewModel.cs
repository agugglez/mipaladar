using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Windows.Input;
using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class ProductionAreasListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public ProductionAreasListViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        protected override void OnDispose()
        {
            if (firstTime) return;

            foreach (var item in appvm.ProductionAreasOC)
            {
                item.PropertyChanged -= productionArea_PropertyChanged;
            }
        }

        public override string DisplayName
        {
            get { return "AREAS DE PRODUCCION"; }
        }

        //string newProductionAreaName;
        //public string NewProductionAreaName 
        //{
        //    get { return newProductionAreaName; }
        //    set
        //    {
        //        newProductionAreaName = value;
        //        OnPropertyChanged("NewProductionAreaName");
        //    }
        //}

        #region New Production Area Command

        RelayCommand newProductionAreaCommand;
        public ICommand NewProductionAreaCommand
        {
            get 
            {
                if (newProductionAreaCommand == null)
                    newProductionAreaCommand = new RelayCommand(x => NewProductionArea());
                return newProductionAreaCommand; 
            }
        }

        void NewProductionArea()
        {
            ProductionArea pa = new ProductionArea();
            pa.Name = "Nueva área de producción";

            appvm.Context.ProductionAreas.AddObject(pa);
            appvm.ProductionAreasOC.Add(pa);

            appvm.SaveChanges();

            pa.PropertyChanged += productionArea_PropertyChanged;

            //var windowManager = base.GetService<IWindowManager>();

            //ProductionAreaViewModel cvm = new ProductionAreaViewModel(appvm);

            //windowManager.Show(cvm);
        }

        #endregion

        public ProductionArea SelectedProductionArea { get; set; }

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.Remove(SelectedProductionArea), x => this.CanRemove);
                return removeCommand;
            }
        }

        bool CanRemove { get { return SelectedProductionArea != null; } }

        void Remove(ProductionArea paToRemove)
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = string.Format("¿Está seguro que desea eliminar el área de producción '{0}'?", paToRemove.Name);

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                appvm.ProductionAreasOC.Remove(paToRemove);
                appvm.Context.ProductionAreas.DeleteObject(paToRemove);

                appvm.SaveChanges();

                paToRemove.PropertyChanged -= productionArea_PropertyChanged;

                ////close this window
                //var windowManager = base.GetService<IWindowManager>();

                //windowManager.Close(this);
            }
        }

        #endregion

        //#region Expand Item Command

        //RelayCommand expandItemCommand;
        //public ICommand ExpandItemCommand
        //{
        //    get
        //    {
        //        if (expandItemCommand == null)
        //            expandItemCommand = new RelayCommand(x => this.ExpandItem());
        //        return expandItemCommand;
        //    }
        //}

        //bool CanExpand { get { return SelectedProductionArea != null; } }

        //void ExpandItem()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
        //    {
        //        if (!(wsvm is ProductionAreaViewModel)) return false;

        //        ProductionAreaViewModel svm = (ProductionAreaViewModel)wsvm;

        //        return svm.WrappedProductionArea == SelectedProductionArea;
        //    };

        //    if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
        //    else
        //    {
        //        ProductionAreaViewModel avm = new ProductionAreaViewModel(appvm, SelectedProductionArea);

        //        windowManager.ShowChildWindow(avm, this);
        //    }
        //}

        //#endregion

        bool firstTime = true;
        public ObservableCollection<ProductionArea> ProductionAreas 
        {
            get 
            {
                if (firstTime)
                {
                    firstTime = false;

                    foreach (var item in appvm.ProductionAreasOC)
                    {
                        item.PropertyChanged += productionArea_PropertyChanged;
                    }
                }
                return appvm.ProductionAreasOC; 
            }
        }

        void productionArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                appvm.SaveChanges();
            }
        }
    }
}
