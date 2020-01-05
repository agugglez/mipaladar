using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
    }

    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(RestaurantDBEntities context)
            : base(context)
        {
        }
    }
}
