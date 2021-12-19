using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class Oracle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Index_Oracle_OracleId",
                table: "Index");

            migrationBuilder.DropForeignKey(
                name: "FK_Oracle_Games_GameId",
                table: "Oracle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Oracle",
                table: "Oracle");

            migrationBuilder.RenameTable(
                name: "Oracle",
                newName: "Oracles");

            migrationBuilder.RenameIndex(
                name: "IX_Oracle_GameId",
                table: "Oracles",
                newName: "IX_Oracles_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Oracles",
                table: "Oracles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Index_Oracles_OracleId",
                table: "Index",
                column: "OracleId",
                principalTable: "Oracles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Oracles_Games_GameId",
                table: "Oracles",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Index_Oracles_OracleId",
                table: "Index");

            migrationBuilder.DropForeignKey(
                name: "FK_Oracles_Games_GameId",
                table: "Oracles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Oracles",
                table: "Oracles");

            migrationBuilder.RenameTable(
                name: "Oracles",
                newName: "Oracle");

            migrationBuilder.RenameIndex(
                name: "IX_Oracles_GameId",
                table: "Oracle",
                newName: "IX_Oracle_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Oracle",
                table: "Oracle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Index_Oracle_OracleId",
                table: "Index",
                column: "OracleId",
                principalTable: "Oracle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Oracle_Games_GameId",
                table: "Oracle",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
