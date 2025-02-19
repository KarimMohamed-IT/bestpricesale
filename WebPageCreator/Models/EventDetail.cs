namespace bestpricesale.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EventDetail
    {
        [Key]
        public int Id { get; set; }

        public string EventTitle { get; set; }
        public string Description { get; set; }

        // One EventDetail can have many options
        public ICollection<EventOption> EventOptions { get; set; }

        // Foreign key to Page
        public int PageId { get; set; }
        [ForeignKey("PageId")]
        public Page Page { get; set; }
    }

}