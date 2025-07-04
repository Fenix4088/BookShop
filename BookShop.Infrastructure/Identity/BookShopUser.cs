using System;
using System.Collections.Generic;
using BookShop.Domain.Entities.Cart;
using BookShop.Domain.Entities.Rating;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Infrastructure.Identity;

public class BookShopUser: IdentityUser<Guid>
{
    public ICollection<BookRatingEntity> Ratings { get; private set; } = new List<BookRatingEntity>();
    
    public ICollection<AuthorRatingEntity> RatingsAuthor { get; private set; } = new List<AuthorRatingEntity>();
    
    public CartEntity Cart { get; private set; }
}