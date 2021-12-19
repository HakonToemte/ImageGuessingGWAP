using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class OracleImagepath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Index_Oracle_OracleId1",
                table: "Index");

            migrationBuilder.DropIndex(
                name: "IX_Index_OracleId1",
                table: "Index");

            migrationBuilder.DropColumn(
                name: "OracleId1",
                table: "Index");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Oracle",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Oracle");

            migrationBuilder.AddColumn<Guid>(
                name: "OracleId1",
                table: "Index",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Index_OracleId1",
                table: "Index",
                column: "OracleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Index_Oracle_OracleId1",
                table: "Index",
                column: "OracleId1",
                principalTable: "Oracle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
