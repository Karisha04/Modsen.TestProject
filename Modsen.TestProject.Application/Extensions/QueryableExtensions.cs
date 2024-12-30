using Modsen.TestProject.Application.Models;

namespace Modsen.TestProject.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationParams paginationParams)
        {
            return query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                        .Take(paginationParams.PageSize);
        }
    }

}
