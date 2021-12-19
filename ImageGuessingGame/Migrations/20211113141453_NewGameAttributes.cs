using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class NewGameAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Guesser_GuessersId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "GuessersId",
                table: "Games",
                newName: "GuesserId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GuessersId",
                table: "Games",
                newName: "IX_Games_GuesserId");

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Guesser_GuesserId",
                table: "Games",
                column: "GuesserId",
                principalTable: "Guesser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Guesser_GuesserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "GuesserId",
                table: "Games",
                newName: "GuessersId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GuesserId",
                table: "Games",
                newName: "IX_Games_GuessersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Guesser_GuessersId",
                table: "Games",
                column: "GuessersId",
                principalTable: "Guesser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
