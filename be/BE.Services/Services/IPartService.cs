using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.DAL.DTO;
using BE.DAL.Models;

namespace BE.Services.Services
{
    public interface IPartService
    {
        Task<PagedResult<Part>> GetAllPartsAsync();
        Task<Part> GetPartByIdAsync(Guid id);
        Task<Part> CreatePartAsync(PartDTO partDto);
        Task<Part> UpdatePartAsync(PartDTO partDto);
        Task<bool> DeletePartAsync(Guid id);
    }
}
