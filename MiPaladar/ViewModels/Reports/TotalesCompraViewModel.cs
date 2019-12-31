using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.Entities;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace MiPaladar.ViewModels
{
    public class TotalesCompraViewModel:ViewModelBase
    {
        MainWindowViewModel appvm;
        TotalesViewModel totalesParent;

        public TotalesCompraViewModel(TotalesViewModel totalesParent, MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
            this.totalesParent = totalesParent;
        }

        public void UpdateTotals()
        {
            totalsCompras.Clear();
            foreach (var item in Accounter.UpdateTotalsCompras(appvm.Context, totalesParent.FromDate, totalesParent.ToDate))
            {
                totalsCompras.Add(item);
            }
        }

        ObservableCollection<TotalByProduct> totalsCompras = new ObservableCollection<TotalByProduct>();
        ICollectionView icvTotalsCompras;
        public ICollectionView TotalsCompras
        {
            get 
            {
                if (icvTotalsCompras == null) 
                {
                    CollectionViewSource cvs = new CollectionViewSource();
                    cvs.Source = totalsCompras;
                    icvTotalsCompras = cvs.View;

                    //sort
                    icvTotalsCompras.SortDescriptions.Add(new SortDescription("Product.Name", ListSortDirection.Ascending));
                    //filter
                    icvTotalsCompras.Filter = new Predicate<object>(FilterCompras);
                }
                return icvTotalsCompras;
            }
        }

        string searchString;
        public string SearchString 
        {
            get { return searchString; }
            set 
            {
                searchString = value;
                icvTotalsCompras.Refresh();
            }
        }

        public bool FilterCompras(object b)
        {
            TotalByProduct tbp = b as TotalByProduct;
            
            bool cond1 = false;

            if (string.IsNullOrWhiteSpace(SearchString)) cond1 = true;
            else 
            {
                string prefix = SearchString.Trim();

                string name = tbp.Product.Name;

                if (tbp.Product.Name != null) 
                    cond1 = name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;
            }            

            return cond1;
        }
    }
}
