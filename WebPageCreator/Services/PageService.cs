// Updated Page model for database storage
using bestpricesale.Data;
using bestpricesale.Models;
using bestpricesale.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;


// Updated PageService using EF Core
public class PageService : IPageService
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public PageService(ApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task CreatePageAsync(Page page)
    {
        page.Title = page.Title.ToLowerInvariant();
        _context.Pages.Add(page);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePageAsync(Page page)
    {
        var existingPage = await _context.Pages.FindAsync(page.Id);
        if (existingPage != null)
        {
            existingPage.Content = page.Content;
            existingPage.UpdatedAt = DateTime.UtcNow;
            existingPage.Version++;
            await _context.SaveChangesAsync();

            // Clear cache
            _cache.Remove($"page_{existingPage.Title}");
        }
    }

    public async Task DeletePageAsync(string slug)
    {
        var page = await _context.Pages
            .FirstOrDefaultAsync(p => p.Title == slug);

        if (page != null)
        {
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            _cache.Remove($"page_{slug}");
        }
    }

    public async Task<bool> PageExistsAsync(string slug)
    {
        return await _context.Pages
            .AnyAsync(p => p.Title == slug.ToLower());
    }

    public async Task<IEnumerable<Page>> GetAllPagesAsync()
    {
        return await _context.Pages
            .AsNoTracking()
            .OrderBy(p => p.Title)
            .ToListAsync();
    }

    public async Task<Page> GetPageBySlugAsync(string slug)
    {
        return await _cache.GetOrCreateAsync($"page_{slug}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await _context.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Title == slug.ToLower());
        });
    }

    public async Task UpdatePageAsync(string slug, string content)
    {
        var page = await _context.Pages
            .FirstOrDefaultAsync(p => p.Title == slug);

        if (page != null)
        {
            // Save current version before updating
            var version = new PageVersion
            {
                PageId = page.Id,
                Content = page.Content,
                CreatedAt = DateTime.UtcNow,
                AuthorId = page.AuthorId
            };

            _context.PageVersions.Add(version);

            page.Content = content;
            page.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _cache.Remove($"page_{page.Title}");
        }
    }

    public async Task<IEnumerable<PageVersion>> GetPageHistoryAsync(string slug)
    {
        return await _context.PageVersions
            .Where(v => v.Page.Title == slug)
            .OrderByDescending(v => v.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task RestorePageVersionAsync(Guid versionId)
    {
        var version = await _context.PageVersions
            .Include(v => v.Page)
            .FirstOrDefaultAsync(v => v.Id == versionId);

        if (version != null)
        {
            version.Page.Content = version.Content;
            version.Page.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _cache.Remove($"page_{version.Page.Title}");
        }
    }

    public async Task<PageVersion> GetPageVersionAsync(Guid versionId)
    {
        return await _context.PageVersions
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == versionId);
    }
}