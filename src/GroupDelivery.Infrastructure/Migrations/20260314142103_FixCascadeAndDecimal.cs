using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeAndDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryRules_tbStore_StoreId",
                table: "DeliveryRules");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupOrderItems_StoreMenuItems_StoreMenuItemId",
                table: "GroupOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupOrderItems_tbGroupOrder_GroupOrderId",
                table: "GroupOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupOrderItems_tbUser_UserId",
                table: "GroupOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemOptions_OrderItems_OrderItemId",
                table: "OrderItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_StoreMenuItems_StoreMenuItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_tbGroupOrder_GroupOrderId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_tbUser_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuCategories_tbStore_StoreId",
                table: "StoreMenuCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItemOptionGroups_StoreMenuItems_StoreMenuItemId",
                table: "StoreMenuItemOptionGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItemOptions_StoreMenuItemOptionGroups_OptionGroupId",
                table: "StoreMenuItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_tbGroupOrder_tbStore_StoreId",
                table: "tbGroupOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_tbGroupOrder_tbUser_OwnerUserId",
                table: "tbGroupOrder");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "tbStore",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreMenuItemOptionGroupId",
                table: "StoreMenuItemOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreMenuItemId1",
                table: "StoreMenuItemOptionGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuItems_StoreId",
                table: "StoreMenuItems",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuItemOptions_StoreMenuItemOptionGroupId",
                table: "StoreMenuItemOptions",
                column: "StoreMenuItemOptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuItemOptionGroups_StoreMenuItemId1",
                table: "StoreMenuItemOptionGroups",
                column: "StoreMenuItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryRules_tbStore_StoreId",
                table: "DeliveryRules",
                column: "StoreId",
                principalTable: "tbStore",
                principalColumn: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOrderItems_StoreMenuItems_StoreMenuItemId",
                table: "GroupOrderItems",
                column: "StoreMenuItemId",
                principalTable: "StoreMenuItems",
                principalColumn: "StoreMenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOrderItems_tbGroupOrder_GroupOrderId",
                table: "GroupOrderItems",
                column: "GroupOrderId",
                principalTable: "tbGroupOrder",
                principalColumn: "GroupOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOrderItems_tbUser_UserId",
                table: "GroupOrderItems",
                column: "UserId",
                principalTable: "tbUser",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemOptions_OrderItems_OrderItemId",
                table: "OrderItemOptions",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_StoreMenuItems_StoreMenuItemId",
                table: "OrderItems",
                column: "StoreMenuItemId",
                principalTable: "StoreMenuItems",
                principalColumn: "StoreMenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_tbGroupOrder_GroupOrderId",
                table: "Orders",
                column: "GroupOrderId",
                principalTable: "tbGroupOrder",
                principalColumn: "GroupOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_tbUser_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "tbUser",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuCategories_tbStore_StoreId",
                table: "StoreMenuCategories",
                column: "StoreId",
                principalTable: "tbStore",
                principalColumn: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItemOptionGroups_StoreMenuItems_StoreMenuItemId",
                table: "StoreMenuItemOptionGroups",
                column: "StoreMenuItemId",
                principalTable: "StoreMenuItems",
                principalColumn: "StoreMenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItemOptionGroups_StoreMenuItems_StoreMenuItemId1",
                table: "StoreMenuItemOptionGroups",
                column: "StoreMenuItemId1",
                principalTable: "StoreMenuItems",
                principalColumn: "StoreMenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItemOptions_StoreMenuItemOptionGroups_OptionGroupId",
                table: "StoreMenuItemOptions",
                column: "OptionGroupId",
                principalTable: "StoreMenuItemOptionGroups",
                principalColumn: "StoreMenuItemOptionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItemOptions_StoreMenuItemOptionGroups_StoreMenuItemOptionGroupId",
                table: "StoreMenuItemOptions",
                column: "StoreMenuItemOptionGroupId",
                principalTable: "StoreMenuItemOptionGroups",
                principalColumn: "StoreMenuItemOptionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "StoreMenuCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_tbStore_StoreId",
                table: "StoreMenuItems",
                column: "StoreId",
                principalTable: "tbStore",
                principalColumn: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbGroupOrder_tbStore_StoreId",
                table: "tbGroupOrder",
                column: "StoreId",
                principalTable: "tbStore",
                principalColumn: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbGroupOrder_tbUser_OwnerUserId",
                table: "tbGroupOrder",
                column: "OwnerUserId",
                principalTable: "tbUser",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryRules_tbStore_StoreId",
                table: "DeliveryRules");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupOrderItems_StoreMenuItems_StoreMenuItemId",
                table: "GroupOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupOrderItems_tbGroupOrder_GroupOrderId",
                table: "GroupOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupOrderItems_tbUser_UserId",
                table: "GroupOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemOptions_OrderItems_OrderItemId",
                table: "OrderItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_StoreMenuItems_StoreMenuItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_tbGroupOrder_GroupOrderId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_tbUser_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuCategories_tbStore_StoreId",
                table: "StoreMenuCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItemOptionGroups_StoreMenuItems_StoreMenuItemId",
                table: "StoreMenuItemOptionGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItemOptionGroups_StoreMenuItems_StoreMenuItemId1",
                table: "StoreMenuItemOptionGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItemOptions_StoreMenuItemOptionGroups_OptionGroupId",
                table: "StoreMenuItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItemOptions_StoreMenuItemOptionGroups_StoreMenuItemOptionGroupId",
                table: "StoreMenuItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreMenuItems_tbStore_StoreId",
                table: "StoreMenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_tbGroupOrder_tbStore_StoreId",
                table: "tbGroupOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_tbGroupOrder_tbUser_OwnerUserId",
                table: "tbGroupOrder");

            migrationBuilder.DropIndex(
                name: "IX_StoreMenuItems_StoreId",
                table: "StoreMenuItems");

            migrationBuilder.DropIndex(
                name: "IX_StoreMenuItemOptions_StoreMenuItemOptionGroupId",
                table: "StoreMenuItemOptions");

            migrationBuilder.DropIndex(
                name: "IX_StoreMenuItemOptionGroups_StoreMenuItemId1",
                table: "StoreMenuItemOptionGroups");

            migrationBuilder.DropColumn(
                name: "City",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "StoreMenuItemOptionGroupId",
                table: "StoreMenuItemOptions");

            migrationBuilder.DropColumn(
                name: "StoreMenuItemId1",
                table: "StoreMenuItemOptionGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryRules_tbStore_StoreId",
                table: "DeliveryRules",
                column: "StoreId",
                principalTable: "tbStore",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOrderItems_StoreMenuItems_StoreMenuItemId",
                table: "GroupOrderItems",
                column: "StoreMenuItemId",
                principalTable: "StoreMenuItems",
                principalColumn: "StoreMenuItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOrderItems_tbGroupOrder_GroupOrderId",
                table: "GroupOrderItems",
                column: "GroupOrderId",
                principalTable: "tbGroupOrder",
                principalColumn: "GroupOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupOrderItems_tbUser_UserId",
                table: "GroupOrderItems",
                column: "UserId",
                principalTable: "tbUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemOptions_OrderItems_OrderItemId",
                table: "OrderItemOptions",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "OrderItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_StoreMenuItems_StoreMenuItemId",
                table: "OrderItems",
                column: "StoreMenuItemId",
                principalTable: "StoreMenuItems",
                principalColumn: "StoreMenuItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_tbGroupOrder_GroupOrderId",
                table: "Orders",
                column: "GroupOrderId",
                principalTable: "tbGroupOrder",
                principalColumn: "GroupOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_tbUser_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "tbUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuCategories_tbStore_StoreId",
                table: "StoreMenuCategories",
                column: "StoreId",
                principalTable: "tbStore",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItemOptionGroups_StoreMenuItems_StoreMenuItemId",
                table: "StoreMenuItemOptionGroups",
                column: "StoreMenuItemId",
                principalTable: "StoreMenuItems",
                principalColumn: "StoreMenuItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItemOptions_StoreMenuItemOptionGroups_OptionGroupId",
                table: "StoreMenuItemOptions",
                column: "OptionGroupId",
                principalTable: "StoreMenuItemOptionGroups",
                principalColumn: "StoreMenuItemOptionGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId",
                principalTable: "StoreMenuCategories",
                principalColumn: "StoreMenuCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbGroupOrder_tbStore_StoreId",
                table: "tbGroupOrder",
                column: "StoreId",
                principalTable: "tbStore",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbGroupOrder_tbUser_OwnerUserId",
                table: "tbGroupOrder",
                column: "OwnerUserId",
                principalTable: "tbUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
