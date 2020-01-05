//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Entities;
//using MiPaladar.Classes;

//namespace MiPaladar.ViewModels
//{
//    public class AdjustmentItemViewModel : ProductQuantityViewModel
//    {
//        //AdjustmentItem adjustmentItem;
//        AdjustmentViewModel parentAdjustment;

//        public AdjustmentItemViewModel(AdjustmentViewModel parentAdjustment, Product prod, double qtty, UnitMeasure um)
//            : base(prod, qtty, um)
//        {
//            this.parentAdjustment = parentAdjustment;
//        }

//        public AdjustmentItemViewModel(AdjustmentViewModel parentAdjustment, AdjustmentItem adjustmentItem)
//            : base(adjustmentItem.Product, adjustmentItem.Quantity, adjustmentItem.UnitMeasure)
//        {
//            this.parentAdjustment = parentAdjustment;
//            //this.adjustmentItem = adjustmentItem;
//        }

//        //public AdjustmentItem AdjustmentItem 
//        //{
//        //    get { return adjustmentItem; }
//        //}

//        protected override void OnQuantityChanged()
//        {
//            parentAdjustment.HasPendingChanges = true;
//        }

//        protected override void OnUnitMeasureChanged()
//        {
//            parentAdjustment.HasPendingChanges = true;
//        }
//    }
//}
