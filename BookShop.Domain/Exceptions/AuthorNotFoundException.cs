namespace BookShop.Domain.Exceptions;

public sealed class AuthorNotFoundException : BookShopException
{
    public AuthorNotFoundException(int authorId) : base($"Author with ID: {authorId} not found")
    {
        
    }
}