using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRent_CarRentalWebApp.Migrations
{
    public partial class UpdateCarPecularityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarPeculiarities_Peculiarities_PeculiarityId",
                table: "CarPeculiarities");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "CarPeculiarities");

            migrationBuilder.AlterColumn<int>(
                name: "PeculiarityId",
                table: "CarPeculiarities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarPeculiarities_Peculiarities_PeculiarityId",
                table: "CarPeculiarities",
                column: "PeculiarityId",
                principalTable: "Peculiarities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarPeculiarities_Peculiarities_PeculiarityId",
                table: "CarPeculiarities");

            migrationBuilder.AlterColumn<int>(
                name: "PeculiarityId",
                table: "CarPeculiarities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FeatureId",
                table: "CarPeculiarities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CarPeculiarities_Peculiarities_PeculiarityId",
                table: "CarPeculiarities",
                column: "PeculiarityId",
                principalTable: "Peculiarities",
                principalColumn: "Id");
        }
    }
}
