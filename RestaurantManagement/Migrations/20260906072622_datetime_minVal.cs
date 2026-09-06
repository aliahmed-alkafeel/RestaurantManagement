using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class datetime_minVal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupsRoles_GroupId_RoleId_DeletedAt_IsDeleted",
                table: "GroupsRoles");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsRoles_GroupId_RoleId_DeletedAt_IsDeleted",
                table: "GroupsRoles",
                columns: new[] { "GroupId", "RoleId", "DeletedAt", "IsDeleted" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupsRoles_GroupId_RoleId_DeletedAt_IsDeleted",
                table: "GroupsRoles");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsRoles_GroupId_RoleId_DeletedAt_IsDeleted",
                table: "GroupsRoles",
                columns: new[] { "GroupId", "RoleId", "DeletedAt", "IsDeleted" });
        }
    }
}
