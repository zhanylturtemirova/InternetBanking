using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InternetBanking.Migrations
{
    public partial class AddTemplateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InnerTransferId = table.Column<int>(type: "int", nullable: true),
                    InterBankTransferId = table.Column<int>(type: "int", nullable: true),
                    TempalteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateDiscription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_InnerTransfers_InnerTransferId",
                        column: x => x.InnerTransferId,
                        principalTable: "InnerTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Templates_InterBankTransfers_InterBankTransferId",
                        column: x => x.InterBankTransferId,
                        principalTable: "InterBankTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_InnerTransferId",
                table: "Templates",
                column: "InnerTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_InterBankTransferId",
                table: "Templates",
                column: "InterBankTransferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
