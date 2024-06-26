﻿using Microsoft.AspNetCore.Mvc;
using EventPlannerAPI.Models;
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
                e.Price,
                e.Type,
                // Avoiding circular references by selectively projecting only necessary 
                // information about People related to each Event, preventing serialization issues.
                EventsPeople = e.EventsPeople.Select(ep => new { ep.Person.PersonId, ep.Person.Name, ep.Person.Age })
            }).ToList();
        return Ok(events);
    }

    // GET: api/Events/5
    [HttpGet("{id}")]
    public ActionResult GetEventById(int id)
    {
        var eventItem = _context.Events
            .Where(e => e.EventId == id)
            .Select(e => new
            {
                e.EventId,
                e.Title,
                e.Date,
                e.Description,
                e.Price,
                e.Type,
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
        existingEvent.Price = updatedEvent.Price;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.Type = updatedEvent.Type;

        // Saving the changes to the DB.
        _context.SaveChanges();

        return NoContent();
    }

    // POST: api/Events/
    [HttpPost]
    public ActionResult<Event> AddNewEvent([FromBody] Event newEvent)
    {
        _context.Events.Add(newEvent);
        _context.SaveChanges(); // ef generates id on save cause of the attribute set on the key.
        return CreatedAtAction(nameof(GetEventById), new { id = newEvent.EventId }, newEvent);
    }

    // DELETE: api/Events/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteEventById(int id)
    {
        var eventToDelete = _context.Events.Include(e => e.EventsPeople).FirstOrDefault(e => e.EventId == id);
        if (eventToDelete == null)
        {
            return NotFound("Event not found.");
        }
        // Deleting the relationships between this event and all of it's people.
        _context.EventPerson.RemoveRange(eventToDelete.EventsPeople);

        // Finally removing the actuall event.
        _context.Events.Remove(eventToDelete);

        _context.SaveChanges();
        return Ok($"Deleted Event with id {id}");
    }

    // PEOPLE ENDPOINTS:

    [HttpGet("People")]
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

    // GET: api/Events/5
    [HttpGet("Person/{id}")]
    public ActionResult GetPersonById(int id)
    {
        var personItem = _context.People
            .Where(p => p.PersonId == id)
            .Select(p => new
            {
                p.PersonId,
                p.Name,
                p.Age,
                // Avoiding circular references by selectively projecting only necessary 
                // information about Events related to each Person, preventing serialization issues.
                EventsPeople = p.EventsPeople.Select(ep => new { ep.Event.EventId, ep.Event.Title, ep.Event.Description })
            }).First();
        if (personItem == null)
        {
            return NotFound();
        }
        return Ok(personItem);
    }

    // Update: api/Events/Person/{personId}
    [HttpPut("Person/{personId}")]
    public ActionResult UpdatePerson(int personId, [FromBody] Person updatedPerson)
    {
        if (personId != updatedPerson.PersonId)
        {
            return BadRequest();
        }
        var existingPerson = _context.People.Find(personId);
        if (existingPerson == null)
        {
            return NotFound();
        }
        // Updating only specific fields of the event.
        existingPerson.Name = updatedPerson.Name;
        existingPerson.Age = updatedPerson.Age;
        // Saving the changes to the DB.
        _context.SaveChanges();
        return NoContent();
    }

    // DELETE: api/Events/Person/{personId}
    [HttpDelete("Person/{personId}")]
    public ActionResult DeletePersonById(int personId)
    {
        var personToDelete = _context.People.Find(personId);
        if (personToDelete == null)
        {
            return NotFound("Event not found.");
        }
        // Remove the person.
        _context.People.Remove(personToDelete);
        _context.SaveChanges(); // Save the changes.
        return Ok($"Deleted Person with id: {personId}");
    }

    [HttpPost("{eventId}/Person/{personId}")]
    public ActionResult AddPersonToEvent(int eventId, int personId)
    {
        // Check if the event exists within the intermediate table. (Supporting Many to Many)
        var eventItem = _context.Events.Include(e => e.EventsPeople).SingleOrDefault(e => e.EventId == eventId);
        if (eventItem == null)
        {
            return NotFound("Event not found.");
        }
        // Check if the person exists
        var personItem = _context.People.Find(personId);
        if (personItem == null)
        {
            return NotFound("Person not found.");
        }
        // Checking if the person is already going to the event.

        var existingLink = eventItem.EventsPeople.FirstOrDefault(ep => ep.PersonId == personId);
        if (existingLink != null) { 
        
            return BadRequest("Person is already added to the event.");
        }
        // Add the person to the event.
        eventItem.EventsPeople.Add(new EventPerson { EventId = eventId, PersonId = personId });

        _context.SaveChanges();

        return Ok("Person added to event successfully.");
    }

    [HttpPost("Person")]
    public ActionResult AddNewPerson([FromBody] Person newPerson)
    {
        _context.People.Add(newPerson);
        _context.SaveChanges(); // ef generates id on save cause of the attribute set on the key.
        return CreatedAtAction(nameof(GetPersonById), new { id = newPerson.PersonId }, newPerson);
    }
}