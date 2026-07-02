using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilsoftAnketPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class EnableIdentityLockout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "LockoutEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "LockoutEnabled",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "LockoutEnabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "LockoutEnabled",
                value: false);
        }
    }
}
