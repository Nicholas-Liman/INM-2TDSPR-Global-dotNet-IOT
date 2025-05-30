using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AshBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustesLeituraSensor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArraySensores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NomeLocal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArraySensores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NomeLocal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Latitude = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Longitude = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Temperatura = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NivelCO2 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DirecaoVento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DataUltimaLeitura = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ArraySensorId = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensores_ArraySensores_ArraySensorId",
                        column: x => x.ArraySensorId,
                        principalTable: "ArraySensores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DataHoraColeta = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NomeLocal = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Latitude = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Longitude = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Evento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Gravidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IncendioProximo = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SensorId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_Sensores_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_SensorId",
                table: "Alertas",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensores_ArraySensorId",
                table: "Sensores",
                column: "ArraySensorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Sensores");

            migrationBuilder.DropTable(
                name: "ArraySensores");
        }
    }
}
