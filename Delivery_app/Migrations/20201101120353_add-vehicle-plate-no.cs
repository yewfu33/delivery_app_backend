using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class addvehicleplateno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "vehicle_plate_no",
                table: "couriers",
                type: "VARCHAR(10)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vehicle_plate_no",
                table: "couriers");
        }
    }
}
