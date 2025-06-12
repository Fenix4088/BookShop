using BookShop.Domain.Entities;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Application.Abstractions;

public interface IBookPageQuery : IPagedQuery<BookEntity>
{
    string SearchByBookTitle { get; }
}