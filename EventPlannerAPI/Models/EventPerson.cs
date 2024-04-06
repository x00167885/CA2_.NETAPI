namespace EventPlannerAPI.Models;

// REMEMBER to migrate changes to database on azure after making changes to this Person model class.

public class EventPerson
{
    public int EventId { get; set; }
    public Event Event { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
}