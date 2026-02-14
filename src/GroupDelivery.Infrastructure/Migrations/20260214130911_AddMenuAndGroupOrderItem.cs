using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuAndGroupOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreMenuCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMenuCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMenuCategories_tbStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "tbStore",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreMenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMenuItems_StoreMenuCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "StoreMenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupOrderItems",
                columns: table => new
                {
                    GroupOrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupOrderId = table.Column<int>(type: "int", nullable: false),
                    StoreMenuItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupOrderItems", x => x.GroupOrderItemId);
                    table.ForeignKey(
                        name: "FK_GroupOrderItems_StoreMenuItems_StoreMenuItemId",
                        column: x => x.StoreMenuItemId,
                        principalTable: "StoreMenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupOrderItems_tbGroupOrder_GroupOrderId",
                        column: x => x.GroupOrderId,
                        principalTable: "tbGroupOrder",
                        principalColumn: "GroupOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOrderItems_tbUser_UserId",
                        column: x => x.UserId,
                        principalTable: "tbUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupOrderItems_GroupOrderId",
                table: "GroupOrderItems",
                column: "GroupOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOrderItems_StoreMenuItemId",
                table: "GroupOrderItems",
                column: "StoreMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupOrderItems_UserId",
                table: "GroupOrderItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuCategories_StoreId",
                table: "StoreMenuCategories",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuItems_CategoryId",
                table: "StoreMenuItems",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupOrderItems");

            migrationBuilder.DropTable(
                name: "StoreMenuItems");

            migrationBuilder.DropTable(
                name: "StoreMenuCategories");
        }
    }
}
