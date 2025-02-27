using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using bestpricesale.Models;
using Microsoft.AspNetCore.Hosting;

namespace bestpricesale.Services
{
    public interface ITemplateService
    {
        Task<IEnumerable<Template>> GetAllTemplatesAsync();
        Task SaveTemplateAsync(Template template);
    }

    public class TemplateService : ITemplateService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _templatesFolder;

        public TemplateService(IWebHostEnvironment env)
        {
            _env = env;
            // Use ContentRootPath so templates are saved locally in a folder named "Template"
            _templatesFolder = Path.Combine(_env.ContentRootPath, "Template");
            if (!Directory.Exists(_templatesFolder))
            {
                Directory.CreateDirectory(_templatesFolder);
            }
        }

        public async Task<IEnumerable<Template>> GetAllTemplatesAsync()
        {
            var files = Directory.GetFiles(_templatesFolder, "*.html");
            var templates = files.Select(file => new Template
            {
                Name = Path.GetFileNameWithoutExtension(file),
                Content = File.ReadAllText(file)
            });
            return await Task.FromResult(templates);
        }

        public async Task SaveTemplateAsync(Template template)
        {
            var filePath = Path.Combine(_templatesFolder, $"{template.Name}.html");
            File.WriteAllText(filePath, template.Content);
            await Task.CompletedTask;
        }
    }
}
