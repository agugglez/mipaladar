using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface ITagRepository : IRepository<Tag>
    {
    }

    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(RestaurantDBEntities context)
            : base(context)
        {
        }
    }
}
