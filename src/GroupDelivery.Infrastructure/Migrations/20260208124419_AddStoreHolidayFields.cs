using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreHolidayFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IsOnHoliday",
                table: "tbStore",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HolidayEndDate",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "HolidayStartDate",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "IsOnHoliday",
                table: "tbStore");
        }
    }
}
