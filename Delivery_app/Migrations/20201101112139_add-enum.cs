using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class addenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_status");

            migrationBuilder.DropTable(
                name: "user_type");

            migrationBuilder.DropTable(
                name: "vehicle_type");

            migrationBuilder.DropColumn(
                name: "user_type_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "delivery_status_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "vehicle_type_id",
                table: "orders");

            migrationBuilder.AddColumn<byte>(
                name: "user_type",
                table: "users",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "delivery_status",
                table: "orders",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "vehicle_type",
                table: "orders",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_type",
                table: "users");

            migrationBuilder.DropColumn(
                name: "delivery_status",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "vehicle_type",
                table: "orders");

            migrationBuilder.AddColumn<int>(
                name: "user_type_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "delivery_status_id",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "vehicle_type_id",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "delivery_status",
                columns: table => new
                {
                    delivery_status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_status", x => x.delivery_status_id);
                });

            migrationBuilder.CreateTable(
                name: "user_type",
                columns: table => new
                {
                    user_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_type", x => x.user_type_id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_type",
                columns: table => new
                {
                    vehicle_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_type", x => x.vehicle_type_id);
                });
        }
    }
}
