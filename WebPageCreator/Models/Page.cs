using System;
using System.ComponentModel.DataAnnotations;

namespace bestpricesale.Models
{
    public class Page
    {
        public Guid Id { get; set; }

        [Required, StringLength(150)]
        public string Title  { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string AuthorId { get; set; }
        public int Version { get; set; } = 1;
        public List<PageVersion> Versions { get; set; } = new();
    }

    public class PageVersion
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public Page Page { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public int VersionNumber { get; set; }
    }
}