using System.Threading.Tasks;

namespace BookShop.Domain.Repositories;

public interface IAuthorRepository: IRepository<AuthorEntity>
{
    Task<bool> IsUniqueAuthorAsync(string name, string surname);
    Task<AuthorEntity> GetById(int? id);
    void SoftRemove(AuthorEntity authorEntity);
}