using EventPlannerAPI.Models;
using Microsoft.EntityFrameworkCore;

public class EventsDBContext : DbContext
{
    public EventsDBContext(DbContextOptions<EventsDBContext> options) : base(options){}

    // DbSet properties representing tables within the DB.
    public DbSet<Event> Events { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<EventPerson> EventPerson { get; set; } // This is the join / intermediate table to support the many to many relationship.

    // REMINDER!!!! Modifyiing this function (for initialising dummy data in the db), PERFORM A MIGRATION.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seeding the Event table.
        modelBuilder.Entity<Event>().HasData(
            new Event { EventId = 1, Title = "Tech Conference", Date = new DateTime(2023, 4, 12), Description = "This is an event about tech" , Price = 25, Type = EventType.MusicConcert},
            new Event { EventId = 2, Title = "Local Hackathon", Date = new DateTime(2023, 4, 22), Description = "Major Hackathon", Price = 30, Type = EventType.SportsMatch }
        );

        // Seeding the Person table.
        modelBuilder.Entity<Person>().HasData(
            new Person { PersonId = 1, Name = "Alice Smith", Age = 29 },
            new Person { PersonId = 2, Name = "Bob Jones", Age = 35 },
            new Person { PersonId = 3, Name = "Charlie Davis", Age = 22 }
        );

        // Defining keys for EventPerson (the join / intermediate table).
        modelBuilder.Entity<EventPerson>().HasKey(ep => new { ep.EventId, ep.PersonId });

        // Defining the relationship between Event and EventPerson.
        modelBuilder.Entity<EventPerson>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.EventsPeople)
            .HasForeignKey(ep => ep.EventId);

        // Define the relationship between Person and EventPerson.
        modelBuilder.Entity<EventPerson>()
            .HasOne(ep => ep.Person)
            .WithMany(p => p.EventsPeople)
            .HasForeignKey(ep => ep.PersonId);

        // Seeding the EventPerson join / intermediate table to establish many-to-many relationships.
        modelBuilder.Entity<EventPerson>().HasData(
            new EventPerson { EventId = 1, PersonId = 1 },
            new EventPerson { EventId = 1, PersonId = 2 },
            new EventPerson { EventId = 2, PersonId = 3 },
            new EventPerson { EventId = 2, PersonId = 1 } // People can possibly attend multiple events.
        );
    }
}