using System;
using System.Threading.Tasks;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Repositories;

namespace BookShop.Infrastructure.Services.Domain;

public sealed class BookDomainService : IBookDomainService
{
    
    private readonly IBookRepository bookRepository;
    
    public BookDomainService(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }
    
    public Task<bool> IsUniqueBookAsync(string title, DateTime releaseDate)
    {
        return bookRepository.IsUniqueBookAsync(title, releaseDate);
    }
}