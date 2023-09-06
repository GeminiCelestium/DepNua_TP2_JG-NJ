using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModernRecrut.Postulations.API.Migrations
{
    public partial class InitPostulation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postulation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    iDCandidat = table.Column<string>(type: "TEXT", nullable: false),
                    OffreDemploiID = table.Column<int>(type: "INTEGER", nullable: false),
                    PretentionSalariale = table.Column<decimal>(type: "TEXT", nullable: false),
                    DateDisponibilite = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postulation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteRh = table.Column<string>(type: "TEXT", nullable: false),
                    NoteEmeteur = table.Column<string>(type: "TEXT", nullable: false),
                    PostulationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_Postulation_PostulationId",
                        column: x => x.PostulationId,
                        principalTable: "Postulation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Note_PostulationId",
                table: "Note",
                column: "PostulationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Postulation");
        }
    }
}
