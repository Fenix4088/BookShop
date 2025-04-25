using System.Collections.Generic;
using BookShop.Application.Models;

namespace BookShop.Web.Models;

public class BookDetailsViewModel
{
    public BookModel                Book    { get; set; }
    public IEnumerable<AuthorModel> Authors { get; set; }
}