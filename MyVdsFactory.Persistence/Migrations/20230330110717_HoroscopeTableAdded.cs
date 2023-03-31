using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyVdsFactory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class HoroscopeTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Horoscopes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    DateRange = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horoscopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terrors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false, defaultValue: 2000),
                    Month = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Day = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    Region = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    City = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    Location = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    Latitude = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Longitude = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Summary = table.Column<string>(type: "text", nullable: false, defaultValue: "Eklenmemiş"),
                    Alternative = table.Column<string>(type: "text", nullable: false, defaultValue: "Eklenmemiş"),
                    Success = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Suicide = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AttackType = table.Column<string>(type: "text", nullable: false, defaultValue: "Eklenmemiş"),
                    TargetType = table.Column<string>(type: "text", nullable: false, defaultValue: "Eklenmemiş"),
                    TargetSubType = table.Column<string>(type: "text", nullable: false, defaultValue: "Eklenmemiş"),
                    GroupName = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    GroupSubName = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    WeaponType = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    WeaponSubType = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    WeaponDetail = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    Kill = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    DbSource = table.Column<string>(type: "text", nullable: false, defaultValue: "Bilinmeyen"),
                    CityLatitude = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    CityLongitude = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    CountryLatitude = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    CountryLongitude = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoroscopeCommentaries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Commentary = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HoroscopeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoroscopeCommentaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoroscopeCommentaries_Horoscopes_HoroscopeId",
                        column: x => x.HoroscopeId,
                        principalTable: "Horoscopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoroscopeCommentaries_HoroscopeId",
                table: "HoroscopeCommentaries",
                column: "HoroscopeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoroscopeCommentaries");

            migrationBuilder.DropTable(
                name: "Terrors");

            migrationBuilder.DropTable(
                name: "Horoscopes");
        }
    }
}
