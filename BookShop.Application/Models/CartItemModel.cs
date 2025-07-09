namespace BookShop.Application.Models;

public sealed class CartItemModel
{
    public Guid Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    
    public string BookAuthorFullname { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal TotalPrice => Price * Quantity;
    
    public bool IsBookDeleted { get; set; }
    
    public bool NotificationShown { get; set; }
}