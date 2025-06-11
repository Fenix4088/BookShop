using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Domain.Repositories;

public interface IAuthorRepository: IRepository<AuthorEntity>
{
    Task<bool> IsUniqueAuthorAsync(string name, string surname);
    Task<AuthorEntity> GetById(int? id);
    IQueryable<AuthorEntity> GetAllQueryable(bool isDeleted = false);
    void SoftRemove(AuthorEntity authorEntity);
}