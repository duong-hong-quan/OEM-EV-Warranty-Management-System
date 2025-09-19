using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.DTO
{
    public class PagedResult<T>
    {
        /// <summary>
        /// Danh sách kết quả sau khi phân trang.
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Tổng số bản ghi trong toàn bộ kết quả (chưa phân trang).
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Tổng số trang, tính dựa trên PageSize.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Số trang hiện tại.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Số lượng phần tử trên mỗi trang.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Có phải trang đầu tiên không.
        /// </summary>
        public bool IsFirstPage => CurrentPage <= 1;

        /// <summary>
        /// Có phải trang cuối cùng không.
        /// </summary>
        public bool IsLastPage => CurrentPage >= TotalPages;
    }
}
