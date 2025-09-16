using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raffle.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfTicketsAndThemeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfTickets",
                table: "Raffles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ThemeId",
                table: "Raffles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfTickets",
                table: "Raffles");

            migrationBuilder.DropColumn(
                name: "ThemeId",
                table: "Raffles");
        }
    }
}
