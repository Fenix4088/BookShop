using BookShop.Domain.Abstractions;

namespace BookShop.Domain.Events;

public class BookDeleteEvent(int bookId) : IDomainEvent
{
    public int BookId { get; } = bookId;
}