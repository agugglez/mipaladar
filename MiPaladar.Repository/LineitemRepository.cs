using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface ILineItemRepository : IRepository<SaleLineItem>
    {
    }

    public class LineItemRepository : GenericRepository<SaleLineItem>, ILineItemRepository
    {
        public LineItemRepository(RestaurantDBEntities context)
            : base(context)
        {
        }
    }
}
