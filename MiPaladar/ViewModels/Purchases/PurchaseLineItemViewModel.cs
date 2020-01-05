//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Entities;
//using MiPaladar.Services;
//using MiPaladar.Classes;

//namespace MiPaladar.ViewModels
//{
//    public class PurchaseLineItemViewModel : ProductQuantityViewModel
//    {
//        //MainWindowViewModel appvm;
//        //DateTime workingDate;
//        //PurchaseLineItem lineitem;
//        PurchaseViewModel parentPurchase;
        
//        //IInventoryService inventoryService;

//        public PurchaseLineItemViewModel( PurchaseViewModel parentPurchase, 
//            Product prod, double qtty, UnitMeasure um, decimal price)
//            : base(prod, qtty, um)
//        {
//            this.parentPurchase = parentPurchase;

//            this.price = price;
//        }

//        public PurchaseLineItemViewModel( PurchaseViewModel parentPurchase, PurchaseLineItem li)
//            : base(li.Product, li.Quantity, li.UnitMeasure)
//        {
//            //this.appvm = appvm;            
//            //this.lineitem = li;
//            this.parentPurchase = parentPurchase;

//            this.price = li.Amount;
//            //this.workingDate = workingDate;
//            //this.onPriceChanged = onPriceChanged;
//            ////this.workingDate = workingDate;
//            //this.inventoryService = appvm.InventoryService;

//            //quantity = li.Quantity;
//            //um = li.UnitMeasure;
//            //prod = li.Product;
//        }

//        protected override void OnQuantityChanged()
//        {
//           Price = ProductManager.GetCurrentCost(Product, Quantity, UnitMeasure);

//            parentPurchase.HasPendingChanges = true;
//        }

//        protected override void OnUnitMeasureChanged()
//        {
//            Price = ProductManager.GetCurrentCost(Product, Quantity, UnitMeasure);

//            parentPurchase.HasPendingChanges = true;
//        }

//        //use [count] to only execute an inventory operation once both quantity and 
//        //unitmeasure have been set
//        //int count = 0;
//        //float quantity_old; float quantity;
//        //public float Quantity 
//        //{
//        //    get { return quantity; }
//        //    set 
//        //    {
//        //        quantity_old = lineitem.Quantity;
//        //        lineitem.Quantity = value;
//        //        quantity = value;

//        //        if (++count % 2 == 0) 
//        //        {
//        //            OnQuantityExpressionChanged();
//        //        }
//        //    }
//        //}
//        //UnitMeasure um_old;
//        //UnitMeasure um;
//        //public UnitMeasure UnitMeasure
//        //{
//        //    get { return um; }
//        //    set 
//        //    {
//        //        um = value;
//        //        if (um != null && um.UMFamily == Product.UMFamily)
//        //        {
//        //            um_old = lineitem.UnitMeasure;
//        //            lineitem.UnitMeasure = value;
//        //        }
//        //        if (++count % 2 == 0)
//        //        {
//        //            OnQuantityExpressionChanged();
//        //        }
//        //    }
//        //}
//        //Product prod;
//        //public Product Product 
//        //{
//        //    get { return prod; }
//        //    //set 
//        //    //{
//        //    //    ExecuteOperation(Product, -Quantity);
//        //    //    lineitem.Product = value;
//        //    //    ExecuteOperation(Product, Quantity);
//        //    //}
//        //}        

//        //protected override void OnQuantityExpressionChanging()
//        //{
//        //    ExecuteOperation(Product, -Quantity, UnitMeasure);
//        //}

//        //protected override void OnQuantityExpressionChanged() 
//        //{
//        //    lineitem.Quantity = Quantity;
//        //    lineitem.UnitMeasure = UnitMeasure;

//        //    ExecuteOperation(Product, Quantity, UnitMeasure);

//        //    //this will refresh total
//        //    Price = Quantity * Product.PurchasePrice;
//        //}

//        //protected override void OnQuantityChanging()
//        //{
//        //    //ExecuteOperation(Product, -Quantity, UnitMeasure);
//        //    var ts = base.GetService<ITransactionService>();
//        //    ts.UndoPurchase(Product, Quantity, UnitMeasure, Price, parentPurchase.Date, parentPurchase.Destination);
//        //}
//        //protected override void OnQuantityChanged()
//        //{
//        //    lineitem.Quantity = Quantity;

//        //    //ExecuteOperation(Product, Quantity, UnitMeasure);

//        //    //Price = ProductManager.CalculateCost(Product, Quantity, UnitMeasure);
//        //    //Price = Product.PurchasePrice * (decimal)(Quantity * UnitMeasure.ToBaseConversion);

//        //    var ts = base.GetService<ITransactionService>();
//        //    ts.Purchase(Product, Quantity, UnitMeasure, Price, parentPurchase.Date, parentPurchase.Destination);

//        //    appvm.SaveChanges();
//        //}

//        //protected override void OnUnitMeasureChanging() 
//        //{
//        //    //ExecuteOperation(Product, -Quantity, UnitMeasure);
//        //    var ts = base.GetService<ITransactionService>();
//        //    ts.UndoPurchase(Product, Quantity, UnitMeasure, Price, parentPurchase.Date, parentPurchase.Destination);
//        //}
//        //protected override void OnUnitMeasureChanged()
//        //{
//        //    lineitem.UnitMeasure = UnitMeasure;

//        //    //ExecuteOperation(Product, Quantity, UnitMeasure);

//        //    //Price = ProductManager.CalculateCost(Product, Quantity, UnitMeasure);
//        //    //Price = Product.PurchasePrice * (decimal)(Quantity * UnitMeasure.ToBaseConversion);

//        //    var ts = base.GetService<ITransactionService>();
//        //    ts.Purchase(Product, Quantity, UnitMeasure, Price, parentPurchase.Date, parentPurchase.Destination);

//        //    appvm.SaveChanges();
//        //}

//        //void ExecuteOperation(Product prod, double quantity, UnitMeasure um)
//        //{
//        //    if (parentPurchase.Destination == null) return;
//        //    inventoryService.ExecuteInventoryOperation(parentPurchase.Date, parentPurchase.Destination, prod, quantity, um);
//        //    //if (parentPurchase.DestinationCode == PurchaseViewModel.Warehouse_Destination_Code)
//        //    //{
                

//        //    //    //temporary solution to update vale's total price
//        //    //    //vale.RefreshTotal();

//        //    //    //UpdateTotalVentas();
//        //    //}
//        //    //else if (parentPurchase.DestinationCode == PurchaseViewModel.Floor_Destination_Code)
//        //    //{
//        //    //    inventoryService.ExecuteFloorOperation(prod, quantity, um);
//        //    //}
//        //    //else throw new Exception("Destination code not recognized");
//        //}

//        //public void Load(MainWindowViewModel appvm, PurchaseViewModel purchase, LineItem li, DateTime workingDate,
//        //    IInventoryService inventoryService)
//        //{
//        //    this.appvm = appvm;
//        //    this.purchase = purchase;
//        //    this.lineitem = li;
//        //    this.workingDate = workingDate;
//        //    this.inventoryService = inventoryService;
//        //}

//        decimal price;
//        public decimal Price
//        {
//            get { return price; }
//            set
//            {
//                if (value > 0 && price != value) 
//                {
//                    price = value;

//                    OnPropertyChanged("Price");
//                    //lineitem.Amount = value;
//                    //OnPropertyChanged("Price");
//                    parentPurchase.RefreshTotal();

//                    parentPurchase.HasPendingChanges = true;
//                }
//            }
//        }

//        //public PurchaseLineItem LineItem { get { return lineitem; } }

//        //protected void ExecuteSimpleOperation(Product prod, float quantity, UnitMeasure um)
//        //{
//        //    inventoryService.ExecuteSimpleOperation(workingDate, prod, quantity, um);
//        //}
//    }
//}
