using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RebuildStoreStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuImageUrl",
                table: "tbStore");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "tbStore",
                newName: "Landline");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "tbStore",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "ClosedDays",
                table: "tbStore",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "tbStore",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpenTime",
                table: "tbStore",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "ClosedDays",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "tbStore");

            migrationBuilder.RenameColumn(
                name: "Landline",
                table: "tbStore",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "MenuImageUrl",
                table: "tbStore",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
