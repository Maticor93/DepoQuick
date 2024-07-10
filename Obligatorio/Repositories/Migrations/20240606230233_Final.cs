using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    public partial class Final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deposito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tamanio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Climatizacion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposito", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "RangoFechas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepositoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangoFechas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RangoFechas_Deposito_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Deposito",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Promocion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Etiqueta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descuento = table.Column<int>(type: "int", nullable: false),
                    RangoFechaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promocion_RangoFechas_RangoFechaId",
                        column: x => x.RangoFechaId,
                        principalTable: "RangoFechas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfAdmin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepositoId = table.Column<int>(type: "int", nullable: true),
                    ClienteEmail = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangoFechaId = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Pago = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserva_Deposito_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Deposito",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reserva_RangoFechas_RangoFechaId",
                        column: x => x.RangoFechaId,
                        principalTable: "RangoFechas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserva_Usuario_ClienteEmail",
                        column: x => x.ClienteEmail,
                        principalTable: "Usuario",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "PromocionDeposito",
                columns: table => new
                {
                    PromocionId = table.Column<int>(type: "int", nullable: false),
                    DepositoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocionDeposito", x => new { x.PromocionId, x.DepositoId });
                    table.ForeignKey(
                        name: "FK_PromocionDeposito_Deposito_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Deposito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromocionDeposito_Promocion_PromocionId",
                        column: x => x.PromocionId,
                        principalTable: "Promocion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promocion_RangoFechaId",
                table: "Promocion",
                column: "RangoFechaId");

            migrationBuilder.CreateIndex(
                name: "IX_PromocionDeposito_DepositoId",
                table: "PromocionDeposito",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_RangoFechas_DepositoId",
                table: "RangoFechas",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_ClienteEmail",
                table: "Reserva",
                column: "ClienteEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_DepositoId",
                table: "Reserva",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_RangoFechaId",
                table: "Reserva",
                column: "RangoFechaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromocionDeposito");

            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "Promocion");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "RangoFechas");

            migrationBuilder.DropTable(
                name: "Deposito");
        }
    }
}
