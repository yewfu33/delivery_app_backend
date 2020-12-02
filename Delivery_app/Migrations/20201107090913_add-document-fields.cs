using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class adddocumentfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "driving_license",
                table: "couriers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "identity_card",
                table: "couriers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "driving_license",
                table: "couriers");

            migrationBuilder.DropColumn(
                name: "identity_card",
                table: "couriers");
        }
    }
}
