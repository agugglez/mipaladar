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
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace MiPaladar.ViewModels
{
    public class TotalesPorProductoViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        TotalesViewModel totalesParent;

        public TotalesPorProductoViewModel(TotalesViewModel totalesParent, MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
            this.totalesParent = totalesParent;

            showAllProducts = true;
        }

        public void UpdateTotals()
        {
            //totals by product
            //var orderItems = from v in appvm.Context.OrderItems
            //                 where v.Order.WorkingDate >= totalesParent.FromDate && v.Order.WorkingDate <= totalesParent.ToDate
            //                 select v;

            int temp = Environment.TickCount;

            totalsProduct.Clear();
            foreach (var item in Accounter.UpdateTotalsByProduct(appvm.Context, 
                selectedCentro, totalesParent.FromDate, totalesParent.ToDate))
            {
                totalsProduct.Add(item);
            }

            Time = Environment.TickCount - temp;
        }

        int time;
        public int Time 
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        ObservableCollection<TotalByProduct> totalsProduct = new ObservableCollection<TotalByProduct>();
        CollectionView cvTotalsProduct;
        public CollectionView TotalsProduct
        {
            get
            {
                if (cvTotalsProduct == null)
                {
                    //create view
                    CollectionViewSource cvs = new CollectionViewSource();
                    cvs.Source = totalsProduct;
                    cvTotalsProduct = (CollectionView)cvs.View;

                    //sort
                    cvTotalsProduct.SortDescriptions.Add(new SortDescription("Product.Name", ListSortDirection.Ascending));
                    //filter
                    cvTotalsProduct.Filter = new Predicate<object>(FilterPredicate);
                }

                return cvTotalsProduct;
            }
        }

        public bool FilterPredicate(object b)
        {
            TotalByProduct tbp = b as TotalByProduct;

            //
            bool cond1 = false;

            if (string.IsNullOrWhiteSpace(SearchString)) cond1 = true;
            else 
            {
                string prefix = SearchString.Trim();

                if (tbp.Product.Name != null)
                    cond1 = tbp.Product.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;
            }            

            //
            bool cond2 = false;

            if (ShowAllProducts) cond2 = true;
            else if (ShowProductsInMenu) cond2 = !tbp.Product.NotInMenu;
            //else if (ShowIngredients) cond2 = tbp.Product.IsIngredient;

            //
            bool cond3 = false;

            if (!FilteringByCategory || SelectedCategory == null) cond3 = true;
            else
            {
                var queryREsult = from p in SelectedCategory.RelatedProducts
                                  where p.Product != null && p.Product.Id == tbp.Product.Id
                                  select p;

                cond3 = queryREsult.Count() > 0;
            }

            return cond1 && cond2 && cond3;
        }

        string searchString;
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                cvTotalsProduct.Refresh();
            }
        }

        bool showAllProducts;
        public bool ShowAllProducts
        {
            get { return showAllProducts; }
            set
            {
                showAllProducts = value;
                cvTotalsProduct.Refresh();
            }
        }

        bool showProductsInMenu;
        public bool ShowProductsInMenu
        {
            get { return showProductsInMenu; }
            set
            {
                showProductsInMenu = value;
                cvTotalsProduct.Refresh();
            }
        }

        //bool showIngredients;
        //public bool ShowIngredients
        //{
        //    get { return showIngredients; }
        //    set
        //    {
        //        showIngredients = value;
        //        cvTotalsProduct.Refresh();
        //    }
        //}

        bool filteringByCategory;
        public bool FilteringByCategory
        {
            get { return filteringByCategory; }
            set 
            { 
                filteringByCategory = value;
                OnPropertyChanged("FilteringByCategory");
                if (SelectedCategory != null) cvTotalsProduct.Refresh();
            }
        }

        Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set 
            {
                selectedCategory = value;
                if (FilteringByCategory) cvTotalsProduct.Refresh();
            }
        }

        public ObservableCollection<Category> CategoriesOC 
        {
            get { return appvm.CategoriesOC; }
        }

        bool filteringByCentro;
        public bool FilteringByCentro         
        {
            get 
            {
                return filteringByCentro;
            }
            set 
            {
                filteringByCentro = value;
                OnPropertyChanged("FilteringByCentro");
                if (SelectedCentro != null) UpdateTotals();
            }
        }

        PriceList selectedCentro;
        public PriceList SelectedCentro         
        {
            get { return selectedCentro; }
            set 
            {
                selectedCentro = value;
                if (filteringByCentro) UpdateTotals();
            }
        }

        public ObservableCollection<PriceList> Centros 
        {
            get { return appvm.PriceListsOC; }
        }


        public bool Grouping { get; set; }

        RelayCommand groupCommand;
        public ICommand GroupCommand 
        {
            get 
            {
                if (groupCommand == null)
                    groupCommand = new RelayCommand(x => this.Group());
                return groupCommand;
            }
        }

        void Group() 
        {
            cvTotalsProduct.GroupDescriptions.Clear();

            if (Grouping)
            {
                if (cvTotalsProduct.CanGroup == true)
                {
                    PropertyGroupDescription groupDescription
                        = new PropertyGroupDescription("Product.MainCategory.Name");
                    cvTotalsProduct.GroupDescriptions.Add(groupDescription);
                }
                else
                    return;
            }
        }

        RelayCommand exportToExcel;
        public ICommand ExportCommand
        {
            get
            {
                if (exportToExcel == null)
                    exportToExcel = new RelayCommand(x => this.Export());
                return exportToExcel;
            }
        }

        private void Export()
        {
            List<TotalByProduct> totalsList = new List<TotalByProduct>();

            foreach (var item in cvTotalsProduct)
            {
                TotalByProduct tbp = (TotalByProduct)item;

                totalsList.Add(tbp);
            }

            DisplayInExcel(totalsList, (total, cell) =>
            // This multiline lambda expression sets custom processing rules  
            // for the bankAccounts.
            {
                cell.Value = total.Product.Name;
                cell.Offset[0, 1].Value = total.Total;
                //if (invItem.Balance < 0)
                //{
                //    cell.Interior.Color = 255;
                //    cell.Offset[0, 1].Interior.Color = 255;
                //}
            });
        }

        void DisplayInExcel(IEnumerable<TotalByProduct> accounts,
             Action<TotalByProduct, Excel.Range> DisplayFunc)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();

            // Add a new Excel workbook.
            excelApp.Workbooks.Add();

            excelApp.Range["A1"].Value = "Producto";
            excelApp.Range["B1"].Value = "Cantidad";
            excelApp.Range["A2"].Select();

            foreach (var ac in accounts)
            {
                DisplayFunc(ac, excelApp.ActiveCell);
                excelApp.ActiveCell.Offset[1, 0].Select();
            }

            // Copy the results to the Clipboard.
            //excelApp.Range["A1:B3"].Copy();

            excelApp.Columns[1].AutoFit();
            excelApp.Columns[2].AutoFit();

            excelApp.Visible = true;
        }
    }
}
