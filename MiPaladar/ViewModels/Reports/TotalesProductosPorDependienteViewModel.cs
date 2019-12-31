using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.Entities;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace MiPaladar.ViewModels
{
    public class TotalesProductosPorDependienteViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        TotalesViewModel totalesParent;

        public TotalesProductosPorDependienteViewModel(TotalesViewModel totalesParent, MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
            this.totalesParent = totalesParent;
        }

        public void UpdateTotals()
        {
            totals.Clear();
            foreach (var item in
                Accounter.UpdateTotalsByWaiterByProducts(appvm.Context, totalesParent.FromDate, totalesParent.ToDate))
            {
                totals.Add(item);
            }
        }

        ObservableCollection<TotalByWaiterByProduct> totals = 
            new ObservableCollection<TotalByWaiterByProduct>();

        ICollectionView icvTotals;
        public ICollectionView Totals 
        {
            get 
            {
                if (icvTotals == null) 
                {
                    //create view
                    CollectionViewSource cvs = new CollectionViewSource();
                    cvs.Source = totals;
                    icvTotals = (CollectionView)cvs.View;

                    if (icvTotals.CanGroup == true)
                    {
                        PropertyGroupDescription groupDescription
                            = new PropertyGroupDescription("Waiter.Name");
                        icvTotals.GroupDescriptions.Add(groupDescription);
                    }

                    //filter
                    icvTotals.Filter = new Predicate<object>(FilterPredicate);                    
                }
                return icvTotals;
            }            
        }

        public ObservableCollection<Category> Categories
        {
            get { return appvm.CategoriesOC; }
        }

        public ObservableCollection<Product> Products 
        {
            get { return appvm.ProductsOC; }
        }

        public ObservableCollection<Employee> Waiters 
        {
            get { return appvm.EmployeesOC; }
        }

        //string searchString;
        //public string SearchString
        //{
        //    get { return searchString; }
        //    set
        //    {
        //        searchString = value;
        //        icvTotals.Refresh();
        //    }
        //}

        bool Filtering         
        {
            get { return filteringByCategory || filteringByProduct || filteringByWaiter; }
        }

        bool filteringByCategory;
        public bool FilteringByCategory
        {
            get { return filteringByCategory; }
            set
            {
                filteringByCategory = value;
                OnPropertyChanged("FilteringByCategory");
                icvTotals.Refresh();
            }
        }

        Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                if (FilteringByCategory) icvTotals.Refresh();
            }
        }

        bool filteringByProduct;
        public bool FilteringByProduct
        {
            get { return filteringByProduct; }
            set
            {
                filteringByProduct = value;
                OnPropertyChanged("FilteringByProduct");
                icvTotals.Refresh();
            }
        }

        Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                if (FilteringByProduct) icvTotals.Refresh();
            }
        }

        bool filteringByWaiter;
        public bool FilteringByWaiter
        {
            get { return filteringByWaiter; }
            set
            {
                filteringByWaiter = value;
                OnPropertyChanged("FilteringByWaiter");
                icvTotals.Refresh();
            }
        }

        Employee selectedWaiter;
        public Employee SelectedWaiter
        {
            get { return selectedWaiter; }
            set
            {
                selectedWaiter = value;
                if (FilteringByWaiter) icvTotals.Refresh();
            }
        }

        public bool FilterPredicate(object b)
        {
            if (!Filtering) return false;

            TotalByWaiterByProduct tbp = b as TotalByWaiterByProduct;
            //
            bool cond1 = true;
            if (FilteringByProduct && SelectedProduct != null) cond1 = tbp.Product == SelectedProduct;
            //if (string.IsNullOrWhiteSpace(SearchString)) cond1 = true;
            //else
            //{
            //    string prefix = SearchString.Trim();

            //    if (tbp.Product.Name != null)
            //        cond1 = tbp.Product.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;
            //}            

            bool cond2 = true;
            if (FilteringByWaiter && SelectedWaiter != null) cond2 = tbp.Waiter == SelectedWaiter;

            //
            bool cond3 = true;

            if (FilteringByCategory && SelectedCategory != null) 
            {
                //var queryREsult = from c in tbp.Product.RelatedCategories
                //                  where c.Category.Id == SelectedCategory.Id
                //                  select c;

                //cond3 = queryREsult.Count() > 0;

                cond3 = false;

                foreach (var item in tbp.Product.RelatedCategories)
                {
                    if (item.Category == SelectedCategory)
                    {
                        cond3 = true;
                        break;
                    }
                }               
            }

            return cond1 && cond2 && cond3;
        }
    }
}
