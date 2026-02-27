using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuOptionStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreMenuItemOptionGroups",
                columns: table => new
                {
                    StoreMenuItemOptionGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreMenuItemId = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMenuItemOptionGroups", x => x.StoreMenuItemOptionGroupId);
                    table.ForeignKey(
                        name: "FK_StoreMenuItemOptionGroups_StoreMenuItems_StoreMenuItemId",
                        column: x => x.StoreMenuItemId,
                        principalTable: "StoreMenuItems",
                        principalColumn: "StoreMenuItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreMenuItemOptions",
                columns: table => new
                {
                    StoreMenuItemOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionGroupId = table.Column<int>(type: "int", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceAdjust = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMenuItemOptions", x => x.StoreMenuItemOptionId);
                    table.ForeignKey(
                        name: "FK_StoreMenuItemOptions_StoreMenuItemOptionGroups_OptionGroupId",
                        column: x => x.OptionGroupId,
                        principalTable: "StoreMenuItemOptionGroups",
                        principalColumn: "StoreMenuItemOptionGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuItemOptionGroups_StoreMenuItemId",
                table: "StoreMenuItemOptionGroups",
                column: "StoreMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMenuItemOptions_OptionGroupId",
                table: "StoreMenuItemOptions",
                column: "OptionGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreMenuItemOptions");

            migrationBuilder.DropTable(
                name: "StoreMenuItemOptionGroups");
        }
    }
}
