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
    public class AdjustmentsListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        public AdjustmentsListViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;

            FindAdjustments();
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        ObservableCollection<Adjustment> adjustmentsOC = new ObservableCollection<Adjustment>();
        public IEnumerable<Adjustment> Adjustments 
        {
            get { return adjustmentsOC; }
        }

        public void FindAdjustments() 
        {
            adjustmentsOC.Clear();

            DateTime toDatePlusOne = ToDate.AddDays(1);

            var query = from adj in appvm.Context.Orders.OfType<Adjustment>()
                        where adj.Date >= FromDate && adj.Date < toDatePlusOne
                        select adj;

            foreach (var item in query)
            {
                adjustmentsOC.Add(item);
            }
        }

        public Adjustment SelectedAdjustment { get; set; }

        #region New Adjustment Command

        RelayCommand newAdjustmentCommand;
        public ICommand NewAdjustmentCommand
        {
            get
            {
                if (newAdjustmentCommand == null)
                    newAdjustmentCommand = new RelayCommand(x => NewAdjustment());
                return newAdjustmentCommand;
            }
        }

        void NewAdjustment()
        {
            var windowManager = base.GetService<IWindowManager>();

            AdjustInventoryViewModel aivm = new AdjustInventoryViewModel(appvm, OnNewAdjustmentCreated);

            windowManager.Show(aivm);
        }

        void OnNewAdjustmentCreated(Adjustment newAdj) 
        {
            adjustmentsOC.Add(newAdj);

            ExpandItem(newAdj);
        }

        #endregion

        #region Expand Item Command

        RelayCommand expandItemCommand;
        public ICommand ExpandItemCommand 
        {
            get
            {
                if (expandItemCommand == null)
                    expandItemCommand = new RelayCommand(x => this.ExpandItem(SelectedAdjustment));
                return expandItemCommand;
            }
        }

        bool CanExpand 
        {
            get { return SelectedAdjustment != null; }
        }

        void ExpandItem(Adjustment toExpand) 
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is AdjustmentViewModel)) return false;

                AdjustmentViewModel svm = (AdjustmentViewModel)wsvm;

                return svm.WrappedAdjustment == toExpand;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                AdjustmentViewModel avm = new AdjustmentViewModel(appvm, toExpand, OnRemoved, OnAssociationChanged);

                windowManager.ShowChildWindow(avm, this);
            } 
        }

        void OnRemoved(Adjustment adj) 
        {
            adjustmentsOC.Remove(adj);
        }

        //some properties don't fire propertychanged events
        void OnAssociationChanged(Adjustment adj)
        {
            int index = adjustmentsOC.IndexOf(adj);

            if (index >= 0)
            {
                adjustmentsOC.RemoveAt(index);
                adjustmentsOC.Insert(index, adj);
            }
        }
        #endregion
    }
}
