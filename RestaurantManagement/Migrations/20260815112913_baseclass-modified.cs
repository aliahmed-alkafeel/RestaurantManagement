using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class baseclassmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ItemsOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemsOrders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "GroupsRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "GroupsRoles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "Roles",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "Orders",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "ItemsOrders",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "Items",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "GroupsRoles",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "Groups",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "Employees",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "Discounts",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "DeletedId",
                table: "Categories",
                newName: "UpdatedById");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "ItemsOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "GroupsRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Discounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "ItemsOrders");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "GroupsRoles");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Roles",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Orders",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "ItemsOrders",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Items",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "GroupsRoles",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Groups",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Employees",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Discounts",
                newName: "DeletedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Categories",
                newName: "DeletedId");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ItemsOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ItemsOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GroupsRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "GroupsRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
