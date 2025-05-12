using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    public partial class AuthorEntityShouldBeuniqueAmoungNotDeletedAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_Name_Surname",
                schema: "shop",
                table: "Authors");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Name_Surname",
                schema: "shop",
                table: "Authors",
                columns: new[] { "Name", "Surname" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_Name_Surname",
                schema: "shop",
                table: "Authors");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Name_Surname",
                schema: "shop",
                table: "Authors",
                columns: new[] { "Name", "Surname" },
                unique: true);
        }
    }
}
