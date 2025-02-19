namespace bestpricesale.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Page
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }  // URL slug (e.g., "TemplateName")

        public string Content { get; set; }

        // Foreign key to Template
        public int TemplateId { get; set; }
        [ForeignKey("TemplateId")]
        public Template Template { get; set; }

        // For Event template details (if applicable)
        public EventDetail EventDetail { get; set; }
    }

}
