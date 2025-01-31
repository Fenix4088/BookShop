using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    public partial class DeleteAtPropertyForBooksAndAuthorEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                schema: "shop",
                table: "Books");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "shop",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                schema: "shop",
                table: "Authors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "shop",
                table: "Authors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                schema: "shop",
                table: "Books",
                column: "AuthorId",
                principalSchema: "shop",
                principalTable: "Authors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                schema: "shop",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "shop",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                schema: "shop",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "shop",
                table: "Authors");

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
    }
}
