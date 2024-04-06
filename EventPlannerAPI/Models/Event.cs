using System.ComponentModel.DataAnnotations;

namespace EventPlannerAPI.Models;

// REMEMBER to migrate changes to database on azure after making changes to this Person model class.

public class Event
{
    [Key]
    public int EventId { get; set; } // Primary Key

    [Required]
    [StringLength(20)]
    public string Title { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public List<EventPerson> EventsPeople { get; set; } // For many-to-many relationship.
}
