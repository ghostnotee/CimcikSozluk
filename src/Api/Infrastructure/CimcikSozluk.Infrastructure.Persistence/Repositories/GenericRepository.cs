using System.Linq.Expressions;
using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Api.Domain.Models;
using CimcikSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CimcikSozluk.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;

    protected DbSet<TEntity> _entity => _context.Set<TEntity>();

    public GenericRepository(DbContext dbContext, CimcikSozlukContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(dbContext));
    }

    #region Insert Methods

    public virtual async Task<int> AddAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities)
    {
        await _entity.AddRangeAsync(entities);
        return await _context.SaveChangesAsync();
    }

    public virtual int Add(TEntity entity)
    {
        _entity.Add(entity);
        return _context.SaveChanges();
    }

    public virtual int Add(IEnumerable<TEntity> entities)
    {
        if (entities is not null && !entities.Any())
            return 0;
        _entity.AddRange(entities);

        return _context.SaveChanges();
    }

    #endregion

    #region Update Methods

    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        _entity.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public int Update(TEntity entity)
    {
        _entity.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return _context.SaveChanges();
    }

    #endregion

    #region Delete Methods

    public virtual Task<int> DeleteAsync(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _entity.Attach(entity);
        _entity.Remove(entity);
        return _context.SaveChangesAsync();
    }

    public virtual Task<int> DeleteAsync(Guid id)
    {
        var entity = _entity.Find(id);
        return DeleteAsync(entity);
    }

    public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
    {
        _context.RemoveRange(predicate);
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual int Delete(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _entity.Attach(entity);
        return _context.SaveChanges();
    }

    public virtual int Delete(Guid id)
    {
        var entity = _entity.Find(id);
        return Delete(entity);
    }

    public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
    {
        _context.RemoveRange(_entity.Where(predicate));
        return _context.SaveChanges() > 0;
    }

    #endregion

    #region Add Or Update Methods

    public virtual Task<int> AddOrUpdateAsync(TEntity entity)
    {
        if (!_entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            _context.Update(entity);
        return _context.SaveChangesAsync();
    }

    public virtual int AddOrUpdate(TEntity entity)
    {
        if (!_entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            _context.SaveChanges();
        return _context.SaveChanges();
    }

    #endregion

    #region Get Methods

    public virtual IQueryable<TEntity> AsQueryable() => _entity.AsQueryable();

    public virtual async Task<List<TEntity>> GetAll(bool noTracking = true)
    {
        if (noTracking)
            return await _entity.AsNoTracking().ToListAsync();
        return await _entity.ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _entity;

        if (predicate is not null)
            query = query.Where(predicate);

        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            query = query.Include(include);
        }

        if (orderBy is not null)
            query = orderBy(query);

        if (noTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        TEntity found = await _entity.FindAsync();
        if (found is null)
            return null;
        if (noTracking)
            _context.Entry(found).State = EntityState.Detached;
        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            _context.Entry(found).Reference(include).Load();
        }

        return found;
    }

    public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _entity;
        if (predicate is not null)
            query = query.Where(predicate);

        query = ApplyIncludes(query, includes);

        if (noTracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync();
    }

    public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return Get(predicate, noTracking, includes).FirstOrDefaultAsync();
    }

    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _entity.AsQueryable();
        if (predicate is not null)
            query = query.Where(predicate);

        query = ApplyIncludes(query, includes);

        if (noTracking)
            query = query.AsNoTracking();

        return query;
    }

    #endregion

    #region Bulk Methods

    public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
    {
        if (ids is not null && !ids.Any())
            return Task.CompletedTask;

        _context.RemoveRange(_entity.Where(i => ids.Contains(i.Id)));

        return _context.SaveChangesAsync();
    }

    public virtual Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
    {
        _context.RemoveRange(_entity.Where(predicate));
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkDelete(IEnumerable<TEntity> entities)
    {
        if (entities is not null && !entities.Any())
            return Task.CompletedTask;
        _entity.RemoveRange(entities);
        return _context.SaveChangesAsync();
    }

    public virtual Task BulkUpdate(IEnumerable<TEntity> entities)
    {
        if (entities is not null && !entities.Any())
            return Task.CompletedTask;

        foreach (var entityItem in entities)
        {
            _entity.Update(entityItem);
        }

        return _context.SaveChangesAsync();
    }

    public virtual async Task BulkAdd(IEnumerable<TEntity> entities)
    {
        if (entities is not null && !entities.Any())
            await Task.CompletedTask;
        await _entity.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region SaveChanges Methods

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    #endregion

    private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query,
        params Expression<Func<TEntity, object>>[] includes)
    {
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }
}