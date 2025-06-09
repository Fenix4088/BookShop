using System;
using BookShop.Application.Enums;

namespace BookShop.Web.Models;

public record RatingViewModel(RatingItemType ItemType, int ItemId, int Rating, int CurrentPage);