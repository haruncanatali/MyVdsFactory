using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyVdsFactory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Earthquake_table_date_modified_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rms",
                table: "Earthquakes");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Earthquakes",
                newName: "Type");

            migrationBuilder.AlterColumn<decimal>(
                name: "Magnitude",
                table: "Earthquakes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Earthquakes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Earthquakes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<decimal>(
                name: "Depth",
                table: "Earthquakes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Depth",
                table: "Earthquakes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Earthquakes",
                newName: "Country");

            migrationBuilder.AlterColumn<double>(
                name: "Magnitude",
                table: "Earthquakes",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Earthquakes",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Earthquakes",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<double>(
                name: "Rms",
                table: "Earthquakes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
