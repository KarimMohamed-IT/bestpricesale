namespace bestpricesale.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bestpricesale.Data;
    using bestpricesale.Models;

        public class TemplateService : ITemplateService
        {
            private readonly ITemplateRepository _templateRepository;

            public TemplateService(ITemplateRepository templateRepository)
            {
                _templateRepository = templateRepository;
            }

            public async Task<IEnumerable<Template>> GetAllTemplatesAsync()
            {
                return await _templateRepository.GetAllTemplatesAsync();
            }

            public async Task<Template> GetTemplateByIdAsync(int id)
            {
                return await _templateRepository.GetTemplateByIdAsync(id);
            }
        }
}
