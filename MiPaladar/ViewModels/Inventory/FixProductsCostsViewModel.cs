using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

using MiPaladar.MVVM;
using MiPaladar.Services;
using MiPaladar.Repository;
using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class WrongProductLine : ViewModelBase
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        decimal ingCost;
        public decimal IngredientsCost
        {
            get { return ingCost; }
            set
            {
                ingCost = value;
                OnPropertyChanged("IngredientsCost");
            }
        }
        decimal arbCost;
        public decimal ArbitraryCost
        {
            get { return arbCost; }
            set
            {
                arbCost = value;
                OnPropertyChanged("ArbitraryCost");
            }
        }

        decimal totCost;
        public decimal TotalCost
        {
            get { return totCost; }
            set
            {
                totCost = value;
                OnPropertyChanged("TotalCost");
            }
        }
    }

    public class FixProductsCostsViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        public FixProductsCostsViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }

        ObservableCollection<WrongProductLine> wrongProductsLines;
        public ObservableCollection<WrongProductLine> WrongProductsLines
        {
            get 
            {
                if (wrongProductsLines == null)
                {

                    wrongProductsLines = new ObservableCollection<WrongProductLine>();
                    FindWrongProducts();
                }
                return wrongProductsLines; 
            }
        }

        void FindWrongProducts()
        {
            using (var uow = base.GetNewUnitOfWork())
            {
                var invSVC = base.GetService<IInventoryService>();

                foreach (var prod in uow.ProductRepository.Get())
                {                    
                    if (!invSVC.CheckProductCost(prod))
                    {
                        decimal componentsCost = 0;

                        if (IsRecipe(prod))
                        {
                            componentsCost = prod.Ingredients.Sum(x => invSVC.GetProductCost(x.IngredientProduct, x.Quantity, x.UnitMeasure));
                            componentsCost /= (decimal)prod.RecipeQuantity;
                        }

                        WrongProductLine wpl = new WrongProductLine();
                        wpl.ProductName = prod.Name;
                        wpl.ProductId = prod.Id;
                        wpl.IngredientsCost = componentsCost;

                        wrongProductsLines.Add(wpl);
                    }
                }
            }
        }

        bool IsRecipe(Product p)
        {
            return p.ProductType == ProductType.FinishedGoods || p.ProductType == ProductType.WorkInProcess;
        }

        #region Fix Costs Command

        RelayCommand fixAllCommand;
        public RelayCommand FixAllCommand
        {
            get
            {
                if (fixAllCommand == null)
                {
                    fixAllCommand = new RelayCommand(x => FixAll(), x => CanFixAll);
                }
                return fixAllCommand;
            }
        }

        bool CanFixAll { get { return wrongProductsLines.Count > 0; } }

        void FixAll()
        {
            var invSVC = base.GetService<IInventoryService>();

            using (var unitOfWOrk = base.GetNewUnitOfWork())
            {
                foreach (var line in wrongProductsLines)
                {
                    var targetProduct = unitOfWOrk.ProductRepository.GetById(line.ProductId);
                    invSVC.UpdateProductCost(targetProduct);
                }

                unitOfWOrk.SaveChanges();
            }
            
        }
        #endregion
    }
}
