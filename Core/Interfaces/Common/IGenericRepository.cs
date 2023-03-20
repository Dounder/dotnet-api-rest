using System.Linq.Expressions;

namespace Core.Interfaces.Common;

public interface IGenericRepository<T> where T : class, IBaseEntity
{
    Task<IEnumerable<TM>> Find<TM>();
    Task<IEnumerable<TM>> Find<TM>(Expression<Func<T, bool>> predicate);


    Task<T?> FindOne(Guid id);

    Task<TM?> FindOne<TM>(Guid id);

    Task<TM?> FindOne<TM>(Expression<Func<T, bool>> predicate);


    ValueTask Add(T entity);

    ValueTask AddRange(IEnumerable<T> entities);


    void Update(T entity);

    void UpdateRange(IEnumerable<T> entities);
    
    Task<bool> Delete(Guid id);

    Task<bool> Any(Expression<Func<T, bool>> predicate);
}