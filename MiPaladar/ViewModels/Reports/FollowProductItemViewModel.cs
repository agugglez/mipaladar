using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Enums;

namespace MiPaladar.ViewModels
{
    public enum InventoryOperationType {Inicio, Entrada, Salida}

    public class FollowProductItemViewModel : ViewModelBase
    {
        public FollowProductItemViewModel() { }

        //public FollowProductItemViewModel(InventoryTrace iTrace)
        //{
        //    OperationType = InventoryOperationType.Inicio;

        //    Date = iTrace.Date;
        //    //InventoryArea = iTrace.Inventory;
        //    Product = iTrace.Product;
        //    Quantity = iTrace.Quantity;
        //    UnitMeasure = iTrace.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
        //}

        //public FollowProductItemViewModel(LineItem li, Inventory pisoInventory)
        //{
        //    if (li.Order is Sale) 
        //    {
        //        if (li.Quantity >= 0) OperationType = InventoryOperationType.Salida;
        //        else OperationType = InventoryOperationType.Entrada;

        //        //InventoryArea = pisoInventory;
        //    }
        //    else if (li.Order is Purchase) 
        //    {
        //        if (li.Quantity >= 0) OperationType = InventoryOperationType.Entrada;
        //        else OperationType = InventoryOperationType.Salida;

        //        //InventoryArea = ((Purchase)li.Order).Inventory;
        //    }

        //    Quantity = Math.Abs(li.Quantity);
        //    UnitMeasure = li.UnitMeasure;
        //    Product = li.Product;

        //    //get this from Order
        //    Date = li.Order.DateCreated;           
                        
        //    Order = li.Order;            
        //}

        //public FollowProductItemViewModel(AdjustmentItem adj)
        //{
        //    //InventoryArea = adj.Adjustment.Inventory;

        //    if (adj.Quantity >= 0) OperationType = InventoryOperationType.Entrada;
        //    else OperationType = InventoryOperationType.Salida;

        //    Quantity = Math.Abs(adj.Quantity);
        //    UnitMeasure = adj.UnitMeasure;
        //    Product = adj.Product;

        //    //get this from Order
        //    Date = adj.Adjustment.Date;

        //    Order = adj.Adjustment;
        //}

        public DateTime Date { get; set; }

        public Product Product { get; set; }

        public UnitMeasure UnitMeasure { get; set; }

        public double Quantity { get; set; }

        public double InitialQuantity { get; set; }

        public double InQuantity { get; set; }

        public double OutQuantity { get; set; }

        public object Order { get; set; }

        //public Inventory InventoryArea { get; set; }

        public InventoryOperationType OperationType { get; set; }
    }
}