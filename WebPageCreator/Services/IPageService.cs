namespace bestpricesale.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bestpricesale.Models;

        public interface IPageService
        {
            Task<Page> GetPageBySlugAsync(string slug);
            Task<IEnumerable<Page>> GetAllPagesAsync();
            Task CreatePageAsync(Page page);
            Task UpdatePageAsync(Page page);
            Task DeletePageAsync(Page page);
        }
}
