using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using bestpricesale.Models;
using bestpricesale.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bestpricesale.Pages
{
    public class PageEditorModel : PageModel
    {
        private readonly IPageService _pageService;
        private readonly ITemplateService _templateService;
        private readonly UserManager<IdentityUser> _userManager;

        public PageEditorModel(
            IPageService pageService,
            ITemplateService templateService,
            UserManager<IdentityUser> userManager)
        {
            _pageService = pageService;
            _templateService = templateService;
            this._userManager = userManager;
            //var user = await _userManager.GetUserAsync(User);
            //Page.AuthorId = user?.Id ?? throw new UnauthorizedAccessException();
        }

        [BindProperty]
        public Models.Page Page { get; set; }

        public List<SelectListItem> TemplateOptions { get; set; } = new();
        public List<SelectListItem> VersionOptions { get; set; } = new();

        public async Task OnGetAsync(string slug)
        {
            await LoadPageData(slug);
            await LoadTemplates();
        }

        private async Task LoadPageData(string slug)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                Page = await _pageService.GetPageBySlugAsync(slug);
                await LoadVersionHistory(slug);
            }
            else
            {
                Page = new Models.Page { Title = slug };
            }
        }

        private async Task LoadVersionHistory(string slug)
        {
            var versions = await _pageService.GetPageHistoryAsync(slug);
            VersionOptions = versions.OrderByDescending(v => v.CreatedAt)
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.CreatedAt:g} (v{v.VersionNumber})"
                })
                .ToList();
        }

        private async Task LoadTemplates()
        {
            var templates = await _templateService.GetAllTemplatesAsync();
            TemplateOptions = templates?
                .Select(t => new SelectListItem
                {
                    Value = t.Name,
                    Text = t.Name
                })
                .ToList() ?? new List<SelectListItem>();
        }

        public async Task<IActionResult> OnGetLoadTemplateAsync(string name)
        {
            var template = await _templateService.GetTemplateByNameAsync(name);
            return template != null
                ? Content(template.Content, "text/html")
                : NotFound();
        }

        public async Task<IActionResult> OnGetLoadVersionAsync(Guid versionId)
        {
            var version = await _pageService.GetPageVersionAsync(versionId);
            return version != null
                ? Content(version.Content, "text/html")
                : NotFound();
        }

        public async Task<IActionResult> OnGetCheckSlugAsync(string slug)
        {
            var exists = await _pageService.PageExistsAsync(slug);
            var message = exists
                ? $"<div class='text-danger'>Slug '{slug}' is taken</div>"
                : $"<div class='text-success'>Slug '{slug}' is available</div>";

            return Content(message, "text/html");
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                await LoadTemplates();
                return Page();
            }

            if (await _pageService.PageExistsAsync(Page.Title))
            {
                await _pageService.UpdatePageAsync(Page.Title, Page.Content);
            }
            else
            {
                await _pageService.CreatePageAsync(Page);
            }

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostRestoreVersionAsync(Guid versionId)
        {
            await _pageService.RestorePageVersionAsync(versionId);
            return RedirectToPage(new { slug = Page.Title });
        }
    }
}