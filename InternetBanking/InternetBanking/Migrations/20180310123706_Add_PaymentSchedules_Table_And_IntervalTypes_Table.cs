using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InternetBanking.Migrations
{
    public partial class Add_PaymentSchedules_Table_And_IntervalTypes_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TypeOfTransfers_TypeOfTransferId",
                table: "Templates");

            migrationBuilder.AlterColumn<int>(
                name: "TypeOfTransferId",
                table: "Templates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "IntervalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IntervalCode = table.Column<int>(type: "int", nullable: false),
                    IntervalName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Finish = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntervalTypeId = table.Column<int>(type: "int", nullable: false),
                    NextPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentSchedules_IntervalTypes_IntervalTypeId",
                        column: x => x.IntervalTypeId,
                        principalTable: "IntervalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentSchedules_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedules_IntervalTypeId",
                table: "PaymentSchedules",
                column: "IntervalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedules_TemplateId",
                table: "PaymentSchedules",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TypeOfTransfers_TypeOfTransferId",
                table: "Templates",
                column: "TypeOfTransferId",
                principalTable: "TypeOfTransfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TypeOfTransfers_TypeOfTransferId",
                table: "Templates");

            migrationBuilder.DropTable(
                name: "PaymentSchedules");

            migrationBuilder.DropTable(
                name: "IntervalTypes");

            migrationBuilder.AlterColumn<int>(
                name: "TypeOfTransferId",
                table: "Templates",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TypeOfTransfers_TypeOfTransferId",
                table: "Templates",
                column: "TypeOfTransferId",
                principalTable: "TypeOfTransfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
