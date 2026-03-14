using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicIdToStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "tbStore",
                newName: "StorePublicId");

            migrationBuilder.RenameIndex(
                name: "IX_tbStore_PublicId",
                table: "tbStore",
                newName: "IX_tbStore_StorePublicId");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "tbGroupOrder",
                newName: "GroupOrderPublicId");

            migrationBuilder.RenameIndex(
                name: "IX_tbGroupOrder_PublicId",
                table: "tbGroupOrder",
                newName: "IX_tbGroupOrder_GroupOrderPublicId");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "StoreMenuItems",
                newName: "StoreMenuItemPublicId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreMenuItems_PublicId",
                table: "StoreMenuItems",
                newName: "IX_StoreMenuItems_StoreMenuItemPublicId");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "Orders",
                newName: "OrderPublicId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PublicId",
                table: "Orders",
                newName: "IX_Orders_OrderPublicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorePublicId",
                table: "tbStore",
                newName: "PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_tbStore_StorePublicId",
                table: "tbStore",
                newName: "IX_tbStore_PublicId");

            migrationBuilder.RenameColumn(
                name: "GroupOrderPublicId",
                table: "tbGroupOrder",
                newName: "PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_tbGroupOrder_GroupOrderPublicId",
                table: "tbGroupOrder",
                newName: "IX_tbGroupOrder_PublicId");

            migrationBuilder.RenameColumn(
                name: "StoreMenuItemPublicId",
                table: "StoreMenuItems",
                newName: "PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreMenuItems_StoreMenuItemPublicId",
                table: "StoreMenuItems",
                newName: "IX_StoreMenuItems_PublicId");

            migrationBuilder.RenameColumn(
                name: "OrderPublicId",
                table: "Orders",
                newName: "PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_OrderPublicId",
                table: "Orders",
                newName: "IX_Orders_PublicId");
        }
    }
}
