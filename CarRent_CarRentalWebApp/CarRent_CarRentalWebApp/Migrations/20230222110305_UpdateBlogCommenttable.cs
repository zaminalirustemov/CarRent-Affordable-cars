using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRent_CarRentalWebApp.Migrations
{
    public partial class UpdateBlogCommenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_Cars_CarId",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_CarId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "BlogComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "BlogComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_CarId",
                table: "BlogComments",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_Cars_CarId",
                table: "BlogComments",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }
    }
}
