using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class delete_composite_keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemsOrders",
                table: "ItemsOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupsRoles",
                table: "GroupsRoles");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ItemsOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "GroupsRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemsOrders",
                table: "ItemsOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupsRoles",
                table: "GroupsRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOrders_OrderId_ItemId_IsDeleted_DeletedAt",
                table: "ItemsOrders",
                columns: new[] { "OrderId", "ItemId", "IsDeleted", "DeletedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupsRoles_GroupId_RoleId_DeletedAt_IsDeleted",
                table: "GroupsRoles",
                columns: new[] { "GroupId", "RoleId", "DeletedAt", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemsOrders",
                table: "ItemsOrders");

            migrationBuilder.DropIndex(
                name: "IX_ItemsOrders_OrderId_ItemId_IsDeleted_DeletedAt",
                table: "ItemsOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupsRoles",
                table: "GroupsRoles");

            migrationBuilder.DropIndex(
                name: "IX_GroupsRoles_GroupId_RoleId_DeletedAt_IsDeleted",
                table: "GroupsRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ItemsOrders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GroupsRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemsOrders",
                table: "ItemsOrders",
                columns: new[] { "OrderId", "ItemId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupsRoles",
                table: "GroupsRoles",
                columns: new[] { "GroupId", "RoleId" });
        }
    }
}
