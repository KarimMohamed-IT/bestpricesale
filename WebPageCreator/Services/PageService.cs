using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using bestpricesale.Models;

namespace bestpricesale.Services
{
    public class PageService : IPageService
    {
        private readonly string _pagesFolder;

        public PageService()
        {
            // Set the folder to store pages as HTML files.
            _pagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Pages");
            if (!Directory.Exists(_pagesFolder))
            {
                Directory.CreateDirectory(_pagesFolder);
            }
        }

        // Save the page's HTML content using the slug as filename.
        public async Task CreatePageAsync(Page page)
        {
            string filePath = Path.Combine(_pagesFolder, $"{page.Slug}.html");
            await File.WriteAllTextAsync(filePath, page.Content);
        }

        // Update an existing page.
        public async Task UpdatePageAsync(Page page)
        {
            string filePath = Path.Combine(_pagesFolder, $"{page.Slug}.html");
            await File.WriteAllTextAsync(filePath, page.Content);
        }

        // Delete a page by slug.
        public async Task DeletePageAsync(string slug)
        {
            string filePath = Path.Combine(_pagesFolder, $"{slug}.html");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            await Task.CompletedTask;
        }

        // Check if a page exists (by slug).
        public async Task<bool> PageExistsAsync(string slug)
        {
            string filePath = Path.Combine(_pagesFolder, $"{slug}.html");
            return await Task.FromResult(File.Exists(filePath));
        }

        // Load all pages.
        public async Task<IEnumerable<Page>> GetAllPagesAsync()
        {
            var pages = new List<Page>();
            var files = Directory.GetFiles(_pagesFolder, "*.html");
            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                var slug = Path.GetFileNameWithoutExtension(file);
                pages.Add(new Page { Slug = slug, Content = content });
            }
            return pages;
        }

        // Load a page by its slug.
        public async Task<Page> GetPageBySlugAsync(string slug)
        {
            string filePath = Path.Combine(_pagesFolder, $"{slug}.html");
            if (!File.Exists(filePath))
                return null;
            var content = await File.ReadAllTextAsync(filePath);
            return new Page { Slug = slug, Content = content };
        }
    }
}
