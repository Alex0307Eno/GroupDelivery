using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerUserIdToGroupOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerUserId",
                table: "tbGroupOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbGroupOrder_OwnerUserId",
                table: "tbGroupOrder",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbGroupOrder_tbUser_OwnerUserId",
                table: "tbGroupOrder",
                column: "OwnerUserId",
                principalTable: "tbUser",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbGroupOrder_tbUser_OwnerUserId",
                table: "tbGroupOrder");

            migrationBuilder.DropIndex(
                name: "IX_tbGroupOrder_OwnerUserId",
                table: "tbGroupOrder");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "tbGroupOrder");
        }
    }
}
