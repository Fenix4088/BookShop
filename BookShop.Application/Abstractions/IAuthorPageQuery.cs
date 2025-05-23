using BookShop.Application.Models;

namespace BookShop.Application.Abstractions;

public interface IAuthorPageQuery : IPagedQuery<AuthorModel>
{
    string SearchByNameAndSurname { get; }
}