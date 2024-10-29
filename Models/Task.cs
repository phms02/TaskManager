using System.ComponentModel.DataAnnotations;

public class Task {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public string Status { get; set; } // "pending", "in progress", "completed"

    // Foreign key for the Person responsible for the task
    public int? PersonId { get; set; }

    // Navigation property
    public virtual Person Person { get; set; }
}