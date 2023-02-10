using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intus.Data.Repository
{
    public partial class ApplicationDbRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Properties

        private readonly ApplicationDbContext _context;

        private DbSet<TEntity>? _entity;

        #endregion

        #region Methods

        public ApplicationDbRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(nameof(predicate));

            return await EntityWithNoTracking.AnyAsync(predicate);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await DbSetEntity.FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(nameof(predicate));

            return await DbSetEntity.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(nameof(entity));

            await DbSetEntity.AddAsync(entity);

            await SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {

            ArgumentNullException.ThrowIfNull(nameof(entity));

            DbSetEntity.Update(entity);

            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(nameof(entity));

            DbSetEntity.Remove(entity);

            await SaveChangesAsync();
        }


        private async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region Properties
        public virtual IQueryable<TEntity> Entity => DbSetEntity;
        public virtual IQueryable<TEntity> EntityWithNoTracking => DbSetEntity;

        protected virtual DbSet<TEntity> DbSetEntity
        {
            get
            {
                if (_entity is null)
                    _entity = _context.Set<TEntity>();

                return _entity;
            }
        }
        #endregion
    }
}
