namespace BookShop.Application.Models;

public sealed class CartModel
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public List<CartItemModel> CartItems { get; set; } = new();
    public decimal TotalPrice => CartItems.Sum(x => x.Price * x.Quantity);
    
    public int TotalQuantity => CartItems.Sum(x => x.Quantity);
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}