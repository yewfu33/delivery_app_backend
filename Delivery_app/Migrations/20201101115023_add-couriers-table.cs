using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class addcourierstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "couriers",
                columns: table => new
                {
                    courier_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    otp = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    password_salt = table.Column<string>(nullable: true),
                    phone_num = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    email = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    profile_picture = table.Column<string>(nullable: true),
                    vehicle_type = table.Column<byte>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_couriers", x => x.courier_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "couriers");
        }
    }
}
