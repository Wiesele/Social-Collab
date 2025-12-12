using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRO.SKM.Docu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedStyleguideToRepo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StyleGuide",
                table: "Repositories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StyleGuide",
                table: "Repositories");
        }
    }
}
