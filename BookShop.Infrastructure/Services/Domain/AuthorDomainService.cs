using System.Threading.Tasks;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Repositories;

namespace BookShop.Infrastructure.Services.Domain;

public sealed class AuthorDomainService : IAuthorDomainService
{
    private readonly IAuthorRepository authorRepository;
    
    public AuthorDomainService(IAuthorRepository authorRepository)
    {
        this.authorRepository = authorRepository;
    }
    
    public Task<bool> IsUniqueAuthorAsync(string name, string surname)
    {
        return authorRepository.IsUniqueAuthorAsync(name, surname);
    }
}