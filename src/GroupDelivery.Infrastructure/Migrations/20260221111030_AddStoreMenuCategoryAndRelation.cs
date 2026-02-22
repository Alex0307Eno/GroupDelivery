using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreMenuCategoryAndRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StoreMenuCategories",
                newName: "StoreMenuCategoryId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StoreMenuCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "StoreMenuCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StoreMenuCategories");

            migrationBuilder.RenameColumn(
                name: "StoreMenuCategoryId",
                table: "StoreMenuCategories",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "Id");
        }
    }
}
