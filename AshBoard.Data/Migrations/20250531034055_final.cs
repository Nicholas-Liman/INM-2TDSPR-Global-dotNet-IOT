using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "NivelCO2",
                table: "Alertas",
                type: "BINARY_FLOAT",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Temperatura",
                table: "Alertas",
                type: "BINARY_FLOAT",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NivelCO2",
                table: "Alertas");

            migrationBuilder.DropColumn(
                name: "Temperatura",
                table: "Alertas");
        }
    }
}
