namespace bestpricesale.Pages
{
    using bestpricesale.Services;
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

            public IEnumerable<Models.Page> Pages { get; set; }

            public async Task OnGetAsync()
            {
                Pages = await _pageService.GetAllPagesAsync();
            }
        }
}
