using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageGuessingGame.Migrations
{
    public partial class IndexCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfSlices",
                table: "Oracle",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Index",
                columns: table => new
                {
                    IndexId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    index = table.Column<int>(type: "INTEGER", nullable: false),
                    OracleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OracleId1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Index", x => x.IndexId);
                    table.ForeignKey(
                        name: "FK_Index_Oracle_OracleId",
                        column: x => x.OracleId,
                        principalTable: "Oracle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Index_Oracle_OracleId1",
                        column: x => x.OracleId1,
                        principalTable: "Oracle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Index_OracleId",
                table: "Index",
                column: "OracleId");

            migrationBuilder.CreateIndex(
                name: "IX_Index_OracleId1",
                table: "Index",
                column: "OracleId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Index");

            migrationBuilder.DropColumn(
                name: "NumberOfSlices",
                table: "Oracle");
        }
    }
}
