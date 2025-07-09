using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameBasedProperties_For_All_Entites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateAt",
                schema: "shop",
                table: "BookRatings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                schema: "shop",
                table: "AuthorRatings",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "shop",
                table: "Carts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "shop",
                table: "Carts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "shop",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "shop",
                table: "BookRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "shop",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "shop",
                table: "AuthorRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "shop",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "shop",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "shop",
                table: "BookRatings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "shop",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "shop",
                table: "AuthorRatings");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "shop",
                table: "BookRatings",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "shop",
                table: "AuthorRatings",
                newName: "CreateAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "shop",
                table: "Carts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
