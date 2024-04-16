using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimsPlugin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "InsertBy", "InsertDateTime", "PasswordHash", "UpdatedBy", "UpdatedOn", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "test@gmail.com", null, null, "hashed-password", null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
