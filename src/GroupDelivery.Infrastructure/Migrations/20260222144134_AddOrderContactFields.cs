using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderContactFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GroupOrderId",
                table: "Orders",
                column: "GroupOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_tbGroupOrder_GroupOrderId",
                table: "Orders",
                column: "GroupOrderId",
                principalTable: "tbGroupOrder",
                principalColumn: "GroupOrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_tbGroupOrder_GroupOrderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_GroupOrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Orders");
        }
    }
}
