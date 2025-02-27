using System.Collections.Generic;
using System.Threading.Tasks;
using bestpricesale.Models;

namespace bestpricesale.Services
{
    public interface IPageService
    {
        Task<Page> GetPageBySlugAsync(string slug);
        Task<IEnumerable<Page>> GetAllPagesAsync();
        Task CreatePageAsync(Page page);
        Task UpdatePageAsync(Page page);
        Task DeletePageAsync(string slug);
        Task<bool> PageExistsAsync(string slug);
    }
}
