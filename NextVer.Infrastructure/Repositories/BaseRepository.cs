using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NextVer.Infrastructure.Interfaces;
using NextVer.Infrastructure.Persistance;
using System.Linq.Expressions;


namespace NextVer.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntityWithLinkIds
    {
        private readonly NextVerDbContext _context;
        private readonly IMapper _mapper;

        public BaseRepository(NextVerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Add(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await GetById(id);
            if (entity == null)
                return false;

            try
            {
                _context.Set<T>().Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> Edit<TKey, TDto>(TKey id, TDto dto)
        {
            try
            {
                if (typeof(TKey) == typeof(int))
                {
                    var intId = Convert.ToInt32(id);
                    var existingEntity = await GetById(intId);
                    if (existingEntity == null)
                    {
                        return false;
                    }

                    if (existingEntity.GetType().GetProperty("UpdatedAt") != null)
                    {
                        var updatedAtProperty = existingEntity.GetType().GetProperty("UpdatedAt");
                        if (updatedAtProperty != null)
                        {
                            updatedAtProperty.SetValue(existingEntity, DateTime.UtcNow);
                        }
                    }

                    _mapper.Map(dto, existingEntity);

                    return await _context.SaveChangesAsync() > 0;
                }
                else
                {
                    throw new NotSupportedException($"Editing entities with {typeof(TKey).Name} identifier is not supported.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var query = _context.Set<T>().AsQueryable();

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query.FirstOrDefaultAsync(entity => entity.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public IQueryable<TLinkEntity> GetLinkEntitiesFor<TEntity, TLinkEntity>(
            Expression<Func<TLinkEntity, bool>> filterExpression
        )
            where TEntity : class
            where TLinkEntity : class, new()
        {
            return _context.Set<TLinkEntity>()
                .Where(filterExpression);
        }
        public void AddLinkEntity<TLinkEntity>(TLinkEntity entity) where TLinkEntity : class
        {
            _context.Set<TLinkEntity>().Add(entity);
        }

        public void RemoveLinkEntity<TLinkEntity>(TLinkEntity entity) where TLinkEntity : class
        {
            _context.Set<TLinkEntity>().Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<int> GetNumberOfEntities<T>() where T : class
        {
            try
            {
                return await _context.Set<T>().CountAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<IEnumerable<TEntity>> GetEntitiesBy<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class
        {
            try
            {
                return await _context.Set<TEntity>()
                    .Where(filterExpression)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}