using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IMiscRepository : IRepository<Misc>
    {
    }

    public class MiscRepository : GenericRepository<Misc>, IMiscRepository
    {
        public MiscRepository(RestaurantDBEntities context)
            : base(context)
        {
        }
    }
}
