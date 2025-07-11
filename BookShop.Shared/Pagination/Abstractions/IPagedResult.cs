﻿using BookShop.Shared.Enums;

namespace BookShop.Shared.Pagination.Abstractions;

public interface IPagedResult<TResult>
{
    IEnumerable<TResult> Items { get; }
    int CurrentPage { get; }
    int PageCount { get; }
    int PageSize { get; }
    int TotalRowCount { get; }
        
    SortDirection SortDirection { get; }
        
    string SearchByNameAndSurname { get; }
        
    string SearchByBookTitle { get; }
    string SearchByAuthorName { get; }
    
    bool IsDeleted { get; }
}