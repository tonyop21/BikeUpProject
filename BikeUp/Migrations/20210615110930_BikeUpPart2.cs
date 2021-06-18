using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeUp.Migrations
{
    public partial class BikeUpPart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountSpent",
                table: "Customers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "NrOfRentedBikes",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRentingHours",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesRented",
                table: "Bikes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountSpent",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NrOfRentedBikes",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TotalRentingHours",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TimesRented",
                table: "Bikes");
        }
    }
}
