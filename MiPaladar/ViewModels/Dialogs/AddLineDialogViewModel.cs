using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.MVVM;
using MiPaladar.Entities;
using MiPaladar.Services;

using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class AddLineDialogViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        Predicate<Product> filter;
        bool showUoM;

        public AddLineDialogViewModel(MainWindowViewModel appvm, Predicate<Product> filter, bool showUoM = true)
        {
            this.appvm = appvm;
            this.filter = filter;
            this.showUoM = showUoM;
        }

        public double Quantity { get; set; }

        #region UMs

        List<UnitMeasure> allUMs;
        ObservableCollection<UnitMeasure> availableUMs = new ObservableCollection<UnitMeasure>();

        public ObservableCollection<UnitMeasure> AvailableUMs
        {
            get
            {
                if (allUMs == null)
                {
                    UpdateAvailableUMs();
                }
                return availableUMs;
            }
        }

        private void UpdateAvailableUMs()
        {
            if (allUMs == null)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    allUMs = unitOfWork.UMRepository.Get();
                }
            }

            availableUMs.Clear();
            if (sltdProd == null) return;
            foreach (var item in allUMs)
            {
                if (item.UMFamilyId == sltdProd.UMFamilyId) availableUMs.Add(item);
            }         
        }

        #endregion

        UnitMeasure um;
        public UnitMeasure UnitOfMeasure
        {
            get { return um; }
            set
            {
                um = value;
                OnPropertyChanged("UnitOfMeasure");
            }
        }

        public bool UnitOfMeasureVisible { get { return showUoM; } }

        Product sltdProd;
        public Product SelectedProduct
        {
            get { return sltdProd; }
            set
            {
                sltdProd = value;

                UpdateAvailableUMs();

                UnitOfMeasure = availableUMs.FirstOrDefault(x => x.IsFamilyBase);

                //OnPropertyChanged("SelectedProduct");
            }
        }

        public string SearchText { get; set; }

        ObservableCollection<Product> productList;
        public ObservableCollection<Product> ProductList
        {
            get 
            {
                if (productList == null)
                {
                    productList = new ObservableCollection<Product>();

                    using (var unitOfWork = base.GetNewUnitOfWork())
                    {
                        foreach (var item in unitOfWork.ProductRepository.Get(includeProperties: "Ingredients").OrderBy(x => x.Name))
                        {
                            if (filter(item)) productList.Add(item);
                        }
                    }                    
                }
                return productList;
            }
        }

        #region Ok Command

        RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                if (okCommand == null)
                {
                    okCommand = new RelayCommand(x => { }, x => CanOk);
                }
                return okCommand;
            }
        }

        bool CanOk { get { return sltdProd != null && Quantity > 0; } }

        #endregion

        #region New Product Command

        RelayCommand newProductCmd;

        public RelayCommand NewProductCommand
        {
            get
            {
                if (newProductCmd == null)
                    newProductCmd = new RelayCommand(x => NewProduct());
                return newProductCmd;
            }
        }

        public void NewProduct()
        {
            ProductViewModel pvm = new ProductViewModel(appvm);
            pvm.Name = SearchText;

            var windowManager = base.GetService<IWindowManager>();

            windowManager.ShowDialog(pvm, this);

            if (pvm.DialogResult)
            {
                Product pCreated = base.GetNewUnitOfWork().ProductRepository.GetById(pvm.ProductId);

                if (filter(pCreated)) productList.Add(pCreated);
            }
        }        

        #endregion

    }
}
