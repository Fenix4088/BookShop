using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookShop.Application.Models;

public class BookModel
{
    public int Id { get;  set; }
    public string Title { get;  set; }
    public string Description { get;  set; }
    public DateTime ReleaseDate { get;  set; }
    public int AuthorId { get; set; }
    [ValidateNever]  
    public AuthorModel Author { get; set; }
    public string? CoverImgUrl { get;  set; }
    
    public bool IsDeleted { get; set; }

    public int AverageRating { get; set; }
}