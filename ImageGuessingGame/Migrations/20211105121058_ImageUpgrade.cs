using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class ImageUpgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guesser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guesser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guesser_loginusers_UserId",
                        column: x => x.UserId,
                        principalTable: "loginusers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GuessersId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Guesser_GuessersId",
                        column: x => x.GuessersId,
                        principalTable: "Guesser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gameplay",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gameplay", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Gameplay_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oracle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oracle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oracle_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GuessersId",
                table: "Games",
                column: "GuessersId");

            migrationBuilder.CreateIndex(
                name: "IX_Guesser_UserId",
                table: "Guesser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Oracle_GameId",
                table: "Oracle",
                column: "GameId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gameplay");

            migrationBuilder.DropTable(
                name: "Oracle");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Guesser");
        }
    }
}
