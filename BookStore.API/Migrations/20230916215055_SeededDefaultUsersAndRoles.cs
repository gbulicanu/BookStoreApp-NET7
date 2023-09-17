using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00000000-0000-0000-C000-000000000046", null, "User", "USER" },
                    { "00020400-0000-0000-C000-000000000046", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "00000000-0000-0000-C000-000000000046", 0, "04ff558a-b5c8-4a39-838c-5f6a605d5acf", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEMyFrrMR3XLil1Urhw31PoJD4AK+T4dIeHn9XDNv0pYTRZ8boQM7kc1VCLLtmQvwmg==", null, false, "9e63e650-5deb-4392-889f-b9c657cfb2af", false, "user@bookstore.com" },
                    { "0000011B-0000-0000-C000-000000000046", 0, "0f8eb47c-28f8-40f0-8c33-4340c6d42b70", "admin@bookstore.com", false, "System", "Administrator", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAECtdEYhiwAZqoRT/0rYSqK8EiNunz4+ZfJlJmURoC+GJI3LHMYGVRdVh6gZY0k0uBg==", null, false, "60d8597a-4fb9-46ba-82e6-3cf4d07067dd", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "00000000-0000-0000-C000-000000000046", "00000000-0000-0000-C000-000000000046" },
                    { "00020400-0000-0000-C000-000000000046", "0000011B-0000-0000-C000-000000000046" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "00000000-0000-0000-C000-000000000046", "00000000-0000-0000-C000-000000000046" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "00020400-0000-0000-C000-000000000046", "0000011B-0000-0000-C000-000000000046" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-C000-000000000046");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00020400-0000-0000-C000-000000000046");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-C000-000000000046");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0000011B-0000-0000-C000-000000000046");
        }
    }
}
