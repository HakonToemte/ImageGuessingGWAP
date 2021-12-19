using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class IndexSuggestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Index_Suggestions_SuggestionId",
                table: "Index");

            migrationBuilder.DropIndex(
                name: "IX_Index_SuggestionId",
                table: "Index");

            migrationBuilder.DropColumn(
                name: "SuggestionId",
                table: "Index");

            migrationBuilder.CreateTable(
                name: "IndexSuggestion",
                columns: table => new
                {
                    IndexSuggestionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    index = table.Column<int>(type: "INTEGER", nullable: false),
                    SuggestionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexSuggestion", x => x.IndexSuggestionId);
                    table.ForeignKey(
                        name: "FK_IndexSuggestion_Suggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "Suggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndexSuggestion_SuggestionId",
                table: "IndexSuggestion",
                column: "SuggestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndexSuggestion");

            migrationBuilder.AddColumn<Guid>(
                name: "SuggestionId",
                table: "Index",
                type: "TEXT",
                nullable: true);

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
    }
}
