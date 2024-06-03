using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pustaka.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Penulis",
                columns: table => new
                {
                    PenulisId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nama = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penulis", x => x.PenulisId);
                });

            migrationBuilder.CreateTable(
                name: "Buku",
                columns: table => new
                {
                    BukuId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Judul = table.Column<string>(type: "TEXT", nullable: false),
                    PenulisId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buku", x => x.BukuId);
                    table.ForeignKey(
                        name: "FK_Buku_Penulis_PenulisId",
                        column: x => x.PenulisId,
                        principalTable: "Penulis",
                        principalColumn: "PenulisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buku_PenulisId",
                table: "Buku",
                column: "PenulisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buku");

            migrationBuilder.DropTable(
                name: "Penulis");
        }
    }
}
