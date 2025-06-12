using System.Threading.Tasks;

namespace BookShop.Domain.Abstractions;

public interface IAuthorDomainService
{
    Task<bool> IsUniqueAuthorAsync(string name, string surname);
}