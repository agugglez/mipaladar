using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        List<Employee> GetWaiters();
    }

    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RestaurantDBEntities context)
            : base(context)
        {
        }

        public List<Employee> GetWaiters()
        {
            return context.Employees.Where(x => x.CanSell).ToList();
        }
    }
}
