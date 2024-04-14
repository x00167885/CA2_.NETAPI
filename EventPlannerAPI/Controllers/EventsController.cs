using Microsoft.AspNetCore.Mvc;
using EventPlannerAPI.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                People = e.EventsPeople.Select(ep => new { ep.Person.PersonId, ep.Person.Name, ep.Person.Age })
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
                People = e.EventsPeople.Select(ep => new { ep.Person.PersonId, ep.Person.Name, ep.Person.Age })
            }).First();
        if (eventItem == null)
        {
            return NotFound();
        }
        return Ok(eventItem);
    }
}