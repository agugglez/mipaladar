using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(RestaurantDBEntities context)
            : base(context)
        {
        }
    }
}
