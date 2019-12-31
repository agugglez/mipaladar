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
    public class ProductionsListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        public ProductionsListViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;

            FindProductions();
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        ObservableCollection<Production> productionsOC = new ObservableCollection<Production>();
        public ObservableCollection<Production> Productions 
        {
            get { return productionsOC; }
        }

        public void FindProductions()
        {
            productionsOC.Clear();

            DateTime toDatePlusOne = ToDate.AddDays(1);

            var query = from prod in appvm.Context.Orders.OfType<Production>()
                        where prod.Date >= FromDate && prod.Date < toDatePlusOne
                        select prod;

            foreach (var item in query)
            {
                productionsOC.Add(item);
            }
        }

        #region New Production Command

        RelayCommand newProductionCommand;
        public ICommand NewProductionCommand
        {
            get
            {
                if (newProductionCommand == null)
                    newProductionCommand = new RelayCommand(x => DoNewProduction());
                return newProductionCommand;
            }
        }

        void DoNewProduction()
        {
            var windowManager = base.GetService<IWindowManager>();

            ProductionViewModel pvm = new ProductionViewModel(appvm, OnCreated, OnRemoved, OnAssociationChanged);

            windowManager.Show(pvm);
        }

        void OnCreated(Production p)
        {
            productionsOC.Add(p);
        }

        //some properties don't fire propertychanged events
        void OnAssociationChanged(Production tra)
        {
            int index = productionsOC.IndexOf(tra);

            if (index >= 0)
            {
                productionsOC.RemoveAt(index);
                productionsOC.Insert(index, tra);
            }
        }

        #endregion

        public Production SelectedProduction { get; set; }

        RelayCommand expandItemCommand;
        public ICommand ExpandItemCommand
        {
            get
            {
                if (expandItemCommand == null)
                    expandItemCommand = new RelayCommand(x => this.ExpandItem(), x => this.CanExpand);
                return expandItemCommand;
            }
        }

        bool CanExpand { get { return SelectedProduction != null; } }

        void ExpandItem()
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is ProductionViewModel)) return false;

                ProductionViewModel svm = (ProductionViewModel)wsvm;

                return svm.WrappedProduction == SelectedProduction;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                ProductionViewModel avm = new ProductionViewModel(appvm, SelectedProduction, OnRemoved, OnAssociationChanged);

                windowManager.ShowChildWindow(avm, this);
            }
        }

        void OnRemoved(Production p)
        {
            productionsOC.Remove(p);
        }
    }
}
