using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUseless : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbGroupOrder_tbUser_CreatorUserId",
                table: "tbGroupOrder");

            migrationBuilder.DropTable(
                name: "tbEmailLoginToken");

            migrationBuilder.DropTable(
                name: "tbGroupOrderItem");

            migrationBuilder.DropTable(
                name: "tbStoreProduct");

            migrationBuilder.DropIndex(
                name: "IX_tbGroupOrder_CreatorUserId",
                table: "tbGroupOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbEmailLoginToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbEmailLoginToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbGroupOrderItem",
                columns: table => new
                {
                    GroupOrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupOrderId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubtotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbGroupOrderItem", x => x.GroupOrderItemId);
                    table.ForeignKey(
                        name: "FK_tbGroupOrderItem_tbGroupOrder_GroupOrderId",
                        column: x => x.GroupOrderId,
                        principalTable: "tbGroupOrder",
                        principalColumn: "GroupOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbStoreProduct",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbStoreProduct", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_tbStoreProduct_tbStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "tbStore",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbGroupOrder_CreatorUserId",
                table: "tbGroupOrder",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbGroupOrderItem_GroupOrderId",
                table: "tbGroupOrderItem",
                column: "GroupOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tbStoreProduct_StoreId",
                table: "tbStoreProduct",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbGroupOrder_tbUser_CreatorUserId",
                table: "tbGroupOrder",
                column: "CreatorUserId",
                principalTable: "tbUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
