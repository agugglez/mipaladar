using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
    }

    public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(RestaurantDBEntities context)
            : base(context)
        {
        }
    }
}
