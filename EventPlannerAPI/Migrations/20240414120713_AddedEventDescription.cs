using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedEventDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 1,
                column: "Description",
                value: "This is an event about tech");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: 2,
                column: "Description",
                value: "Major Hackathon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Events");
        }
    }
}
