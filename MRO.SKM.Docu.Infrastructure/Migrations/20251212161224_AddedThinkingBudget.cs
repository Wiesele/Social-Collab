using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRO.SKM.Docu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedThinkingBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenerateGuideThinkingBudget",
                table: "RepositoryAiConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenerateGuideThinkingBudget",
                table: "RepositoryAiConfigurations");
        }
    }
}
