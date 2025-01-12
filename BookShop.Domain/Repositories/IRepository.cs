using System.Threading.Tasks;

namespace BookShop.Domain.Repositories;

public interface IRepository<TEntity>
{
    Task Add(TEntity customer);

    Task Update(TEntity customer);

    Task Save();
}