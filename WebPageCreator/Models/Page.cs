namespace bestpricesale.Models
{
    public class Page
    {
        // The unique identifier used for filenames and URL slugs.
        public string Slug { get; set; }

        public string Title { get; set; }

        // The full HTML content of the page.
        public string Content { get; set; }

        // (Optional) Other properties like Template name can be added here.
    }
}
