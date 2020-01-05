using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IShiftRepository : IRepository<Shift>
    {
    }

    public class ShiftRepository : GenericRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(RestaurantDBEntities context)
            : base(context)
        {
        }
    }
}
