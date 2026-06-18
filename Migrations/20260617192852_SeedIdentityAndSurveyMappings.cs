using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BilsoftAnketPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityAndSurveyMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin-role-id", "admin-role-stamp", "Admin", "ADMIN" },
                    { "kullanici-role-id", "kullanici-role-stamp", "Kullanici", "KULLANICI" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AdSoyad", "AktifMi", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", 0, "Sistem Admin", true, "admin-concurrency-stamp", "admin@bilsoft.com", true, false, null, "ADMIN@BILSOFT.COM", "ADMIN@BILSOFT.COM", "AQAAAAIAAYagAAAAEMygiBagleiMV23HKs8YExyJ/zKlwTYtbsWe+fn1f2psIg5fBcuXIBFepu+XT3Qq0Q==", null, false, "admin-security-stamp", false, "admin@bilsoft.com" },
                    { "22222222-2222-2222-2222-222222222222", 0, "Ayse Kullanici", true, "ayse-concurrency-stamp", "ayse@bilsoft.com", true, false, null, "AYSE@BILSOFT.COM", "AYSE@BILSOFT.COM", "AQAAAAIAAYagAAAAEDaj7mispSCG7o7/wiv8A2QH+/tXM44u4hEAqnCQl4r1zyYUNnVtqw2vUW3/cDWALQ==", null, false, "ayse-security-stamp", false, "ayse@bilsoft.com" }
                });

            migrationBuilder.InsertData(
                table: "Personel",
                columns: new[] { "PersonelID", "AdSoyad", "AktifMi" },
                values: new object[,]
                {
                    { 1, "Ahmet Yilmaz", true },
                    { 2, "Ayse Demir", true },
                    { 3, "Mehmet Kaya", true }
                });

            migrationBuilder.InsertData(
                table: "AppUserPersonel",
                columns: new[] { "Id", "PersonelID", "UserId" },
                values: new object[] { 1, 2, "22222222-2222-2222-2222-222222222222" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "admin-role-id", "11111111-1111-1111-1111-111111111111" },
                    { "kullanici-role-id", "22222222-2222-2222-2222-222222222222" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUserPersonel",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin-role-id", "11111111-1111-1111-1111-111111111111" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "kullanici-role-id", "22222222-2222-2222-2222-222222222222" });

            migrationBuilder.DeleteData(
                table: "Personel",
                keyColumn: "PersonelID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Personel",
                keyColumn: "PersonelID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "kullanici-role-id");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222");

            migrationBuilder.DeleteData(
                table: "Personel",
                keyColumn: "PersonelID",
                keyValue: 2);
        }
    }
}
