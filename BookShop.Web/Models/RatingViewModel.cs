using System;
using BookShop.Shared.Enums;

namespace BookShop.Web.Models;

public record RatingViewModel(RatingItemType ItemType, int ItemId, int Rating, int CurrentPage);