using System.Linq;
using System.Threading.Tasks;
using BookShop.Application;
using BookShop.Application.Abstractions;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Handlers.Abstractions;
using BookShop.Infrastructure.Pagination;
using BookShop.Models.Queries;

namespace BookShop.Infrastructure.Handlers
{
    public class GetAuthorListQueryHandler : IQueryHandler<GetAuthorListQuery, IPagedResult<AuthorModel>>
    {
        private readonly ShopDbContext dbContext;

        public GetAuthorListQueryHandler(ShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IPagedResult<AuthorModel>> Handler(GetAuthorListQuery query)
        {
            var dbQuery = dbContext.Authors.AsQueryable().OrderBy(x => x.Id);
            var pagedResult = await dbQuery.ToPagedResult(query, x => new AuthorModel
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname
            });
            return pagedResult;
        }
    }
}