using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using bestpricesale.Models;
using bestpricesale.Services;

namespace bestpricesale.Pages.Admin
{
    public class PageEditorModel : PageModel
    {
        private readonly IPageService _pageService;
        private readonly ITemplateService _templateService;

        public PageEditorModel(IPageService pageService, ITemplateService templateService)
        {
            _pageService = pageService;
            _templateService = templateService;
        }

        [BindProperty]
        public Models.Page Page { get; set; }

        public List<SelectListItem> TemplateOptions { get; set; }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                // Load the page for editing (if implemented)
                // For now, we assume a new page creation if no ID is provided.
                // Page = await _pageService.GetPageByIdAsync(id.Value);
                Page = new Models.Page();
            }
            else
            {
                Page = new Models.Page();
            }

            // Populate the template dropdown
            var templates = await _templateService.GetAllTemplatesAsync();
            TemplateOptions = new List<SelectListItem>();
            foreach (var template in templates)
            {
                TemplateOptions.Add(new SelectListItem { Value = template.Id.ToString(), Text = template.Name });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // If model binding fails, redisplay the page.
                return Page();
            }

            // Create new page or update existing one based on Page.Id
            if (Page.Id == 0)
            {
                await _pageService.CreatePageAsync(Page);
            }
            else
            {
                await _pageService.UpdatePageAsync(Page);
            }

            // Redirect to the home/index page after saving
            return RedirectToPage("/Index");
        }
    }
}
