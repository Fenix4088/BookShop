using BookShop.Domain.Abstractions;
using BookShop.Domain.Events;
using BookShop.Domain.Repositories;

namespace BookShop.Application.EventHandlers;

public class BookDeleteEventHandler(ICartRepository cartRepository) : IDomainEventHandler<BookDeleteEvent>
{
    public async Task HandleAsync(BookDeleteEvent ev, CancellationToken ct = default)
    {
         await cartRepository.MarkBookAsDeletedAsync(ev.BookId);
    }
}