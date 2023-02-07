using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRent_CarRentalWebApp.Migrations
{
    public partial class BrandsAndCarsAndCarImagesTablesCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerHour = table.Column<double>(type: "float", nullable: false),
                    PricePerDay = table.Column<double>(type: "float", nullable: false),
                    PricePerMonth = table.Column<double>(type: "float", nullable: false),
                    ConsumptionPer100KM = table.Column<double>(type: "float", nullable: false),
                    Millage = table.Column<int>(type: "int", nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    Luggage = table.Column<int>(type: "int", nullable: false),
                    Fuel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isFeatured = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    isRent = table.Column<bool>(type: "bit", nullable: false),
                    Airconditions = table.Column<bool>(type: "bit", nullable: false),
                    ChildSeat = table.Column<bool>(type: "bit", nullable: false),
                    GPS = table.Column<bool>(type: "bit", nullable: false),
                    Music = table.Column<bool>(type: "bit", nullable: false),
                    SeatBelt = table.Column<bool>(type: "bit", nullable: false),
                    SleepingBed = table.Column<bool>(type: "bit", nullable: false),
                    Bluetooth = table.Column<bool>(type: "bit", nullable: false),
                    OnboardComputer = table.Column<bool>(type: "bit", nullable: false),
                    LongTermTrips = table.Column<bool>(type: "bit", nullable: false),
                    CarKit = table.Column<bool>(type: "bit", nullable: false),
                    RemoteCentralLocking = table.Column<bool>(type: "bit", nullable: false),
                    ClimateControl = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isPoster = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarImages_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarImages_CarId",
                table: "CarImages",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BrandId",
                table: "Cars",
                column: "BrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarImages");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
