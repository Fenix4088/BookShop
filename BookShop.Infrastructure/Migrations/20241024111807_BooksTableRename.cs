using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    public partial class BooksTableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEntity_Authors_AuthorId",
                table: "BookEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                table: "Authors");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "AuthorEntity");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEntity_AuthorEntity_AuthorId",
                table: "BookEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorEntity",
                table: "AuthorEntity");

            migrationBuilder.RenameTable(
                name: "AuthorEntity",
                newName: "Authors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                table: "Authors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEntity_Authors_AuthorId",
                table: "BookEntity",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
