using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserprl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "tbUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "tbUser");
        }
    }
}
