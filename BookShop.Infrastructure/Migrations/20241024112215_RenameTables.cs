using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEntity_AuthorEntity_AuthorId",
                table: "BookEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookEntity",
                table: "BookEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorEntity",
                table: "AuthorEntity");

            migrationBuilder.EnsureSchema(
                name: "shop");

            migrationBuilder.RenameTable(
                name: "BookEntity",
                newName: "Books",
                newSchema: "shop");

            migrationBuilder.RenameTable(
                name: "AuthorEntity",
                newName: "Authors",
                newSchema: "shop");

            migrationBuilder.RenameIndex(
                name: "IX_BookEntity_AuthorId",
                schema: "shop",
                table: "Books",
                newName: "IX_Books_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                schema: "shop",
                table: "Books",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                schema: "shop",
                table: "Authors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                schema: "shop",
                table: "Books",
                column: "AuthorId",
                principalSchema: "shop",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                schema: "shop",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                schema: "shop",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                schema: "shop",
                table: "Authors");

            migrationBuilder.RenameTable(
                name: "Books",
                schema: "shop",
                newName: "BookEntity");

            migrationBuilder.RenameTable(
                name: "Authors",
                schema: "shop",
                newName: "AuthorEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Books_AuthorId",
                table: "BookEntity",
                newName: "IX_BookEntity_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookEntity",
                table: "BookEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorEntity",
                table: "AuthorEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEntity_AuthorEntity_AuthorId",
                table: "BookEntity",
                column: "AuthorId",
                principalTable: "AuthorEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
