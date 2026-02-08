using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreWeeklyClosedDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreWeeklyClosedDays",
                columns: table => new
                {
                    StoreWeeklyClosedDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreWeeklyClosedDays", x => x.StoreWeeklyClosedDayId);
                    table.ForeignKey(
                        name: "FK_StoreWeeklyClosedDays_tbStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "tbStore",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreWeeklyClosedDays_StoreId",
                table: "StoreWeeklyClosedDays",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreWeeklyClosedDays");
        }
    }
}
