namespace bestpricesale.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Template
    {
        public string Name { get; set; }  // e.g., "Event", "Product"

        public string Content { get; set; }
    }

}