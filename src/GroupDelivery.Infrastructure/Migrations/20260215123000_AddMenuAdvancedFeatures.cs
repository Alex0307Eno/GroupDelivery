using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    public partial class AddMenuAdvancedFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 新增菜單時間區段欄位
            migrationBuilder.AddColumn<TimeSpan>(
                name: "AvailableStartTime",
                table: "StoreMenuItems",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AvailableEndTime",
                table: "StoreMenuItems",
                type: "time",
                nullable: true);

            // 明確維持分類外鍵為 Restrict 策略
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

            migrationBuilder.DropColumn(
                name: "AvailableStartTime",
                table: "StoreMenuItems");

            migrationBuilder.DropColumn(
                name: "AvailableEndTime",
                table: "StoreMenuItems");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
