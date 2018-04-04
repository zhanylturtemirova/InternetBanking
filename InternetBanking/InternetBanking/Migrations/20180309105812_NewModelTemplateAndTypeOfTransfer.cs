using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InternetBanking.Migrations
{
    public partial class NewModelTemplateAndTypeOfTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_InnerTransfers_InnerTransferId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_InterBankTransfers_InterBankTransferId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_InnerTransferId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_InterBankTransferId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "InnerTransferId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "InterBankTransferId",
                table: "Templates");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountReceiverId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountSenderId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Templates",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentCodeId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentСodeId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReciverName",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeOfTransferId",
                table: "Templates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TypeOfTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfTransfers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_AccountReceiverId",
                table: "Templates",
                column: "AccountReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_AccountSenderId",
                table: "Templates",
                column: "AccountSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_BankId",
                table: "Templates",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_PaymentСodeId",
                table: "Templates",
                column: "PaymentСodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TypeOfTransferId",
                table: "Templates",
                column: "TypeOfTransferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Accounts_AccountReceiverId",
                table: "Templates",
                column: "AccountReceiverId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Accounts_AccountSenderId",
                table: "Templates",
                column: "AccountSenderId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Banks_BankId",
                table: "Templates",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_PaymentСodies_PaymentСodeId",
                table: "Templates",
                column: "PaymentСodeId",
                principalTable: "PaymentСodies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TypeOfTransfers_TypeOfTransferId",
                table: "Templates",
                column: "TypeOfTransferId",
                principalTable: "TypeOfTransfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Accounts_AccountReceiverId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Accounts_AccountSenderId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Banks_BankId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_PaymentСodies_PaymentСodeId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TypeOfTransfers_TypeOfTransferId",
                table: "Templates");

            migrationBuilder.DropTable(
                name: "TypeOfTransfers");

            migrationBuilder.DropIndex(
                name: "IX_Templates_AccountReceiverId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_AccountSenderId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_BankId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_PaymentСodeId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_TypeOfTransferId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "AccountReceiverId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "AccountSenderId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "PaymentCodeId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "PaymentСodeId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "ReciverName",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "TypeOfTransferId",
                table: "Templates");

            migrationBuilder.AddColumn<int>(
                name: "InnerTransferId",
                table: "Templates",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterBankTransferId",
                table: "Templates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_InnerTransferId",
                table: "Templates",
                column: "InnerTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_InterBankTransferId",
                table: "Templates",
                column: "InterBankTransferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_InnerTransfers_InnerTransferId",
                table: "Templates",
                column: "InnerTransferId",
                principalTable: "InnerTransfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_InterBankTransfers_InterBankTransferId",
                table: "Templates",
                column: "InterBankTransferId",
                principalTable: "InterBankTransfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
