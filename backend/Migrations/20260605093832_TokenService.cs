using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class TokenService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a8db037-2301-4946-b8b0-4ce3f781d3dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8283778-b3b0-48f6-b693-3b2a5d1ab3fc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "57d2e0b4-74c8-4d83-a185-f8e1c5cc6d83", null, "Admin", "ADMIN" },
                    { "64067838-6726-4346-98e2-3ce0858965c0", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57d2e0b4-74c8-4d83-a185-f8e1c5cc6d83");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64067838-6726-4346-98e2-3ce0858965c0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a8db037-2301-4946-b8b0-4ce3f781d3dd", null, "User", "USER" },
                    { "d8283778-b3b0-48f6-b693-3b2a5d1ab3fc", null, "Admin", "ADMIN" }
                });
        }
    }
}
