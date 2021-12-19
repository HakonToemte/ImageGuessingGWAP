using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class Multiplayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Guesser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameMode",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MultiplayerOpenLobby",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MultiPlayerGuess",
                columns: table => new
                {
                    MultiPlayerGuessId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoginUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Guess = table.Column<string>(type: "TEXT", nullable: true),
                    OracleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiPlayerGuess", x => x.MultiPlayerGuessId);
                    table.ForeignKey(
                        name: "FK_MultiPlayerGuess_Oracles_OracleId",
                        column: x => x.OracleId,
                        principalTable: "Oracles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guesser_GameId",
                table: "Guesser",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiPlayerGuess_OracleId",
                table: "MultiPlayerGuess",
                column: "OracleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guesser_Games_GameId",
                table: "Guesser",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guesser_Games_GameId",
                table: "Guesser");

            migrationBuilder.DropTable(
                name: "MultiPlayerGuess");

            migrationBuilder.DropIndex(
                name: "IX_Guesser_GameId",
                table: "Guesser");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Guesser");

            migrationBuilder.DropColumn(
                name: "GameMode",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MultiplayerOpenLobby",
                table: "Games");
        }
    }
}
