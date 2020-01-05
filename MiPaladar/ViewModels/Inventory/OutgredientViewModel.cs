using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class OutgredientViewModel : ViewModelBase  //ProductQuantityViewModel
    {

        public OutgredientViewModel(){}

        public OutgredientViewModel(Ingredient source)
        {
            Id = source.Id;

            quantity = source.Quantity;
            unitMeasure = source.UnitMeasure;

            baseProduct = source.BaseProduct;
            ingredientProduct = source.IngredientProduct;
        }

        public int Id { get; set; }

        double quantity;
        public double Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value) 
                {
                    quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        UnitMeasure unitMeasure;
        public UnitMeasure UnitMeasure
        {
            get { return unitMeasure; }
            set
            {
                if (unitMeasure != value) 
                {
                    unitMeasure = value;
                    OnPropertyChanged("UnitMeasure");
                }                
            }
        }

        Product ingredientProduct;
        public Product IngredientProduct
        {
            get { return ingredientProduct; }
            set
            {
                if (ingredientProduct != value)
                {
                    ingredientProduct = value;
                    OnPropertyChanged("IngredientProduct");
                }
            }
        }

        Product baseProduct;
        public Product BaseProduct
        {
            get { return baseProduct; }
            set
            {
                if (baseProduct != value)
                {
                    baseProduct = value;
                    OnPropertyChanged("BaseProduct");
                }
            }
        }
    }
}