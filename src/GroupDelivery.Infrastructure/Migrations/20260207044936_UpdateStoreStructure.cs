using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoreStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "tbStore",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "tbStore",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "tbStore",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptingOrders",
                table: "tbStore",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MenuImageUrl",
                table: "tbStore",
                type: "nvarchar(255)",
                maxLength: 255,
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "IsAcceptingOrders",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "MenuImageUrl",
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

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "tbStore",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
