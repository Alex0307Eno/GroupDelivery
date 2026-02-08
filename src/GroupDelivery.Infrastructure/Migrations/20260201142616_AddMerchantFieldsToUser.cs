using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMerchantFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tbStore");

            migrationBuilder.AddColumn<decimal>(
                name: "Lat",
                table: "tbUser",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Lng",
                table: "tbUser",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreAddress",
                table: "tbUser",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "tbUser",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorePhone",
                table: "tbUser",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "tbStore",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "OwnerUserId",
                table: "tbStore",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "tbStore",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "StoreAddress",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "StorePhone",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "tbStore");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "tbStore");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "tbStore",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tbStore",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
