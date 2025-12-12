using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRO.SKM.Docu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAiConfigGuideProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GenerateGuide",
                table: "RepositoryAiConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GenerateGuideFileCount",
                table: "RepositoryAiConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GenerateGuideFileExtensions",
                table: "RepositoryAiConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GenerateGuideFilePickMethod",
                table: "RepositoryAiConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GenerateGuidePrompt",
                table: "RepositoryAiConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenerateGuide",
                table: "RepositoryAiConfigurations");

            migrationBuilder.DropColumn(
                name: "GenerateGuideFileCount",
                table: "RepositoryAiConfigurations");

            migrationBuilder.DropColumn(
                name: "GenerateGuideFileExtensions",
                table: "RepositoryAiConfigurations");

            migrationBuilder.DropColumn(
                name: "GenerateGuideFilePickMethod",
                table: "RepositoryAiConfigurations");

            migrationBuilder.DropColumn(
                name: "GenerateGuidePrompt",
                table: "RepositoryAiConfigurations");
        }
    }
}
