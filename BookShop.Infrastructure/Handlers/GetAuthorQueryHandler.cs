using System.Threading.Tasks;
using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Domain.Repositories;

namespace BookShop.Infrastructure.Handlers;

public class GetAuthorQueryHandler: IQueryHandler<GetAuthorQuery, AuthorModel>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorModel?> Handler(GetAuthorQuery query)
    {
        var author = await _authorRepository.GetById(query?.Id);
        return author?.AsModel();
    }
}