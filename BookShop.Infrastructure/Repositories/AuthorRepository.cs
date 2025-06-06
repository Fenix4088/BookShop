﻿using System.Linq;
using System.Threading.Tasks;
using BookShop.Domain;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repositories;

public class AuthorRepository : GenericRepository<AuthorEntity, ShopDbContext>, IAuthorRepository
{
    public AuthorRepository(ShopDbContext shopDbContext) : base(shopDbContext)
    {
    }

    public async Task<bool> IsUniqueAuthorAsync(string name, string surname)
    {
        return !(await context.Authors.Where(x => x.DeletedAt == null).AnyAsync(author =>
            author.Name == name && author.Surname == surname));
    }

    public async Task<AuthorEntity> GetById(int? id)
    {
        return await context.Authors.SingleOrDefaultAsync(x => x.Id == id);
    }

    public void SoftRemove(AuthorEntity authorEntity)
    { 
        authorEntity.Delete();
        context.Authors.Update(authorEntity);
    }
}