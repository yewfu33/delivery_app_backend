using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class addonBoardcourier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "onBoard",
                table: "couriers",
                nullable: true,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "onBoard",
                table: "couriers");
        }
    }
}
