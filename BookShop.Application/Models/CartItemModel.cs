namespace BookShop.Application.Models;

public sealed class CartItemModel
{
    public Guid Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public int Quantity { get; set; }
}