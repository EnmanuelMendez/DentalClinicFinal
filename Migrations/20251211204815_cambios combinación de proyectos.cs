using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalClinic.Migrations
{
    /// <inheritdoc />
    public partial class cambioscombinacióndeproyectos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemServicio_Servicio_ServicioId",
                table: "ItemServicio");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Cliente_ClienteId",
                table: "Reserva");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Servicio_ServicioId",
                table: "Reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servicio",
                table: "Servicio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reserva",
                table: "Reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemServicio",
                table: "ItemServicio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HorarioAtencion",
                table: "HorarioAtencion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfiguracionEmpresa",
                table: "ConfiguracionEmpresa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente");

            migrationBuilder.RenameTable(
                name: "Servicio",
                newName: "Servicios");

            migrationBuilder.RenameTable(
                name: "Reserva",
                newName: "Reservas");

            migrationBuilder.RenameTable(
                name: "ItemServicio",
                newName: "ItemsServicio");

            migrationBuilder.RenameTable(
                name: "HorarioAtencion",
                newName: "HorariosAtencion");

            migrationBuilder.RenameTable(
                name: "ConfiguracionEmpresa",
                newName: "ConfiguracionesEmpresa");

            migrationBuilder.RenameTable(
                name: "Cliente",
                newName: "Clientes");

            migrationBuilder.RenameIndex(
                name: "IX_Reserva_ServicioId",
                table: "Reservas",
                newName: "IX_Reservas_ServicioId");

            migrationBuilder.RenameIndex(
                name: "IX_Reserva_ClienteId",
                table: "Reservas",
                newName: "IX_Reservas_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemServicio_ServicioId",
                table: "ItemsServicio",
                newName: "IX_ItemsServicio_ServicioId");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Servicios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DuracionMinutos",
                table: "Servicios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MargenErrorMinutos",
                table: "Servicios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnonymousToken",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DentistaId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DuracionRealMinutos",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "Clientes",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Seguro",
                table: "Clientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaReserva",
                table: "Clientes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servicios",
                table: "Servicios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemsServicio",
                table: "ItemsServicio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HorariosAtencion",
                table: "HorariosAtencion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfiguracionesEmpresa",
                table: "ConfiguracionesEmpresa",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Clinicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RNC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consultorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pasillo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroPuerta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quejas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaId = table.Column<int>(type: "int", nullable: false),
                    Asunto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quejas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quejas_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosCorreo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaId = table.Column<int>(type: "int", nullable: true),
                    FromEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosCorreo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosCorreo_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Valoraciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservaId = table.Column<int>(type: "int", nullable: false),
                    ServicioId = table.Column<int>(type: "int", nullable: false),
                    Puntuacion = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valoraciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Valoraciones_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dentistas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConsultorioId = table.Column<int>(type: "int", nullable: true),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dentistas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dentistas_Consultorios_ConsultorioId",
                        column: x => x.ConsultorioId,
                        principalTable: "Consultorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_DentistaId",
                table: "Reservas",
                column: "DentistaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dentistas_ConsultorioId",
                table: "Dentistas",
                column: "ConsultorioId");

            migrationBuilder.CreateIndex(
                name: "IX_Quejas_ReservaId",
                table: "Quejas",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosCorreo_ReservaId",
                table: "RegistrosCorreo",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Valoraciones_ReservaId",
                table: "Valoraciones",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsServicio_Servicios_ServicioId",
                table: "ItemsServicio",
                column: "ServicioId",
                principalTable: "Servicios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Clientes_ClienteId",
                table: "Reservas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Dentistas_DentistaId",
                table: "Reservas",
                column: "DentistaId",
                principalTable: "Dentistas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Servicios_ServicioId",
                table: "Reservas",
                column: "ServicioId",
                principalTable: "Servicios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsServicio_Servicios_ServicioId",
                table: "ItemsServicio");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Clientes_ClienteId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Dentistas_DentistaId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Servicios_ServicioId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "Clinicas");

            migrationBuilder.DropTable(
                name: "Dentistas");

            migrationBuilder.DropTable(
                name: "Quejas");

            migrationBuilder.DropTable(
                name: "RegistrosCorreo");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Valoraciones");

            migrationBuilder.DropTable(
                name: "Consultorios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servicios",
                table: "Servicios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_DentistaId",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemsServicio",
                table: "ItemsServicio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HorariosAtencion",
                table: "HorariosAtencion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfiguracionesEmpresa",
                table: "ConfiguracionesEmpresa");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "DuracionMinutos",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "MargenErrorMinutos",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "AnonymousToken",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "DentistaId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "DuracionRealMinutos",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Seguro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UltimaReserva",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "Servicios",
                newName: "Servicio");

            migrationBuilder.RenameTable(
                name: "Reservas",
                newName: "Reserva");

            migrationBuilder.RenameTable(
                name: "ItemsServicio",
                newName: "ItemServicio");

            migrationBuilder.RenameTable(
                name: "HorariosAtencion",
                newName: "HorarioAtencion");

            migrationBuilder.RenameTable(
                name: "ConfiguracionesEmpresa",
                newName: "ConfiguracionEmpresa");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "Cliente");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_ServicioId",
                table: "Reserva",
                newName: "IX_Reserva_ServicioId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_ClienteId",
                table: "Reserva",
                newName: "IX_Reserva_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsServicio_ServicioId",
                table: "ItemServicio",
                newName: "IX_ItemServicio_ServicioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servicio",
                table: "Servicio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reserva",
                table: "Reserva",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemServicio",
                table: "ItemServicio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HorarioAtencion",
                table: "HorarioAtencion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfiguracionEmpresa",
                table: "ConfiguracionEmpresa",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemServicio_Servicio_ServicioId",
                table: "ItemServicio",
                column: "ServicioId",
                principalTable: "Servicio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Cliente_ClienteId",
                table: "Reserva",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Servicio_ServicioId",
                table: "Reserva",
                column: "ServicioId",
                principalTable: "Servicio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
