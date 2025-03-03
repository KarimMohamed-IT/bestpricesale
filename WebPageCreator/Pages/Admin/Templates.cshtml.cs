using bestpricesale.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bestpricesale.Pages.Admin
{
    public class TemplatesModel : PageModel
    {
        private readonly ITemplateService _templateService;
        public TemplatesModel(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [BindProperty]
        public string TemplateName { get; set; }
        [BindProperty]
        public string TemplateContent { get; set; }

        public List<string> ExistingTemplates { get; set; } = new List<string>();

        public async Task OnGetAsync()
        {
            var templates = await _templateService.GetAllTemplatesAsync();
            ExistingTemplates = templates.Select(t => t.Name).ToList();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(TemplateName) || string.IsNullOrWhiteSpace(TemplateContent))
            {
                ModelState.AddModelError("", "Both fields are required.");
                await OnGetAsync();
                return Page();
            }

            var templates = await _templateService.GetAllTemplatesAsync();
            bool exists = templates.Any(t => t.Name.Equals(TemplateName, System.StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                ModelState.AddModelError("", "Template already exists. Please change the name or use Update.");
                await OnGetAsync();
                return Page();
            }

            var template = new Template { Name = TemplateName, Content = TemplateContent };
            await _templateService.SaveTemplateAsync(template);
            TempData["Message"] = "Template saved successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (string.IsNullOrWhiteSpace(TemplateName) || string.IsNullOrWhiteSpace(TemplateContent))
            {
                ModelState.AddModelError("", "Both fields are required.");
                await OnGetAsync();
                return Page();
            }

            var template = new Template { Name = TemplateName, Content = TemplateContent };
            await _templateService.SaveTemplateAsync(template);
            TempData["Message"] = "Template updated successfully.";
            return RedirectToPage();
        }
    }
}
