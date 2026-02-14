using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorStoreStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HolidayEndDate",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "HolidayStartDate",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "IsAcceptingOrders",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "IsOnHoliday",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "IsOpenNow",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tbStore");

            migrationBuilder.RenameColumn(
                name: "CurrentStatus",
                table: "tbStore",
                newName: "OperationStatus");

            migrationBuilder.AddColumn<int>(
                name: "AccountStatus",
                table: "tbStore",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "tbStore");

            migrationBuilder.RenameColumn(
                name: "OperationStatus",
                table: "tbStore",
                newName: "CurrentStatus");

            migrationBuilder.AddColumn<DateTime>(
                name: "HolidayEndDate",
                table: "tbStore",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HolidayStartDate",
                table: "tbStore",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptingOrders",
                table: "tbStore",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnHoliday",
                table: "tbStore",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenNow",
                table: "tbStore",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "tbStore",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
