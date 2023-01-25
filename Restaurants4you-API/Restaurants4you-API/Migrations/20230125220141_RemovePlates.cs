using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants4you_API.Migrations
{
    public partial class RemovePlates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plates");

            migrationBuilder.AddColumn<int>(
                name: "UserFK",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_UserFK",
                table: "Restaurant",
                column: "UserFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Users_UserFK",
                table: "Restaurant",
                column: "UserFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Users_UserFK",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_UserFK",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "UserFK",
                table: "Restaurant");

            migrationBuilder.CreateTable(
                name: "Plates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantFK = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plates_Restaurant_RestaurantFK",
                        column: x => x.RestaurantFK,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plates_RestaurantFK",
                table: "Plates",
                column: "RestaurantFK");
        }
    }
}
