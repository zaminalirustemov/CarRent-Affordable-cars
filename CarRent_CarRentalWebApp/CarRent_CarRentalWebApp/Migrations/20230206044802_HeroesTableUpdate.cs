using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRent_CarRentalWebApp.Migrations
{
    public partial class HeroesTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Hero",
                table: "Hero");

            migrationBuilder.RenameTable(
                name: "Hero",
                newName: "Heroes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Heroes",
                table: "Heroes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Heroes",
                table: "Heroes");

            migrationBuilder.RenameTable(
                name: "Heroes",
                newName: "Hero");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hero",
                table: "Hero",
                column: "Id");
        }
    }
}
