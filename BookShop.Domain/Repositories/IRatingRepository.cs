using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Domain.Repositories;

public interface IRatingRepository<TEntity> : IRepository<TEntity>
where TEntity : class
{
    Task<TEntity> GetByEntityAndUserIdsAsync(int entityId, Guid userId);
    Task<List<TEntity>> GetAllByEntityIdAsync(int entityId);
}