﻿namespace BookShop.Application.Models;

public class AuthorModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public int BookCount { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public int AverageRating { get; set; }

    public string NameAndSurname
    {
        get
        {
            return $"{Surname} {Name}";
        }
    }
}