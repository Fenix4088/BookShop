using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Infrastructure.Abstractions;

public interface IDataSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);   
}