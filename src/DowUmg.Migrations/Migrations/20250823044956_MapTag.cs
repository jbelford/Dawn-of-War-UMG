using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DowUmg.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class MapTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Maps",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Maps");
        }
    }
}
