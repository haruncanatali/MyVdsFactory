using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyVdsFactory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Prayers_Table_Modified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "Prayers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Prayers_CityId",
                table: "Prayers",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prayers_Cities_CityId",
                table: "Prayers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prayers_Cities_CityId",
                table: "Prayers");

            migrationBuilder.DropIndex(
                name: "IX_Prayers_CityId",
                table: "Prayers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Prayers");
        }
    }
}
