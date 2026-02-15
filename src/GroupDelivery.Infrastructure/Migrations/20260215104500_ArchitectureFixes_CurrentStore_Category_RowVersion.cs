using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    public partial class ArchitectureFixes_CurrentStore_Category_RowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StoreMenuCategories",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StoreMenuCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "StoreMenuItems",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "StoreMenuItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.DropIndex(
                name: "IX_StoreMenuCategories_StoreId",
                table: "StoreMenuCategories");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuCategories_StoreId_Name_IsDeleted",
                table: "StoreMenuCategories",
                columns: new[] { "StoreId", "Name", "IsDeleted" },
                unique: true);

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems");

            migrationBuilder.DropIndex(
                name: "IX_StoreMenuCategories_StoreId_Name_IsDeleted",
                table: "StoreMenuCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StoreMenuCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StoreMenuCategories");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "StoreMenuItems");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "StoreMenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuCategories_StoreId",
                table: "StoreMenuCategories",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "Id");
        }
    }
}
