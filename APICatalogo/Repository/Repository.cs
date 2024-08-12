using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repository;

public class Repository<T> : IRepository<T> where T : class // garanto que o tipo T vai ser uma class
{
    protected readonly AppDbContext _context; // protected permite que seja usada pelas classes que herdarem != de private

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking().ToList(); // set acessa uma colecao do tipo T 
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
       return _context.Set<T>().FirstOrDefault(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        //_context.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        //_context.SaveChanges();
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        //_context.SaveChanges();
        return entity;
    }
}
