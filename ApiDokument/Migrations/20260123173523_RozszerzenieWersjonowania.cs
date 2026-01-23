using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDokument.Migrations
{
    /// <inheritdoc />
    public partial class RozszerzenieWersjonowania : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DokumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumerWersji = table.Column<int>(type: "int", nullable: false),
                    SciezkaPliku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypPliku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentVersions_Dokumenty_DokumentId",
                        column: x => x.DokumentId,
                        principalTable: "Dokumenty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_DokumentId",
                table: "DocumentVersions",
                column: "DokumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_DokumentId_NumerWersji",
                table: "DocumentVersions",
                columns: new[] { "DokumentId", "NumerWersji" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentVersions");
        }
    }
}
