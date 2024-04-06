
using Microsoft.EntityFrameworkCore;

namespace EventPlannerAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Access the configuration to retrieve the connection string
        var connectionString = builder.Configuration.GetConnectionString("EAD2_CA2_DATABASE_CONNECTION_STRING");
        // Register DbContext with the connection string and enable retry on failure
        builder.Services.AddDbContext<EventsDBContext>(options =>
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5, // Maximum number of retries
                    maxRetryDelay: TimeSpan.FromSeconds(30), // Delay between retries.
                    errorNumbersToAdd: null);
            }
         ));

        // Building the API application using the configuration set.
        var app = builder.Build();

        // POST BUILD CONFIGURATION:

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment()) //(Removed condition to display swagger API UI on Azure)
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // When opening up the API, automatically reroute to the swagger page.
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        app.MapControllers();

        app.Run();
    }
}
