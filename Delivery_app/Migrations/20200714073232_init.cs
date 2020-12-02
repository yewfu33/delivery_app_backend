using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_status",
                columns: table => new
                {
                    delivery_status_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_status", x => x.delivery_status_id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    pick_up_address = table.Column<string>(nullable: true),
                    weight = table.Column<double>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    contact_num = table.Column<string>(nullable: true),
                    pick_up_datetime = table.Column<DateTime>(nullable: false),
                    price = table.Column<double>(nullable: false),
                    delivery_status_id = table.Column<int>(nullable: false),
                    vehicle_type_id = table.Column<int>(nullable: false),
                    notify_sender = table.Column<bool>(nullable: false),
                    notify_recipient = table.Column<bool>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                });

            migrationBuilder.CreateTable(
                name: "user_type",
                columns: table => new
                {
                    user_type_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_type", x => x.user_type_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    password = table.Column<string>(nullable: true),
                    password_salt = table.Column<string>(nullable: true),
                    phone_num = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    email = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    user_type = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_type",
                columns: table => new
                {
                    vehicle_type_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_type", x => x.vehicle_type_id);
                });

            migrationBuilder.CreateTable(
                name: "drop_points",
                columns: table => new
                {
                    drop_point_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    address = table.Column<string>(nullable: true),
                    contact_num = table.Column<string>(nullable: true),
                    datetime = table.Column<DateTime>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false),
                    order_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drop_points", x => x.drop_point_id);
                    table.ForeignKey(
                        name: "FK_drop_points_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_drop_points_order_id",
                table: "drop_points",
                column: "order_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_status");

            migrationBuilder.DropTable(
                name: "drop_points");

            migrationBuilder.DropTable(
                name: "user_type");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehicle_type");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
