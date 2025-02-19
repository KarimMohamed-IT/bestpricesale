namespace bestpricesale.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EventOption
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }  // For simplicity (could be DateTime)
        public string Location { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // Foreign key to EventDetail
        public int EventDetailId { get; set; }
        [ForeignKey("EventDetailId")]
        public EventDetail EventDetail { get; set; }
    }

}