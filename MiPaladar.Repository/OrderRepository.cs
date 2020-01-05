using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;
using System.Linq.Expressions;

namespace MiPaladar.Repository
{
    public interface IOrderRepository : IRepository<Sale>
    {
        DateTime GetLastSaleDate();
        List<Sale> QuerySalesByWorkDate(DateTime fromDate, DateTime toDate);
    }

    public class OrderRepository : GenericRepository<Sale>, IOrderRepository
    {
        public OrderRepository(RestaurantDBEntities context)
            : base(context)
        {
        }

        public DateTime GetLastSaleDate()
        {
            DateTime result = DateTime.Today;

            if (context.Orders.OfType<Sale>().Count() > 0)
            {
                result = context.Orders.OfType<Sale>().Max(x => x.Date);
            }

            return result;
        }

        public List<Sale> QuerySalesByWorkDate(DateTime fromDate, DateTime toDate)
        {
            DateTime toDatePlusOne = toDate.Date.AddDays(1);

            var query = from o in context.Orders.OfType<Sale>()
                        where o.Date >= fromDate && o.Date < toDatePlusOne
                        select o;

            return query.ToList();
        }
    }
}
