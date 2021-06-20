using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiAppMappings_Data.Migrations
{
    public partial class newName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "Samurais",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "Samurais",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "Samurais");
        }
    }
}
