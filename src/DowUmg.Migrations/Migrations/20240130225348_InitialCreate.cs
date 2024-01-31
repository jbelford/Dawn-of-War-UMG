using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DowUmg.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsVanilla = table.Column<bool>(type: "INTEGER", nullable: false),
                    ModFolder = table.Column<string>(type: "TEXT", nullable: false),
                    Playable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "DowModDowMod",
                columns: table => new
                {
                    DependenciesId = table.Column<int>(type: "INTEGER", nullable: false),
                    DependentsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_DowModDowMod",
                        x => new { x.DependenciesId, x.DependentsId }
                    );
                    table.ForeignKey(
                        name: "FK_DowModDowMod_Mods_DependenciesId",
                        column: x => x.DependenciesId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_DowModDowMod_Mods_DependentsId",
                        column: x => x.DependentsId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "GameRules",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    ModId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsWinCondition = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRules_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    ModId = table.Column<int>(type: "INTEGER", nullable: false),
                    Players = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ModId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Races_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_DowModDowMod_DependentsId",
                table: "DowModDowMod",
                column: "DependentsId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_GameRules_ModId",
                table: "GameRules",
                column: "ModId"
            );

            migrationBuilder.CreateIndex(name: "IX_Maps_ModId", table: "Maps", column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_IsVanilla_ModFolder",
                table: "Mods",
                columns: new[] { "IsVanilla", "ModFolder" }
            );

            migrationBuilder.CreateIndex(name: "IX_Races_ModId", table: "Races", column: "ModId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "DowModDowMod");

            migrationBuilder.DropTable(name: "GameRules");

            migrationBuilder.DropTable(name: "Maps");

            migrationBuilder.DropTable(name: "Races");

            migrationBuilder.DropTable(name: "Mods");
        }
    }
}
