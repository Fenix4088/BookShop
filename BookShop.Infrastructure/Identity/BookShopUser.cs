using System;
using System.Collections.Generic;
using BookShop.Domain.Entities.Rating;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Infrastructure.Identity;

public class BookShopUser: IdentityUser<Guid>
{
    public ICollection<BookRatingEntity> Ratings { get; private set; } = new List<BookRatingEntity>();
}