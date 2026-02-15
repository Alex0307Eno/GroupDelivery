using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeCategoryNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "StoreMenuItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
