using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.DTO
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, bool>>? Filter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public IEnumerable<(string PropertyName, bool Ascending)>? OrderBy { get; set; }
        public bool UseIdentityResolution { get; set; } = false;
        public bool IgnoreQueryFilters { get; set; } = false;
        public List<Func<IQueryable<T>, IQueryable<T>>>? Includes { get; set; }

    }
}
