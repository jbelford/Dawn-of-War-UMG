using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DowUmg.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class GameRuleFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "GameRules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "GameRules");
        }
    }
}
