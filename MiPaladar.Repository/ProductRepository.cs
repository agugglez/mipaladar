using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetFromPLU(int plu);
    }

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(RestaurantDBEntities context)
            : base(context)
        {
            this.context = context;
        }

        //#region Product defaults

        //public void SetFinishedGoodsDefaults(Product prod)
        //{
        //    prod.ProductType = ProductType.FinishedGoods;

        //    //unit of measure
        //    UMFamily qttyFamily = context.UMFamilies.Single();
        //    prod.UMFamily = qttyFamily;
        //    UnitMeasure unitUM = qttyFamily.UnitMeasures.First();
        //    prod.RecipeUnitMeasure = prod.CostUnitMeasure = unitUM;
        //    prod.RecipeQuantity = 1;
        //}
        //#endregion

        //public List<Product> Get()
        //{
        //    return context.Products.Include("Ingredients").ToList();
        //}

        //public Product GetById(int id)
        //{
        //    return context.Products.FirstOrDefault(x => x.Id == id);
        //}

        public Product GetFromPLU(int plu)
        {
            return context.Products.FirstOrDefault(x => x.Code == plu);
        }

        public override void Add(Product prod)
        {
            if (prod.ProductType == ProductType.FinishedGoods)
            {
                prod.UMFamily = context.UMFamilies.Single(x => x.Id == 1);
                prod.RecipeUnitMeasure = prod.CostUnitMeasure = context.UnitMeasures.Single(x => x.Id == 1);
                prod.RecipeQuantity = 1;
            }

            base.Add(prod);             
        }

        //public void Add(Product prod)
        //{
        //    if (prod.ProductType == ProductType.FinishedGoods)
        //    {
        //        prod.UMFamily = context.UMFamilies.Single(x => x.Id == 1);
        //        prod.RecipeUnitMeasure = prod.CostUnitMeasure = context.UnitMeasures.Single(x => x.Id == 1);
        //        prod.RecipeQuantity = 1;
        //    }            

        //    context.Products.AddObject(prod);
        //}

        public override void Remove(object prodId)
        {
            //get product
            var prod = dbSet.Find(prodId);

            //remove product outgredients
            var outgredients = prod.Outgredients.ToList();

            foreach (var item in outgredients)
            {
                context.Ingredients.Remove(item);
            }

            base.Remove(prodId);
        }

        //public void Delete(int prodId)
        //{
        //    //get product
        //    var prod = context.Products.First(x => x.Id == prodId);

        //    //remove product outgredients
        //    var outgredients = prod.Outgredients.ToList();

        //    foreach (var item in outgredients)
        //    {
        //        context.Ingredients.DeleteObject(item);
        //    }

        //    //remove product
        //    context.Products.DeleteObject(prod);
        //}

        //public void Update(Product prod)
        //{
        //    context.Products.Attach(prod);
        //}
    }
}
