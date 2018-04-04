using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InternetBanking.Migrations
{
    public partial class AddFieldToInnerTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExchangeRateIdSecond",
                table: "InnerTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InnerTransfers_ExchangeRateIdSecond",
                table: "InnerTransfers",
                column: "ExchangeRateIdSecond");

            migrationBuilder.AddForeignKey(
                name: "FK_InnerTransfers_ExchangeRates_ExchangeRateIdSecond",
                table: "InnerTransfers",
                column: "ExchangeRateIdSecond",
                principalTable: "ExchangeRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InnerTransfers_ExchangeRates_ExchangeRateIdSecond",
                table: "InnerTransfers");

            migrationBuilder.DropIndex(
                name: "IX_InnerTransfers_ExchangeRateIdSecond",
                table: "InnerTransfers");

            migrationBuilder.DropColumn(
                name: "ExchangeRateIdSecond",
                table: "InnerTransfers");
        }
    }
}
