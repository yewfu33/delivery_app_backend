using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class alterusertypeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_type",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "user_type_id",
                table: "users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_type_id",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "user_type",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
