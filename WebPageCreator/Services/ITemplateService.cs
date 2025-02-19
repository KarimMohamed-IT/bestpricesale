namespace bestpricesale.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bestpricesale.Models;
        public interface ITemplateService
        {
            Task<Template> GetTemplateByIdAsync(int id);
            Task<IEnumerable<Template>> GetAllTemplatesAsync();
        }
}
