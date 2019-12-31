using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Classes;

namespace MiPaladar.ViewModels
{
    public class ProductionItemViewModel : ProductQuantityViewModel
    {
        ProductionViewModel parentProduction;

        public ProductionItemViewModel(ProductionViewModel parentProduction, Product prod, double qtty, UnitMeasure um)
            : base(prod, qtty, um)
        {
            this.parentProduction = parentProduction;
            //base.WhiteSpaceError = true;
        }

        public ProductionItemViewModel(ProductionViewModel parentProduction, ProductionItem productionItem)
            : base(productionItem.Product, productionItem.Quantity, productionItem.UnitMeasure)
        {
            this.parentProduction = parentProduction;

            //base.WhiteSpaceError = true;
        }

        protected override void OnQuantityChanged()
        {
            parentProduction.HasPendingChanges = true;
        }

        protected override void OnUnitMeasureChanged()
        {
            parentProduction.HasPendingChanges = true;
        }
    }
}
