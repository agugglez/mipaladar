using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Entities;

namespace MiPaladar.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();

        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ITagRepository TagRepository { get; }
        IUMFamilyRepository UMFamilyRepository { get; }
        IUMRepository UMRepository { get; }
        IIngredientRepository IngredientRepository { get; }

        IOrderRepository OrderRepository { get; }
        ILineItemRepository LineItemRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IRoleRepository RoleRepository { get; }
        IShiftRepository ShiftRepository { get; }

        IMiscRepository MiscRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private RestaurantDBEntities context = new RestaurantDBEntities();

        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private ITagRepository tagRepository;
        private IUMFamilyRepository umFamilyRepository;
        private IUMRepository umRepository;
        private IIngredientRepository ingredientRepository;
        private IOrderRepository orderRepository;
        private ILineItemRepository lineItemRepository;
        private IEmployeeRepository employeeRepository;
        private IRoleRepository rolRepository;
        private IShiftRepository shfitRepository;
        private IMiscRepository mscRep;

        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null) productRepository = new ProductRepository(context);
                return productRepository;
            }
        }
        
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (categoryRepository == null) categoryRepository = new CategoryRepository(context);
                return categoryRepository;
            }
        }

        
        public ITagRepository TagRepository
        {
            get
            {
                if (tagRepository == null) tagRepository = new TagRepository(context);
                return tagRepository;
            }
        }
        
        public IUMFamilyRepository UMFamilyRepository
        {
            get
            {
                if (umFamilyRepository == null)
                    umFamilyRepository = new UMFamilyRepository(context);
                return umFamilyRepository;
            }
        }

        
        public IUMRepository UMRepository
        {
            get
            {
                if (umRepository == null)
                    umRepository = new UMRepository(context);
                return umRepository;
            }
        }

        
        public IIngredientRepository IngredientRepository
        {
            get
            {
                if (ingredientRepository == null) ingredientRepository = new IngredientRepository(context);
                return ingredientRepository;
            }
        }
                
        public IOrderRepository OrderRepository
        {
            get
            {
                if (orderRepository == null) orderRepository = new OrderRepository(context);
                return orderRepository;
            }
        }

        
        public ILineItemRepository LineItemRepository
        {
            get
            {
                if (lineItemRepository == null) lineItemRepository = new LineItemRepository(context);
                return lineItemRepository;
            }
        }
                
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (employeeRepository == null) employeeRepository = new EmployeeRepository(context);
                return employeeRepository;
            }
        }
                
        public IRoleRepository RoleRepository
        {
            get
            {
                if (rolRepository == null) rolRepository = new RoleRepository(context);
                return rolRepository;
            }
        }
                
        public IShiftRepository ShiftRepository
        {
            get
            {
                if (shfitRepository == null) shfitRepository = new ShiftRepository(context);
                return shfitRepository;
            }
        }
        
        public IMiscRepository MiscRepository
        {
            get
            {
                if (mscRep == null) mscRep = new MiscRepository(context);
                return mscRep;
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
