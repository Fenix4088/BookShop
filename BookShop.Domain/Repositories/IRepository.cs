using System.Threading.Tasks;

namespace BookShop.Domain.Repositories;

public interface IRepository<TEntity>
{
    Task AddAsync(TEntity customer);

    Task UpdateAsync(TEntity customer);

    Task SaveAsync();
}