using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    public partial class BookEntity_NewUniqlessRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_AuthorId_Title",
                schema: "shop",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId_Title_ReleaseDate",
                schema: "shop",
                table: "Books",
                columns: new[] { "AuthorId", "Title", "ReleaseDate" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_AuthorId_Title_ReleaseDate",
                schema: "shop",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId_Title",
                schema: "shop",
                table: "Books",
                columns: new[] { "AuthorId", "Title" },
                unique: true);
        }
    }
}
