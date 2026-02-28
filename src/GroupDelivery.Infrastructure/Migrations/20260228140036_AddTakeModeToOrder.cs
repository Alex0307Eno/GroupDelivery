using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTakeModeToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TakeMode",
                table: "tbGroupOrder");

            migrationBuilder.AddColumn<int>(
                name: "TakeMode",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TakeMode",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "TakeMode",
                table: "tbGroupOrder",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
