using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Input;

using MiPaladar.Entities;
using MiPaladar.Views;
using MiPaladar.Enums;
using MiPaladar.Services;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class MiniMenuViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        //ObservableCollection<ProductViewModel> productVMs;

        public MiniMenuViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;          
        }        

        //ICollectionView cvProducts;
        //public ICollectionView Products
        //{
        //    get
        //    {
        //        if (cvProducts == null)
        //        {
        //            //create the view
        //            CollectionViewSource myCVS = new CollectionViewSource();
        //            myCVS.Source = appvm.ProductsOC;
        //            cvProducts = myCVS.View;

        //            //sort by name
        //            cvProducts.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        //            //filter by category
        //            cvProducts.Filter = new Predicate<object>(MenuFilterPredicate);
        //        }

        //        return cvProducts;
        //    }
        //}

        #region Filtering
        ObservableCollection<Product> products_source = new ObservableCollection<Product>();
        //public ObservableCollection<InventoryItemViewModel> ItemsList
        //{
        //    get { return sourceList; }
        //}

        ObservableCollection<Product> products_filtered;
        public ObservableCollection<Product> Products
        {
            get
            {
                if (products_filtered == null)
                {
                    products_filtered = new ObservableCollection<Product>();

                    OnFirstTime();
                }

                return products_filtered;
            }
        }

        void OnFirstTime()
        {
            products_source = new ObservableCollection<Product>(appvm.ProductsOC);

            RefreshItems();
        }

        private void RefreshItems()
        {
            products_filtered.Clear();

            //add those that pass the filter
            foreach (var item in products_source)
            {
                if (PassesFilter(item))
                {
                    //if (!filteredItems.Contains(item)) 
                    products_filtered.Add(item);
                }
            }
        }

        private bool PassesFilter(Product p)
        {
            //show visible items only
            if (!p.IsInMenu) return false;

            bool cond1 = false;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                cond1 = true;
            }
            else
            {
                string prefix = searchText.Trim();

                if (p.Name != null)
                    cond1 = p.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;

            }

            return cond1;

            //bool cond2 = true;

            //if (filteringByCategory && selectedCategory != null)
            //{
            //    cond2 = false;

            //    foreach (var item in p.RelatedCategories)
            //    {
            //        if (item.Category == selectedCategory)
            //        {
            //            cond2 = true;
            //            break;
            //        }
            //    }
            //}

            //return cond2; 
        }

        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value) 
                {
                    searchText = value;
                    RefreshItems();
                }                
            }
        }        

        #endregion        

        public ObservableCollection<Category> Categories
        {
            get { return appvm.CategoriesOC; }
        }        
    }
}
