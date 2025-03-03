namespace bestpricesale.Pages
{
    using bestpricesale.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {
        private readonly IPageService _pageService;

        public IndexModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        public List<bestpricesale.Models.Page> Pages { get; set; }

        public async Task OnGetAsync()
        {
            var pages = await _pageService.GetAllPagesAsync();
            Pages = pages.ToList();
        }

        public async Task<IActionResult> OnGetDeletePageAsync(string slug)
        {
            await _pageService.DeletePageAsync(slug); // Delete by ID directly
            return RedirectToPage();
        }

    }
}
