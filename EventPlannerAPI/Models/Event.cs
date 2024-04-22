using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlannerAPI.Models;

// REMEMBER to migrate changes to database on azure after making changes to this Person model class.

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; } // Primary Key

    [Required]
    [StringLength(20)]
    public string Title { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public float Price { get; set; }

    public List<EventPerson> EventsPeople { get; set; } // For many-to-many relationship.
}
