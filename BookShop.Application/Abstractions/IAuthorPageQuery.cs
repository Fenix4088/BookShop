using BookShop.Domain;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Application.Abstractions;

public interface IAuthorPageQuery : IPagedQuery<AuthorEntity>
{
    string SearchByNameAndSurname { get; }
}