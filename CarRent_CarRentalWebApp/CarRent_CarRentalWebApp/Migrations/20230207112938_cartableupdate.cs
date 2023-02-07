using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRent_CarRentalWebApp.Migrations
{
    public partial class cartableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isNew",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isNew",
                table: "Cars");
        }
    }
}
