using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class labeltooracle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Oracles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                table: "Oracles");
        }
    }
}
