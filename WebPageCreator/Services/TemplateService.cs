using System.Collections.Generic;
using System.Threading.Tasks;
using bestpricesale.Data;
using bestpricesale.Models;
using Microsoft.EntityFrameworkCore;

namespace bestpricesale.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ApplicationDbContext _context;

        public TemplateService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Template>> GetAllTemplatesAsync()
        {
            return await _context.Templates
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveTemplateAsync(Template template)
        {
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTemplateAsync(Template template)
        {
            _context.Templates.Update(template);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTemplateAsync(string name)
        {
            var template = await _context.Templates
                .FirstOrDefaultAsync(t => t.Name == name);

            if (template != null)
            {
                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Template> GetTemplateByNameAsync(string name)
        {
            return await _context.Templates
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}