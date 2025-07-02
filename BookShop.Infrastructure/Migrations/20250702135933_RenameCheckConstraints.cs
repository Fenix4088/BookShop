using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_BookRating_Score_Valid",
                schema: "shop",
                table: "BookRatings",
                sql: "[Score] >= 1 AND [Score] <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AuthorRating_Score_Valid",
                schema: "shop",
                table: "AuthorRatings",
                sql: "[Score] >= 1 AND [Score] <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_BookRating_Score_Valid",
                schema: "shop",
                table: "BookRatings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AuthorRating_Score_Valid",
                schema: "shop",
                table: "AuthorRatings");
        }
    }
}
