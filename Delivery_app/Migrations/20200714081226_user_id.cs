using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery_app.Migrations
{
    public partial class user_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_drop_points_orders_order_id",
                table: "drop_points");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "order_id",
                table: "drop_points",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_drop_points_orders_order_id",
                table: "drop_points",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_user_id",
                table: "orders",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_drop_points_orders_order_id",
                table: "drop_points");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_user_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_user_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "order_id",
                table: "drop_points",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_drop_points_orders_order_id",
                table: "drop_points",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "order_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
