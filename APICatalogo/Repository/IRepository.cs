﻿using System.Linq.Expressions;

namespace APICatalogo.Repository;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
    // utilizar métodos que VÃO ser usados em TODAS entidades(principio ISP Solid)
}
