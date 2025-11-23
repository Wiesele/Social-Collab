using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRO.SKM.Docu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRepoLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RepositoryLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepositoryLanguages_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryLanguages_RepositoryId",
                table: "RepositoryLanguages",
                column: "RepositoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepositoryLanguages");
        }
    }
}
