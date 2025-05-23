using BookShop.Application.Models;
using BookShop.Models.Queries.Abstractions;

namespace BookShop.Application.Abstractions;

public interface IBookPageQuery : IPagedQuery<BookModel>
{
    string SearchByBookTitle { get; }
}