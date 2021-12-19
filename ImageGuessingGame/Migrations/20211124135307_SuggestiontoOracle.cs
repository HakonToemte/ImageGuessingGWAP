using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class SuggestiontoOracle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SuggestionId",
                table: "Oracles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Oracles_SuggestionId",
                table: "Oracles",
                column: "SuggestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Oracles_Suggestions_SuggestionId",
                table: "Oracles",
                column: "SuggestionId",
                principalTable: "Suggestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oracles_Suggestions_SuggestionId",
                table: "Oracles");

            migrationBuilder.DropIndex(
                name: "IX_Oracles_SuggestionId",
                table: "Oracles");

            migrationBuilder.DropColumn(
                name: "SuggestionId",
                table: "Oracles");
        }
    }
}
