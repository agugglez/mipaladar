//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Entities;
//using MiPaladar.Services;
//using MiPaladar.Classes;

//using System.Windows.Input;
//using System.Collections.ObjectModel;

//namespace MiPaladar.ViewModels
//{
//    public class LineItemViewModel : ProductQuantityViewModel
//    {
//        //MainWindowViewModel appvm;
//        SaleViewModel parentSale;
//        SaleLineItem lineitem;
//        //IInventoryService inventorySvc;
        
//        Action onPriceChanged;
//        Action onCostChanged;

//        //public LineItemViewModel() { }

//        public LineItemViewModel(SaleLineItem li, SaleViewModel parentSaleViewModel, Action onPriceChanged, Action onCostChanged)
//            : base(li.Product, li.Quantity, li.UnitMeasure)
//        {
//            //this.appvm = appvm;
//            this.lineitem = li;
//            this.parentSale = parentSaleViewModel;
//            //this.inventorySvc = appvm.InventoryService;

//            this.onPriceChanged = onPriceChanged;
//            this.onCostChanged = onCostChanged;

//            isEntrant = li.Product.IsEntrant;

//            //InitIsEntrant();

//            //InitToSpecify();

//            //SpecifyFormatPopupVisible = false;

//            //base.WhiteSpaceError = true;

//            //printed = li.Printed.HasValue ? li.Printed.Value : false;
//        }

//        //#region Specify Product Formats

//        //void InitToSpecify()         
//        //{
//        //    toSpecify = false;

//        //    if (Product == null) return;

//        //    if (!Product.IsRecipe || Product.IsStorable) return;

//        //    foreach (var ing in Product.Ingredients)
//        //    {
//        //        //at least 2 diff formats to decide
//        //        if (ing.IngredientProduct.HasDifferentFormats && ing.IngredientProduct.ProductFormats.Count > 1)
//        //        {
//        //            toSpecify = true; break;
//        //        }
//        //    }            
//        //}

//        //bool toSpecify;

//        //public bool ToSpecify
//        //{
//        //    get { return toSpecify; }
//        //    set 
//        //    {
//        //        toSpecify = value;
//        //        OnPropertyChanged("ToSpecify");
//        //    }
//        //}

//        //#region Specify Format Popup Visibility        

//        //public bool SpecifyFormatPopupVisible { get; set; }

//        //RelayCommand toggleSpecifyFormatPopupVisibilityCommand;
//        //public ICommand ToggleSpecifyFormatPopupVisibilityCommand 
//        //{
//        //    get
//        //    {
//        //        if (toggleSpecifyFormatPopupVisibilityCommand == null)
//        //            toggleSpecifyFormatPopupVisibilityCommand = new RelayCommand(x => this.ToggleSpecifyFormatPopupVisibility());
//        //        return toggleSpecifyFormatPopupVisibilityCommand;
//        //    }
//        //}

//        //void ToggleSpecifyFormatPopupVisibility()
//        //{
//        //    SpecifyFormatPopupVisible = true;

//        //    if (SpecifyFormatPopupVisible) 
//        //    {
//        //        itemsToSpecify.Clear();

//        //        foreach (var ing in Product.Ingredients)
//        //        {
//        //            //at least 2 diff formats to decide
//        //            if (ing.IngredientProduct.HasDifferentFormats && ing.IngredientProduct.ProductFormats.Count > 1)
//        //            {
//        //                ToSpecifyItemViewModel tsivm = new ToSpecifyItemViewModel();

//        //                tsivm.GenericProduct = ing.IngredientProduct;

//        //                foreach (var format in ing.IngredientProduct.ProductFormats)
//        //                {
//        //                    FormatOptionViewModel fovm = new FormatOptionViewModel();
//        //                    fovm.Product = format.Product;

//        //                    bool isCheked = false;

//        //                    //check if it is present in used products
//        //                    foreach (var usedProduct in lineitem.UsedProducts)
//        //                    {
//        //                        if (usedProduct == format.Product) { isCheked = true; break; }
//        //                    }

//        //                    fovm.IsChecked = isCheked;

//        //                    tsivm.FormatOptions.Add(fovm);

//        //                }

//        //                itemsToSpecify.Add(tsivm);
//        //            }
//        //        }
//        //    }

//        //    OnPropertyChanged("SpecifyFormatPopupVisible");
//        //}

//        //#endregion

//        //ObservableCollection<ToSpecifyItemViewModel> itemsToSpecify = new ObservableCollection<ToSpecifyItemViewModel>();
//        //public ObservableCollection<ToSpecifyItemViewModel> ItemsToSpecify 
//        //{
//        //    get { return itemsToSpecify; }
//        //}

//        //#endregion        

//        RelayCommand clearPrintedCommand;
//        public ICommand ClearPrintedCommand 
//        {
//            get 
//            {
//                if (clearPrintedCommand == null)
//                    clearPrintedCommand = new RelayCommand(x => this.ClearPrintedFlag());
//                return clearPrintedCommand;
//            }
//        }

//        void ClearPrintedFlag() 
//        {
//            Printed = false;
//        }

//        //bool printed;
//        public bool Printed 
//        {
//            get { return LineItem.Printed; }
//            set 
//            {
//                LineItem.Printed = value;
//                OnPropertyChanged("Printed");
//            }            
//        }

//        //void InitIsEntrant() 
//        //{
//        //    if (Product != null && Product.IsEntrant.HasValue)
//        //    {
//        //        isEntrant = Product.IsEntrant.Value;
//        //    }
//        //}

//        bool isEntrant;
//        public bool IsEntrant
//        {
//            get { return isEntrant; }
//            set
//            {
//                isEntrant = value;
//                OnPropertyChanged("IsEntrant");
//            }
//        }

//        //protected override void OnQuantityExpressionChanging()
//        //{
//        //    ExecuteSellOperation(Product, Quantity, UnitMeasure);
//        //}

//        //protected override void OnQuantityExpressionChanged()
//        //{
//        //    lineitem.Quantity = Quantity;
//        //    lineitem.UnitMeasure = UnitMeasure;

//        //    ExecuteSellOperation(Product, -Quantity, UnitMeasure);

//        //    Price = Quantity * Product.SalePrice;
//        //}

//        protected override void OnQuantityChanging()
//        {
//            //ExecuteSellOperation(Product, Quantity, UnitMeasure);
//            var ts = base.GetService<ITransactionService>();
//            ts.UndoSell(Product, Quantity, lineitem.Cost, parentSale.WorkingDate);
//        }

//        protected override void OnQuantityChanged()
//        {
//            lineitem.Quantity = Quantity;
//            lineitem.UnitMeasure = UnitMeasure;

//            var ts = base.GetService<ITransactionService>();
//            decimal cost = ts.Sell(Product, Quantity, parentSale.WorkingDate);

//            lineitem.Cost = cost;
//            if (onCostChanged != null) onCostChanged();
//            //ExecuteSellOperation(Product, -Quantity, UnitMeasure);

//            //this saves changes
//            Price = (decimal)Quantity * Product.SalePrice;
//        }

//        //sales work against floor items
//        //protected void ExecuteSellOperation(Product prod, double quantity, UnitMeasure um)
//        //{
//        //    if (prod == null) return;

//        //    Inventory pisoInventory = appvm.InventoryAreasOC.Single(x => x.Name == "Piso");

//        //    if (prod.IsStorable)
//        //    {
//        //        inventorySvc.ExecuteInventoryOperation(parentSaleDate, pisoInventory, prod, quantity, um);
//        //    }
//        //    else if (prod.IsRecipe)
//        //    {
//        //        foreach (var tbp in prod.Ingredients)
//        //        {
//        //            inventorySvc.ExecuteInventoryOperation(parentSaleDate, pisoInventory, tbp.IngredientProduct, tbp.Quantity * quantity, tbp.UnitMeasure);
//        //        }
//        //    }
//        //}

//        public decimal Cost
//        {
//            get { return lineitem.Cost; }
//        }

//        public decimal Price
//        {
//            get { return lineitem.Amount; }
//            set
//            {
//                lineitem.Amount = value;
//                OnPropertyChanged("Price");
//                onPriceChanged();
//            }
//        }

//        public SaleLineItem LineItem { get { return lineitem; } }
//    }

//    //public class ToSpecifyItemViewModel 
//    //{
//    //    public Product GenericProduct { get; set; }

//    //    ObservableCollection<FormatOptionViewModel> formatOptions = new ObservableCollection<FormatOptionViewModel>();
//    //    public ObservableCollection<FormatOptionViewModel> FormatOptions 
//    //    {
//    //        get { return formatOptions; }
//    //    }
//    //}

//    //public class FormatOptionViewModel 
//    //{
//    //    public Product Product { get; set; }
//    //    public bool IsChecked { get; set; }
//    //}
//}
