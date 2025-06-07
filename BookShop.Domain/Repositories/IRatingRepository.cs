using System;
using System.Threading.Tasks;

namespace BookShop.Domain.Repositories;

public interface IRatingRepository<TEntity> : IRepository<TEntity>
where TEntity : class
{
    Task<bool> IsRatingAlreadyExistsAsync(int entityId, Guid userId);
    Task<TEntity> GetByIdAsync(int entityId, Guid userId);
}