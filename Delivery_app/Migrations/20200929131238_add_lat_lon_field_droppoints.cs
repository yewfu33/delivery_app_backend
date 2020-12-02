using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class add_lat_lon_field_droppoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "latitude",
                table: "drop_points",
                type: "FLOAT(10, 6)",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "longitude",
                table: "drop_points",
                type: "FLOAT(10, 6)",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "drop_points");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "drop_points");
        }
    }
}
