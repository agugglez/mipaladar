using System;
using System.Collections.Generic;
using System.Linq;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

using System.ComponentModel;

namespace MiPaladar.ViewModels
{
    public enum InventoryItemType { Product, Category }

    public class InventoryItemViewModel : ViewModelBase
    {
        public InventoryItemViewModel(Category category, int categoryLevel)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.Level = categoryLevel;            

            ItemType = InventoryItemType.Category;
        }

        public InventoryItemViewModel(Product product, int productLevel)
        {
            this.Id = product.Id;            
            this.Name = product.Name;
            this.Level = productLevel;

            this.pType = product.ProductType;

            if (pType == ProductType.CompraVenta || pType == ProductType.FinishedGoods)
            {
                this.PLU = product.Code;
                this.Price = product.SalePrice;
            }            

            ItemType = InventoryItemType.Product;

            doneByUser = product.DoneByUser;

            //complPercent = 50;//GetCompletionPercent(product);
        }

        public InventoryItemViewModel(int id, string name, int level, InventoryItemType itemType)
        {
            this.Id = id;
            this.Name = name;
            this.Level = level;

            ItemType = itemType;
        }

        //DISPOSE
        //protected override void OnDispose()
        //{
        //    //workers
        //    if (complPercentWorker != null)
        //    {
        //        complPercentWorker.DoWork -= complPercentWorker_DoWork;
        //        complPercentWorker.RunWorkerCompleted -= complPercentWorker_RunWorkerCompleted;
        //    }
        //}
        
        public int Id { get; private set; }
        public string Name { get; private set; }        
        public int Level { get; private set; }

        //ITEMTYPE
        public InventoryItemType ItemType { get; private set; }

        //to show bold
        public bool IsCategory { get { return ItemType == InventoryItemType.Category; } }

        //PLU & PRICE
        public int? PLU { get; private set; }
        public decimal? Price { get; private set; }

        //PRODUCT TYPE
        ProductType pType;
        public string ProductTypeString
        {
            get { return ItemType == InventoryItemType.Product ? ProductTypeInSpanish() : null; }
        }

        string ProductTypeInSpanish()
        {
            string result;
            switch (pType)
            {
                case ProductType.FinishedGoods:
                    result = "Venta";
                    break;
                case ProductType.WorkInProcess:
                    result = "Elaboración";
                    break;
                case ProductType.RawMaterials:
                    result = "Primario";
                    break;
                case ProductType.CompraVenta:
                    result = "Compra-Venta";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        bool doneByUser;
        public bool DoneByUser
        {
            get { return doneByUser; }
        }

        //#region Completion Percent

        //BackgroundWorker complPercentWorker;

        //void CalculateCompletionPercent()
        //{
        //    if (complPercentWorker == null)
        //    {
        //        complPercentWorker = new BackgroundWorker();

        //        complPercentWorker.DoWork += complPercentWorker_DoWork;

        //        complPercentWorker.RunWorkerCompleted += complPercentWorker_RunWorkerCompleted;
        //    }

        //    complPercentWorker.RunWorkerAsync();
        //    busy = true;
        //}

        //void complPercentWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    var invSvc = base.GetService<IInventoryService>();
        //    e.Result = invSvc.GetProductCompletionPercent(Id);
        //}

        //void complPercentWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    CompletionPercent = (double)e.Result;
        //    CompletionPercentVisible = CompletionPercent < 100;

        //    busy = false;
        //}

        ////COMPLETION PERCENT
        //double complPercent; bool busy;
        //public double CompletionPercent
        //{
        //    get
        //    {
        //        //if (!busy && ItemType == InventoryItemType.Product && complPercent == 0)
        //        //{
        //        //    CalculateCompletionPercent();
        //        //}
        //        return complPercent;
        //    }
        //    private set
        //    {
        //        complPercent = value;
        //        OnPropertyChanged("CompletionPercent");
        //    }
        //}

        //bool complPercentVisible;
        public bool CompletionPercentVisible
        {
            get { return ItemType == InventoryItemType.Product; }
            //set
            //{
            //    if (complPercentVisible != value)
            //    {
            //        complPercentVisible = value;
            //        OnPropertyChanged("CompletionPercentVisible");
            //    }
            //}
        }

        //#endregion                
    }

    //public class PassthruDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    //{
    //     //... other stuff

    //    public new TValue this[TKey key]
    //    {
    //        get
    //        {
    //            TValue value;
    //            if (TryGetValue(key, out value))
    //            {
    //                return value;
    //            }
    //            else
    //            {
    //                return default(TValue);
    //            }
    //        }
    //    }
    //}
}
