using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class IncorrectCountOnGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncorrectCount",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncorrectCount",
                table: "Games");
        }
    }
}
