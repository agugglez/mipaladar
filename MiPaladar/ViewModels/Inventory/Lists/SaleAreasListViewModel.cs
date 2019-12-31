using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Specialized;

namespace MiPaladar.ViewModels
{
    public class SaleAreasListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        //ObservableCollection<AreaViewModel> areasVM;

        public SaleAreasListViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        protected override void OnDispose()
        {
            if (firstTime) return;

            foreach (var item in appvm.PriceListsOC)
            {
                item.PropertyChanged -= saleArea_PropertyChanged;
            }
        }

        public override string DisplayName
        {
            get { return "AREAS DE VENTA"; }
        }

        PriceList selectedArea;
        public PriceList SelectedArea 
        {
            get { return selectedArea; }
            set
            {
                selectedArea = value;
                OnPropertyChanged("SelectedArea");
            }
        }

        bool firstTime = true;
        public ObservableCollection<PriceList> SaleAreas
        {
            get
            {
                if (firstTime)
                {
                    firstTime = false;

                    foreach (var item in appvm.PriceListsOC)
                    {
                        item.PropertyChanged += saleArea_PropertyChanged;
                    }
                }
                return appvm.PriceListsOC;
            }
        }

        void saleArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                appvm.SaveChanges();
            }
        }

        #region New Area Command

        RelayCommand newAreaCommand;
        public ICommand NewAreaCommand
        {
            get
            {
                if (newAreaCommand == null)
                    newAreaCommand = new RelayCommand(x => this.Add());
                return newAreaCommand;
            }
        }

        void Add()
        {
            PriceList lp = new PriceList();
            lp.Name = "Nueva Area de Venta";

            appvm.Context.PriceLists.AddObject(lp);
            appvm.PriceListsOC.Add(lp);

            appvm.SaveChanges();

            lp.PropertyChanged += saleArea_PropertyChanged;

            //AreaViewModel avm = new AreaViewModel(appvm, this, lp);
            //areasVM.Add(avm);
            SelectedArea = lp;
        }

        #endregion
        
        #region Remove Area Command

        RelayCommand deleteCommand;
        public ICommand DeleteCommand 
        {
            get             
            {
                if (deleteCommand == null) 
                {
                    deleteCommand = new RelayCommand(x => this.Delete(selectedArea), x => this.CanDelete);
                }
                return deleteCommand;
            }
        }

        bool CanDelete 
        {
            get { return SelectedArea != null; }
        }

        void Delete(PriceList areaToRemove)
        {
            //delete tables inside the area

            var msgBox = base.GetService<IMessageBoxService>();

            string message = "Está seguro que desea eliminar el área: " + areaToRemove.Name + "?";
            if (msgBox.ShowYesNoDialog(message) != true) return;

            List<Table> tablesToRemove = new List<Table>(areaToRemove.Tables);

            //remove associated tables
            foreach (var item in tablesToRemove)
            {
                appvm.TablesOC.Remove(item);
                appvm.Context.Tables.DeleteObject(item);
            }

            appvm.PriceListsOC.Remove(areaToRemove);
            appvm.Context.PriceLists.DeleteObject(areaToRemove);

            appvm.SaveChanges();          
        }

        #endregion
    }
}
