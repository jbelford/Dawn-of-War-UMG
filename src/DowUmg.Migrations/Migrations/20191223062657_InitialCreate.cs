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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsVanilla = table.Column<bool>(nullable: false),
                    ModFolder = table.Column<string>(nullable: false),
                    Playable = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    ModId = table.Column<int>(nullable: false),
                    IsWinCondition = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRules_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
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
                    ModId = table.Column<int>(nullable: false),
                    Players = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModDependencies",
                columns: table => new
                {
                    MainModId = table.Column<int>(nullable: false),
                    DepModId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModDependencies", x => new { x.MainModId, x.DepModId });
                    table.ForeignKey(
                        name: "FK_ModDependencies_Mods_DepModId",
                        column: x => x.DepModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModDependencies_Mods_MainModId",
                        column: x => x.MainModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
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
                    ModId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRules_ModId",
                table: "GameRules",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_ModId",
                table: "Maps",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_DepModId",
                table: "ModDependencies",
                column: "DepModId");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_IsVanilla_ModFolder",
                table: "Mods",
                columns: new[] { "IsVanilla", "ModFolder" });

            migrationBuilder.CreateIndex(
                name: "IX_Races_ModId",
                table: "Races",
                column: "ModId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRules");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "ModDependencies");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "Mods");
        }
    }
}
