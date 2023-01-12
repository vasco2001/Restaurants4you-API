using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants4you_API.Migrations
{
    public partial class latitudeLongitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Restaurant",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Restaurant",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Restaurant");
        }
    }
}
