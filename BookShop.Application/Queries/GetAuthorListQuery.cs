using BookShop.Application.Enums;
using BookShop.Application.Models;
using BookShop.Models.Queries.Abstractions;

namespace BookShop.Models.Queries
{
    public class GetAuthorListQuery : IPagedQuery<AuthorModel>
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
}