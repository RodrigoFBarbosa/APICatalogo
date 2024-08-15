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
        // to permitindo que a classe unit fornece acesso aos reps especificos sem ter q ficar criando varias instancias dos repositorios
        // Lazy Loading \/ usada para adiar a obtencao dos objetos ate que eles sejam realmente necessarios
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRep = _productRep ?? new ProductRepository(_context);
                // mais didatico \/
                // if (_productRep == null) 
                //{
                //    _productRep = new ProductRepository(_context);
                //}
                //return _productRep;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRep = _categoryRep ?? new CategoryRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
            // confirmar todas operacoes de modificacao, insercao, etc.
        }

        //liberar recursos associados ao context
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
