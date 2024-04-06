using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventPlannerAPI.Models;

// REMEMBER to migrate changes to database on azure after making changes to this Person model class.

public class Person
{
    [Key]
    public int PersonId { get; set; } // Primary Key

    [Required]
    [StringLength(20)]
    public string Name { get; set; }

    [Required]
    [Range(1, 120)]
    public int Age { get; set; }

    public List<EventPerson> EventsPeople { get; set; } // For many-to-many relationship.
}