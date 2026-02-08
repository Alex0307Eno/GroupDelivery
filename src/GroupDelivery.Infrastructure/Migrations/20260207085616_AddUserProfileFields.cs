using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreAddress",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "StorePhone",
                table: "tbUser");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "tbUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "tbUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodPreference",
                table: "tbUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "tbUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotifyOptIn",
                table: "tbUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "City",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "FoodPreference",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "tbUser");

            migrationBuilder.DropColumn(
                name: "NotifyOptIn",
                table: "tbUser");

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
        }
    }
}
