using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class add_lag_lon_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "latitude",
                table: "orders",
                type: "FLOAT(10, 6)",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "longitude",
                table: "orders",
                type: "FLOAT(10, 6)",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "orders");
        }
    }
}
