using System.ComponentModel.DataAnnotations;

public class Template
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}