using System;
using System.Threading.Tasks;

namespace BookShop.Domain.Abstractions;

public interface IBookDomainService
{
    public Task<bool> IsUniqueBookAsync(string title, DateTime releaseDate);
    public Task<bool> IsUniqueBookAsync(int targetBookId, string title, DateTime releaseDate);
}