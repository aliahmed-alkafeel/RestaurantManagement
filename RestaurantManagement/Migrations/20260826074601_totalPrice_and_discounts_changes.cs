using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class totalPrice_and_discounts_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemsOrders_ItemOrderOrderId_ItemOrderItemId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemOrderOrder");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemOrderOrderId_ItemOrderItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemOrderItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemOrderOrderId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Discounts");

            migrationBuilder.AlterColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ItemsOrders",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldPrecision: 18,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Items",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldPrecision: 18,
                oldScale: 5);

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOrders_ItemId",
                table: "ItemsOrders",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsOrders_Items_ItemId",
                table: "ItemsOrders",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsOrders_Orders_OrderId",
                table: "ItemsOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsOrders_Items_ItemId",
                table: "ItemsOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsOrders_Orders_OrderId",
                table: "ItemsOrders");

            migrationBuilder.DropIndex(
                name: "IX_ItemsOrders_ItemId",
                table: "ItemsOrders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "OrderStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ItemsOrders",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Items",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemOrderItemId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemOrderOrderId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Discounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ItemOrderOrder",
                columns: table => new
                {
                    OrdersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemOrdersOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemOrdersItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemOrderOrder", x => new { x.OrdersId, x.ItemOrdersOrderId, x.ItemOrdersItemId });
                    table.ForeignKey(
                        name: "FK_ItemOrderOrder_ItemsOrders_ItemOrdersOrderId_ItemOrdersItemId",
                        columns: x => new { x.ItemOrdersOrderId, x.ItemOrdersItemId },
                        principalTable: "ItemsOrders",
                        principalColumns: new[] { "OrderId", "ItemId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemOrderOrder_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemOrderOrderId_ItemOrderItemId",
                table: "Items",
                columns: new[] { "ItemOrderOrderId", "ItemOrderItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrderOrder_ItemOrdersOrderId_ItemOrdersItemId",
                table: "ItemOrderOrder",
                columns: new[] { "ItemOrdersOrderId", "ItemOrdersItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemsOrders_ItemOrderOrderId_ItemOrderItemId",
                table: "Items",
                columns: new[] { "ItemOrderOrderId", "ItemOrderItemId" },
                principalTable: "ItemsOrders",
                principalColumns: new[] { "OrderId", "ItemId" });
        }
    }
}
