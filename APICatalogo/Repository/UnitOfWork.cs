using APICatalogo.Context;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository? _productRep;
        private ICategoryRepository? _categoryRep;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Lazy Loading 
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRep = _productRep ?? new ProductRepository(_context);
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRep = _categoryRep ?? new CategoryRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        //liberar recursos associados ao context
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
