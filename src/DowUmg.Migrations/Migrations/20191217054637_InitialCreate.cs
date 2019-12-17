using Microsoft.EntityFrameworkCore.Migrations;

namespace DowUmg.Migrations.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    IsVanilla = table.Column<bool>(nullable: false),
                    ModFolder = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => new { x.IsVanilla, x.ModFolder });
                });

            migrationBuilder.CreateTable(
                name: "GameRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    ModId1 = table.Column<bool>(nullable: false),
                    ModId2 = table.Column<string>(nullable: true),
                    IsWinCondition = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRules_Mods_ModId1_ModId2",
                        columns: x => new { x.ModId1, x.ModId2 },
                        principalTable: "Mods",
                        principalColumns: new[] { "IsVanilla", "ModFolder" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    ModId1 = table.Column<bool>(nullable: false),
                    ModId2 = table.Column<string>(nullable: true),
                    Players = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Mods_ModId1_ModId2",
                        columns: x => new { x.ModId1, x.ModId2 },
                        principalTable: "Mods",
                        principalColumns: new[] { "IsVanilla", "ModFolder" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ModId1 = table.Column<bool>(nullable: false),
                    ModId2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_Mods_ModId1_ModId2",
                        columns: x => new { x.ModId1, x.ModId2 },
                        principalTable: "Mods",
                        principalColumns: new[] { "IsVanilla", "ModFolder" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRules_ModId1_ModId2",
                table: "GameRules",
                columns: new[] { "ModId1", "ModId2" });

            migrationBuilder.CreateIndex(
                name: "IX_Maps_ModId1_ModId2",
                table: "Maps",
                columns: new[] { "ModId1", "ModId2" });

            migrationBuilder.CreateIndex(
                name: "IX_Races_ModId1_ModId2",
                table: "Races",
                columns: new[] { "ModId1", "ModId2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRules");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "Mods");
        }
    }
}
