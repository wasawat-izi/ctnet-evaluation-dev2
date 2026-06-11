using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UsernameApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b3b8e06-50b3-4af7-ae2e-1137b00001c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8118ae90-669b-4b9a-b639-ab01d8c9b406");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05cf0e89-b0be-48e4-a13d-ceb8462c1f0d", null, "User", "USER" },
                    { "17057c53-668a-4941-a892-22adfd34d252", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05cf0e89-b0be-48e4-a13d-ceb8462c1f0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17057c53-668a-4941-a892-22adfd34d252");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7b3b8e06-50b3-4af7-ae2e-1137b00001c7", null, "Admin", "ADMIN" },
                    { "8118ae90-669b-4b9a-b639-ab01d8c9b406", null, "User", "USER" }
                });
        }
    }
}
