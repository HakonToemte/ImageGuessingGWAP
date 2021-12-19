using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class Suggestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SuggestionId",
                table: "Index",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Index_SuggestionId",
                table: "Index",
                column: "SuggestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Index_Suggestions_SuggestionId",
                table: "Index",
                column: "SuggestionId",
                principalTable: "Suggestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Index_Suggestions_SuggestionId",
                table: "Index");

            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_Index_SuggestionId",
                table: "Index");

            migrationBuilder.DropColumn(
                name: "SuggestionId",
                table: "Index");
        }
    }
}
