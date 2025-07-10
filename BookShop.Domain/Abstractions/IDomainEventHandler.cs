using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Domain.Abstractions;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task HandleAsync(T ev, CancellationToken ct = default);
}