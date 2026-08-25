using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagement.Migrations
{
    /// <inheritdoc />
    public partial class softIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Email",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Username",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email_IsDeleted_DeletedAt",
                table: "Employees",
                columns: new[] { "Email", "IsDeleted", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username_IsDeleted_DeletedAt",
                table: "Employees",
                columns: new[] { "Username", "IsDeleted", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName_Type_IsDeleted_DeletedAt",
                table: "Categories",
                columns: new[] { "CategoryName", "Type", "IsDeleted", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Email_IsDeleted_DeletedAt",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Username_IsDeleted_DeletedAt",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryName_Type_IsDeleted_DeletedAt",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                column: "Username",
                unique: true);
        }
    }
}
