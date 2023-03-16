using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyVdsFactory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Earthquake_table_date_modified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Earthquakes",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Earthquakes",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Earthquakes",
                type: "integer",
                nullable: false,
                defaultValue: 2006);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Earthquakes");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Earthquakes");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Earthquakes");
        }
    }
}
