using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyStoreStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreClosedDates");

            migrationBuilder.DropTable(
                name: "StoreWeeklyClosedDays");

            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "MinOrderAmount",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "Notice",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "OperationStatus",
                table: "tbStore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountStatus",
                table: "tbStore",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "tbStore",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "tbStore",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "tbStore",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderAmount",
                table: "tbStore",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notice",
                table: "tbStore",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpenTime",
                table: "tbStore",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationStatus",
                table: "tbStore",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StoreClosedDates",
                columns: table => new
                {
                    StoreClosedDateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreClosedDates", x => x.StoreClosedDateId);
                    table.ForeignKey(
                        name: "FK_StoreClosedDates_tbStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "tbStore",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreWeeklyClosedDays",
                columns: table => new
                {
                    StoreWeeklyClosedDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_StoreClosedDates_StoreId",
                table: "StoreClosedDates",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreWeeklyClosedDays_StoreId",
                table: "StoreWeeklyClosedDays",
                column: "StoreId");
        }
    }
}
