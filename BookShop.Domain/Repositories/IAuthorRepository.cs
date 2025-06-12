using System.Linq;
using System.Threading.Tasks;
using BookShop.Shared.Enums;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Domain.Repositories;

public interface IAuthorRepository: IRepository<AuthorEntity>
{
    Task<bool> IsUniqueAuthorAsync(string name, string surname);
    Task<AuthorEntity> GetById(int? id);
    IQueryable<AuthorEntity> GetAllQueryable(bool isDeleted = false);

    Task<IPagedResult<AuthorEntity>> GetPagedResultAsync(
        IPagedQuery<AuthorEntity> pagedQuery,
        SortDirection sortDirection = SortDirection.Descending, 
        string searchByNameAndSurname = "", 
        bool isDeleted = false
        );

    void SoftRemove(AuthorEntity authorEntity);
}