using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Infrastructure.Migrations
{
    public partial class AuthorRatingsEntity_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorRatings",
                schema: "shop",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorRatings", x => x.Id);
                    table.CheckConstraint("CK_AuthorRatings_CK_Rating_Score_Valid", "[Score] >= 1 AND [Score] <= 5");
                    table.ForeignKey(
                        name: "FK_AuthorRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorRatings_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "shop",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRatings_AuthorId",
                schema: "shop",
                table: "AuthorRatings",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRatings_UserId_AuthorId",
                schema: "shop",
                table: "AuthorRatings",
                columns: new[] { "UserId", "AuthorId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorRatings",
                schema: "shop");
        }
    }
}
