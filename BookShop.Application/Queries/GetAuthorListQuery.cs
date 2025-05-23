using BookShop.Application.Abstractions;
using BookShop.Application.Enums;
using BookShop.Application.Models;

namespace BookShop.Application.Queries;

public class GetAuthorListQuery : IAuthorPageQuery
{
    public GetAuthorListQuery(int currentPage, int rowCount, SortDirection sortDirection = SortDirection.Descending, string searchByNameAndSurname = "", bool isDeleted = false)
    {
        CurrentPage = currentPage;
        RowCount = rowCount;
        SortDirection = sortDirection;
        SearchByNameAndSurname = searchByNameAndSurname;
        IsDeleted = isDeleted;
    }
    public string SearchByNameAndSurname { get; set; }

    public int CurrentPage { get; private set; }

    public int RowCount { get; private set; }
        
    public SortDirection SortDirection { get; set; }
        
    public bool IsDeleted { get; set; }
}