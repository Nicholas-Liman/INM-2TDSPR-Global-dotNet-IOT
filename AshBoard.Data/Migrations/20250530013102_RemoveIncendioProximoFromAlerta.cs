using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIncendioProximoFromAlerta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncendioProximo",
                table: "Alertas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncendioProximo",
                table: "Alertas",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }
    }
}
