using System.Threading.Tasks;
using BookShop.Application;
using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Application.Queries;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Repositories;

namespace BookShop.Infrastructure.Handlers;

public class GetBookQueryHandler: IQueryHandler<GetBookQuery, BookModel>
{
    private readonly IBookRepository _bookRepository;

    public GetBookQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookModel> Handler(GetBookQuery query)
    {
        var book = await _bookRepository.GetBookById(query.BookId);

        if (book is null)
        {
            throw new BookNotFoundException(query.BookId);
        }

        return book.ToModel();
    }
}