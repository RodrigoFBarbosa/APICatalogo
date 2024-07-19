using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class NameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtcs_Categories_CategoryId",
                table: "Produtcs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtcs",
                table: "Produtcs");

            migrationBuilder.RenameTable(
                name: "Produtcs",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_Produtcs_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Produtcs");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Produtcs",
                newName: "IX_Produtcs_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtcs",
                table: "Produtcs",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtcs_Categories_CategoryId",
                table: "Produtcs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
