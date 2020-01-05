using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IUMRepository : IRepository<UnitMeasure>
    {
        UnitMeasure Unit { get; }
    }

    public class UMRepository : GenericRepository<UnitMeasure>, IUMRepository
    {
        public UMRepository(RestaurantDBEntities context)
            : base(context)
        {
        }

        UnitMeasure unitUM;
        public UnitMeasure Unit
        {
            get
            {
                if (unitUM == null) unitUM = context.UnitMeasures.First(x => x.Id == 1);
                return unitUM;
            }
        }
    }

    public interface IUMFamilyRepository : IRepository<UMFamily>
    {
        UMFamily QuantityFamily { get; }
    }

    public class UMFamilyRepository : GenericRepository<UMFamily>, IUMFamilyRepository
    {
        public UMFamilyRepository(RestaurantDBEntities context)
            : base(context)
        {
        }

        public UMFamily QuantityFamily
        {
            get { return context.UMFamilies.First(x => x.Id == 1); }
        }
    }
}
