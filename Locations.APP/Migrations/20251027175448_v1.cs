using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locations.APP.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Guid = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country_Indx", x => x.Id);
                }
            );
            
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Guid = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City_Indx", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Country_Id",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_City_Name",
                table: "City",
                column: "CityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                table: "Country",
                column: "CountryName",
                unique: true);
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
