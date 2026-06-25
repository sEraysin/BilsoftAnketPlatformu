using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilsoftAnketPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class AssignSurveyRecordsToPersonnel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE anket_musteri SET personelID = 1 WHERE servisID % 3 = 1;");
            migrationBuilder.Sql("UPDATE anket_musteri SET personelID = 2 WHERE servisID % 3 = 2;");
            migrationBuilder.Sql("UPDATE anket_musteri SET personelID = 3 WHERE servisID % 3 = 0;");
            migrationBuilder.Sql("UPDATE anket_cevaplar SET personelID = 1 WHERE servisNo % 3 = 1;");
            migrationBuilder.Sql("UPDATE anket_cevaplar SET personelID = 2 WHERE servisNo % 3 = 2;");
            migrationBuilder.Sql("UPDATE anket_cevaplar SET personelID = 3 WHERE servisNo % 3 = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE anket_musteri SET personelID = NULL WHERE servisID % 3 = 1;");
            migrationBuilder.Sql("UPDATE anket_musteri SET personelID = NULL WHERE servisID % 3 = 2;");
            migrationBuilder.Sql("UPDATE anket_musteri SET personelID = NULL WHERE servisID % 3 = 0;");
            migrationBuilder.Sql("UPDATE anket_cevaplar SET personelID = NULL WHERE servisNo % 3 = 1;");
            migrationBuilder.Sql("UPDATE anket_cevaplar SET personelID = NULL WHERE servisNo % 3 = 2;");
            migrationBuilder.Sql("UPDATE anket_cevaplar SET personelID = NULL WHERE servisNo % 3 = 0;");
        }
    }
}
