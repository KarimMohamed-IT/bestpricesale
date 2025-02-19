namespace bestpricesale.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Template
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }  // e.g., "Event", "Product"

        public string Description { get; set; }
    }

}