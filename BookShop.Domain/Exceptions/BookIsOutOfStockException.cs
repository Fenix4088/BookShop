namespace BookShop.Domain.Exceptions;

public class BookIsOutOfStockException(int id, string title) : BookShopException($"The book with ID {id} and title '{title}' is out of stock.", true);