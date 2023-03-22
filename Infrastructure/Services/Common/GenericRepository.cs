using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces.Common;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;
    private readonly DbSet<T> dbSet;

    protected GenericRepository(AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
        dbSet = context.Set<T>();
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <typeparam name="TM">Dto to map</typeparam>
    /// <returns>List of mapped entities</returns>
    public virtual async Task<IEnumerable<TM>> Find<TM>()
    {
        return await dbSet.AsNoTracking().Where(x => x.DeletedAt == null).Take(10).Skip(0)
            .ProjectTo<TM>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    /// <summary>
    /// Get all entities with filters
    /// </summary>
    /// <param name="predicate">Predicate to search</param>
    /// <typeparam name="TM">Dto to map</typeparam>
    /// <returns>List of mapped entities filtered</returns>
    public virtual async Task<IEnumerable<TM>> Find<TM>(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.AsNoTracking().Where(predicate)
            .ProjectTo<TM>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    /// <summary>
    /// Get one entity by id unmapped
    /// </summary>
    /// <param name="id">Id of the entity</param>
    /// <returns>An unmapped entity</returns>
    public virtual async Task<T> FindOne(Guid id)
    {
        return await dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.DeletedAt == null && x.Id == id);
    }

    /// <summary>
    /// Get one entity by id mapped
    /// </summary>
    /// <param name="id">Id of the entity</param>
    /// <typeparam name="TM">Dto to map</typeparam>
    /// <returns>A mapped entity</returns>
    public virtual async Task<TM> FindOne<TM>(Guid id)
    {
        return await dbSet.AsNoTracking().Where(x => x.DeletedAt == null && x.Id == id)
            .ProjectTo<TM>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get one entity by predicate
    /// </summary>
    /// <param name="predicate">Predicate to search</param>
    /// <typeparam name="TM">Dto to map</typeparam>
    /// <returns>A mapped entity</returns>
    public virtual async Task<TM> FindOne<TM>(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.AsNoTracking().Where(predicate)
            .ProjectTo<TM>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Add one entity to DB
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <returns></returns>
    public virtual async ValueTask Add(T entity) => await dbSet.AddAsync(entity);

    /// <summary>
    /// Add list of entities to DB
    /// </summary>
    /// <param name="entities">List of entities to add</param>
    /// <returns></returns>
    public virtual async ValueTask AddRange(IEnumerable<T> entities) => await dbSet.AddRangeAsync(entities);

    /// <summary>
    /// Update entity in DB
    /// </summary>
    /// <param name="entity">Entity to update</param>
    public virtual void Update(T entity) => dbSet.Update(entity);

    /// <summary>
    /// Update a list of entities in DB
    /// </summary>
    /// <param name="entities">List of entities to update</param>
    public virtual void UpdateRange(IEnumerable<T> entities) => dbSet.UpdateRange(entities);

    /// <summary>
    /// Soft delete entity in DB
    /// </summary>
    /// <param name="id">Id of the entity</param>
    /// <returns>True if entity is deleted or false if entity does not exists</returns>
    public virtual async Task<bool> Delete(Guid id)
    {
        var entity = await FindOne(id);

        if (entity is null) return false;

        entity.DeletedAt = DateTime.Now;
        Update(entity);
        await context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Check if an entity meets the predicate conditions
    /// </summary>
    /// <param name="predicate">Predicate to search</param>
    /// <returns>True if exists or false if does not exists</returns>
    public virtual async Task<bool> Any(Expression<Func<T, bool>> predicate) => await dbSet.AnyAsync(predicate);
}