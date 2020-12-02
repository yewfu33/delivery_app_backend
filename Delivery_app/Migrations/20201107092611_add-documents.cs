using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class adddocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "driving_license",
                table: "couriers");

            migrationBuilder.DropColumn(
                name: "identity_card",
                table: "couriers");

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    document_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    document = table.Column<string>(nullable: true),
                    courier_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.document_id);
                    table.ForeignKey(
                        name: "FK_documents_couriers_courier_id",
                        column: x => x.courier_id,
                        principalTable: "couriers",
                        principalColumn: "courier_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_documents_courier_id",
                table: "documents",
                column: "courier_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.AddColumn<string>(
                name: "driving_license",
                table: "couriers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "identity_card",
                table: "couriers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
