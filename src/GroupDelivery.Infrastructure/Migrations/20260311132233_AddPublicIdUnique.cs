using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicIdUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbStore_PublicId",
                table: "tbStore",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbGroupOrder_PublicId",
                table: "tbGroupOrder",
                column: "PublicId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbStore_PublicId",
                table: "tbStore");

            migrationBuilder.DropIndex(
                name: "IX_tbGroupOrder_PublicId",
                table: "tbGroupOrder");
        }
    }
}
