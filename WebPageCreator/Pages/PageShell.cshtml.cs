namespace bestpricesale.Pages
{
    using bestpricesale.Models;
    using bestpricesale.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Threading.Tasks;

    public class PageShellModel : PageModel
        {
            private readonly IPageService _pageService;

            public PageShellModel(IPageService pageService)
            {
                _pageService = pageService;
            }

            public Models.Page Page { get; set; }

            public async Task<IActionResult> OnGetAsync(string slug)
            {
                if (string.IsNullOrEmpty(slug))
                {
                    return NotFound();
                }

                Page = await _pageService.GetPageBySlugAsync(slug);
                if (Page == null)
                {
                    return NotFound();
                }

                return Page();
            }
        }
}
