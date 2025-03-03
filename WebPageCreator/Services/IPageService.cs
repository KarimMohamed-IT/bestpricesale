using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bestpricesale.Models;

namespace bestpricesale.Services
{
    public interface IPageService
    {
        Task CreatePageAsync(Page page);
        Task UpdatePageAsync(string slug, string content);
        Task DeletePageAsync(string slug);
        Task<bool> PageExistsAsync(string slug);
        Task<IEnumerable<Page>> GetAllPagesAsync();
        Task<Page> GetPageBySlugAsync(string slug);
        Task<IEnumerable<PageVersion>> GetPageHistoryAsync(string slug);
        Task RestorePageVersionAsync(Guid versionId);
        // Add to IPageService
        Task<PageVersion> GetPageVersionAsync(Guid versionId);
    }
}