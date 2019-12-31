using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using MiPaladar.ViewModels;
using MiPaladar.Entities;

namespace MiPaladar.SampleData
{
    public class SampleProduct
    {
        #region Properties

        public string Name
        {
            get;
            set;
        }

        public double SalePrice
        {
            get;
            set;
        }

        public bool IsPurchasable
        {
            get;
            set;
        }

        public bool IsIngredient
        {
            get;
            set;
        }

        public bool IsRecipe
        {
            get;
            set;
        }

        public bool IsStorable
        {
            get;
            set;
        }

        public bool NotInMenu
        {
            get;
            set;
        }


        public ProductionArea ProductionArea
        {
            get;
            set;
        }


        public bool IsProduced
        {
            get;
            set;
        }


        public bool IsEntrant
        {
            get;
            set;
        }


        public UMFamily UMFamily
        {
            get;
            set;
        }

        public bool HasDifferentFormats { get; set; }

        #endregion

        #region Product Cost

        //price by unit measure family base, will change name later

        public double PurchasePrice
        {
            get;
            set;
        }

        //price in cost expression

        public double CostPrice
        {
            get;
            set;
        }

        //quantity in cost expression

        public double CostQuantity
        {
            get;
            set;
        }

        //unit measure in cost expression

        public UnitMeasure CostUnitMeasure
        {
            get;
            set;
        }

        public string CostQuantityString
        {
            get;
            set;
        }

        public double PriceRate
        {
            get;
            set;
        }

        #endregion       

        ObservableCollection<ProductIndexViewModel> relatedCategories = new ObservableCollection<ProductIndexViewModel>();
        public ObservableCollection<ProductIndexViewModel> RelatedCategories
        {
            get { return relatedCategories; }
        }

        ObservableCollection<IngredientViewModel> ingredients = new ObservableCollection<IngredientViewModel>();
        public ObservableCollection<IngredientViewModel> Ingredients
        {
            get { return ingredients; }
        }

        public bool HasCategories
        {
            get { return relatedCategories.Count > 0; }
        }


        
    }
}
