using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using bestpricesale.Models;
using bestpricesale.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bestpricesale.Pages
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

        // Dropdown options for templates.
        public List<SelectListItem> TemplateOptions { get; set; } = new List<SelectListItem>();

        // On GET: load page by slug if provided, otherwise create a new one.
        public async Task OnGetAsync(string slug)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                Page = await _pageService.GetPageBySlugAsync(slug);
                if (Page == null)
                {
                    // Optionally, create an empty page if not found.
                    Page = new Models.Page { Slug = slug };
                }
            }
            else
            {
                Page = new Models.Page();
            }

            var templates = await _templateService.GetAllTemplatesAsync();
            if (templates != null && templates.Any())
            {
                TemplateOptions = templates.Select(t => new SelectListItem
                {
                    Value = t.Name,
                    Text = t.Name
                }).ToList();
            }
        }

        // Handler to load a template's content (returns HTML directly).
        public async Task<IActionResult> OnGetLoadTemplateAsync(string name)
        {
            var templates = await _templateService.GetAllTemplatesAsync();
            var template = templates.FirstOrDefault(t => t.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
            if (template == null)
            {
                return NotFound();
            }
            return Content(template.Content, "text/html");
        }

        // Handler to check if a slug exists, returning an HTML snippet.
        public async Task<IActionResult> OnGetCheckSlugAsync(string slug)
        {
            if (await _pageService.PageExistsAsync(slug))
            {
                return Content($"<div style='color:red;'>The slug '{slug}' already exists. Please choose a different one.</div>", "text/html");
            }
            else
            {
                return Content($"<div style='color:green;'>The slug '{slug}' is available.</div>", "text/html");
            }
        }

        // On POST: if a page with the slug exists, update it; otherwise, create a new page.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if a page with this slug already exists.
            var existing = await _pageService.GetPageBySlugAsync(Page.Slug);
            if (existing == null)
            {
                await _pageService.CreatePageAsync(Page);
            }
            else
            {
                await _pageService.UpdatePageAsync(Page);
            }

            return RedirectToPage("/Index");
        }
    }
}
