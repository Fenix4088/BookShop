using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Domain.Repositories;

namespace BookShop.Application.Queries.Handlers;

public class GetAuthorQueryHandler(IAuthorRepository authorRepository) : IQueryHandler<GetAuthorQuery, AuthorModel>
{
    public async Task<AuthorModel?> Handler(GetAuthorQuery query)
    {
        var author = await authorRepository.GetById(query?.Id);
        return author?.ToModel();
    }
}