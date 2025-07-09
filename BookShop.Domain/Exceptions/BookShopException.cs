using System;

namespace BookShop.Domain.Exceptions;
public abstract class BookShopException(string message) : Exception(message);