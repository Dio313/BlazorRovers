using Core.Entities;
using Core.Interfaces;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

   
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BlazorRoversContext _dbContext;
        private DbSet<TEntity> _entities;

        public Repository(BlazorRoversContext dbContext) => _dbContext = dbContext;

        public IQueryable<TEntity> Table => Entities;

        public IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        protected DbSet<TEntity> Entities => _entities ??= _dbContext.Set<TEntity>();

       
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await Entities.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Something Wrong");
            }
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Something Wrong");
            }
        }

       
        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Something Wrong");
            }
        }


    }

