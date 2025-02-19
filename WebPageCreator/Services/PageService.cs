namespace bestpricesale.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bestpricesale.Data;
    using bestpricesale.Models;

        public class PageService : IPageService
        {
            private readonly IPageRepository _pageRepository;

            public PageService(IPageRepository pageRepository)
            {
                _pageRepository = pageRepository;
            }

            public async Task CreatePageAsync(Page page)
            {
                await _pageRepository.AddPageAsync(page);
                await _pageRepository.SaveChangesAsync();
            }

            public async Task DeletePageAsync(Page page)
            {
                await _pageRepository.DeletePageAsync(page);
                await _pageRepository.SaveChangesAsync();
            }

            public async Task<IEnumerable<Page>> GetAllPagesAsync()
            {
                return await _pageRepository.GetAllPagesAsync();
            }

            public async Task<Page> GetPageBySlugAsync(string slug)
            {
                return await _pageRepository.GetPageBySlugAsync(slug);
            }

            public async Task UpdatePageAsync(Page page)
            {
                await _pageRepository.UpdatePageAsync(page);
                await _pageRepository.SaveChangesAsync();
            }
        }
}
