using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    public partial class BookUniqueIndex_By_Author_ANd_Title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_AuthorId",
                schema: "shop",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId_Title",
                schema: "shop",
                table: "Books",
                columns: new[] { "AuthorId", "Title" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_AuthorId_Title",
                schema: "shop",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                schema: "shop",
                table: "Books",
                column: "AuthorId");
        }
    }
}
