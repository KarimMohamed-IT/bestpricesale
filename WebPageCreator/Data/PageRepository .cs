namespace bestpricesale.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bestpricesale.Models;
    using Microsoft.EntityFrameworkCore;
    using bestpricesale.Data;

    // ----- Page Repository -----
    public interface IPageRepository
    {
        Task<Page> GetPageBySlugAsync(string slug);
        Task<IEnumerable<Page>> GetAllPagesAsync();
        Task AddPageAsync(Page page);
        Task UpdatePageAsync(Page page);
        Task DeletePageAsync(Page page);
        Task SaveChangesAsync();
    }

    public class PageRepository : IPageRepository
    {
        private readonly ApplicationDbContext _context;

        public PageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Page> GetPageBySlugAsync(string slug)
        {
            return await _context.Pages
                .Include(p => p.Template)
                .Include(p => p.EventDetail)
                    .ThenInclude(e => e.EventOptions)
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<IEnumerable<Page>> GetAllPagesAsync()
        {
            return await _context.Pages
                .Include(p => p.Template)
                .ToListAsync();
        }

        public async Task AddPageAsync(Page page)
        {
            await _context.Pages.AddAsync(page);
        }

        public async Task UpdatePageAsync(Page page)
        {
            _context.Pages.Update(page);
        }

        public async Task DeletePageAsync(Page page)
        {
            _context.Pages.Remove(page);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    // ----- Template Repository -----
    public interface ITemplateRepository
    {
        Task<Template> GetTemplateByIdAsync(int id);
        Task<IEnumerable<Template>> GetAllTemplatesAsync();
    }

    public class TemplateRepository : ITemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Template> GetTemplateByIdAsync(int id)
        {
            return await _context.Templates.FindAsync(id);
        }

        public async Task<IEnumerable<Template>> GetAllTemplatesAsync()
        {
            return await _context.Templates.ToListAsync();
        }
    }
}
