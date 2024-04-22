using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlannerAPI.Models;

public enum EventType {
    FoodFestival, MusicConcert, TechConference, SportsMatch, ArtExhibition
}

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

    [Required]
    public EventType Type { get; set; }

    public List<EventPerson> EventsPeople { get; set; } // For many-to-many relationship.
}
