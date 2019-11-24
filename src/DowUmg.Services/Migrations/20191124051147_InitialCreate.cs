using Microsoft.EntityFrameworkCore.Migrations;

namespace DowUmg.Services.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Difficulty = table.Column<int>(nullable: true),
                    ResourceRate = table.Column<int>(nullable: true),
                    Speed = table.Column<int>(nullable: true),
                    StartingResources = table.Column<int>(nullable: true),
                    RandomPosition = table.Column<bool>(nullable: true),
                    RandomTeams = table.Column<bool>(nullable: true),
                    Sharing = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomRule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameInfoId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomRule", x => new { x.GameInfoId, x.Id });
                    table.ForeignKey(
                        name: "FK_CustomRule_GameInfo_GameInfoId",
                        column: x => x.GameInfoId,
                        principalTable: "GameInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Players = table.Column<byte>(nullable: false),
                    Size = table.Column<byte>(nullable: false),
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
                name: "GameInfoRule",
                columns: table => new
                {
                    RuleId = table.Column<int>(nullable: false),
                    InfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameInfoRule", x => new { x.InfoId, x.RuleId });
                    table.ForeignKey(
                        name: "FK_GameInfoRule_GameInfo_InfoId",
                        column: x => x.InfoId,
                        principalTable: "GameInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameInfoRule_GameRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "GameRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Army",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    CampaignId = table.Column<int>(nullable: false),
                    Race = table.Column<string>(nullable: false),
                    AllianceId = table.Column<int>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Army", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioPlayers",
                columns: table => new
                {
                    ScenarioId = table.Column<int>(nullable: false),
                    ArmyId = table.Column<int>(nullable: false),
                    Position = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioPlayers", x => new { x.ArmyId, x.ScenarioId });
                    table.ForeignKey(
                        name: "FK_ScenarioPlayers_Army_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Army",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alliance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    CampaignId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alliance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scenario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    InheritedWinConditions = table.Column<bool>(nullable: false),
                    InheritedGameRules = table.Column<bool>(nullable: false),
                    InheritedCustomRules = table.Column<bool>(nullable: false),
                    InheritedGameOptions = table.Column<bool>(nullable: false),
                    InfoId = table.Column<int>(nullable: false),
                    MapId = table.Column<int>(nullable: false),
                    ManyMatchupCampaignId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scenario_GameInfo_InfoId",
                        column: x => x.InfoId,
                        principalTable: "GameInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scenario_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ScenarioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaign_Scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alliance_CampaignId",
                table: "Alliance",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Army_AllianceId",
                table: "Army",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_Army_CampaignId",
                table: "Army",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_ScenarioId",
                table: "Campaign",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_GameInfoRule_RuleId",
                table: "GameInfoRule",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRules_ModId",
                table: "GameRules",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_ModId",
                table: "Maps",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_InfoId",
                table: "Scenario",
                column: "InfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_ManyMatchupCampaignId",
                table: "Scenario",
                column: "ManyMatchupCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_MapId",
                table: "Scenario",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioPlayers_ScenarioId",
                table: "ScenarioPlayers",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Army_Campaign_CampaignId",
                table: "Army",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Army_Alliance_AllianceId",
                table: "Army",
                column: "AllianceId",
                principalTable: "Alliance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioPlayers_Scenario_ScenarioId",
                table: "ScenarioPlayers",
                column: "ScenarioId",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alliance_Campaign_CampaignId",
                table: "Alliance",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenario_Campaign_ManyMatchupCampaignId",
                table: "Scenario",
                column: "ManyMatchupCampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenario_Campaign_ManyMatchupCampaignId",
                table: "Scenario");

            migrationBuilder.DropTable(
                name: "CustomRule");

            migrationBuilder.DropTable(
                name: "GameInfoRule");

            migrationBuilder.DropTable(
                name: "ScenarioPlayers");

            migrationBuilder.DropTable(
                name: "GameRules");

            migrationBuilder.DropTable(
                name: "Army");

            migrationBuilder.DropTable(
                name: "Alliance");

            migrationBuilder.DropTable(
                name: "Campaign");

            migrationBuilder.DropTable(
                name: "Scenario");

            migrationBuilder.DropTable(
                name: "GameInfo");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Mods");
        }
    }
}
