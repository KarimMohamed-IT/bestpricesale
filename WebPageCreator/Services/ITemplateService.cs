using System.Collections.Generic;
using System.Threading.Tasks;
using bestpricesale.Models;

namespace bestpricesale.Services
{
    public interface ITemplateService
    {
        Task<IEnumerable<Template>> GetAllTemplatesAsync();
        Task SaveTemplateAsync(Template template);
        Task UpdateTemplateAsync(Template template);
        Task DeleteTemplateAsync(string name);
        Task<Template> GetTemplateByNameAsync(string name);
    }
}