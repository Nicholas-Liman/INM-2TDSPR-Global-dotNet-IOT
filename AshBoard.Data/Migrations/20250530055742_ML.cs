using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class ML : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Alertas",
                type: "NVARCHAR2(2000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Alertas");
        }
    }
}
