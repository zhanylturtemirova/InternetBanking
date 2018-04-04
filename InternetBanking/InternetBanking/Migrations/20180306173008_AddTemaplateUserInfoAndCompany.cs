using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InternetBanking.Migrations
{
    public partial class AddTemaplateUserInfoAndCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CompanyId",
                table: "Templates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_UserInfoId",
                table: "Templates",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Companies_CompanyId",
                table: "Templates",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_UserInfo_UserInfoId",
                table: "Templates",
                column: "UserInfoId",
                principalTable: "UserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Companies_CompanyId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_UserInfo_UserInfoId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_CompanyId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_UserInfoId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Templates");
        }
    }
}
