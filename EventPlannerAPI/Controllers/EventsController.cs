using Microsoft.AspNetCore.Mvc;
using EventPlannerAPI.Models;

namespace EventPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly EventsDBContext _context;

    public EventsController(EventsDBContext context)
    {
        _context = context;
    }

    // EVENT ENDPOINTS:

    // GET: api/Events
    [HttpGet]
    public ActionResult GetEvents()
    {   // Projecting needed information into an anonymous type.
        var events = _context.Events
            .Select(e => new
            {
                e.EventId,
                e.Title,
                e.Date,
                e.Description,
                // Avoiding circular references by selectively projecting only necessary 
                // information about People related to each Event, preventing serialization issues.
                EventsPeople = e.EventsPeople.Select(ep => new { ep.Person.PersonId, ep.Person.Name, ep.Person.Age })
            }).ToList();
        return Ok(events);
    }

    // GET: api/Events/5
    [HttpGet("{id}")]
    public ActionResult GetEvent(int id)
    {
        var eventItem = _context.Events
            .Where(e => e.EventId == id)
            .Select(e => new
            {
                e.EventId,
                e.Title,
                e.Date,
                e.Description,
                // Avoiding circular references by selectively projecting only necessary 
                // information about People related to each Event, preventing serialization issues.
                EventsPeople = e.EventsPeople.Select(ep => new { ep.Person.PersonId, ep.Person.Name, ep.Person.Age })
            }).First();
        if (eventItem == null)
        {
            return NotFound();
        }
        return Ok(eventItem);
    }

    // PUT: api/Events/5
    [HttpPut("{id}")]
    public ActionResult UpdateEvent(int id, [FromBody] Event updatedEvent)
    {
        if (id != updatedEvent.EventId)
        {
            return BadRequest();
        }

        var existingEvent = _context.Events.Find(id);
        if (existingEvent == null)
        {
            return NotFound();
        }

        // Updating only specific fields of the event.
        existingEvent.Title = updatedEvent.Title;
        existingEvent.Date = updatedEvent.Date;
        existingEvent.Description = updatedEvent.Description;
        // NOT GOING TO ADD (UPDATE LIST) PEOPLE HERE!!!! GOING TO USE SEPERATE ENDPOINT.

        // Saving the changes to the DB.
        _context.SaveChanges();

        return NoContent();
    }
    // PEOPLE ENDPOINTS:

    [HttpGet("/People")]
    public ActionResult GetPeople()
    {   // Projecting needed information into an anonymous type.
        var people = _context.People
            .Select(p => new
            {
                p.PersonId,
                p.Name,
                p.Age,
                // Avoiding circular references by selectively projecting only necessary 
                // information about Events related to each Person, preventing serialization issues.
                EventsPeople = p.EventsPeople.Select(ep => new { ep.Event.EventId, ep.Event.Title, ep.Event.Description })
            }).ToList();
        return Ok(people);
    }

}