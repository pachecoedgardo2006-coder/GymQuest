using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymQuest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ejercicios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Categoria = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FactorXp = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejercicios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Heroes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    ExperienciaActual = table.Column<int>(type: "integer", nullable: false),
                    ExperienciaParaSiguienteNivel = table.Column<int>(type: "integer", nullable: false),
                    Fuerza = table.Column<int>(type: "integer", nullable: false),
                    Resistencia = table.Column<int>(type: "integer", nullable: false),
                    Consistencia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heroes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HeroeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Desbloqueado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaDesbloqueo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Misiones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HeroeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    TipoObjetivo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ValorObjetivo = table.Column<int>(type: "integer", nullable: false),
                    ProgresoActual = table.Column<int>(type: "integer", nullable: false),
                    Completada = table.Column<bool>(type: "boolean", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Misiones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rachas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HeroeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiasConsecutivos = table.Column<int>(type: "integer", nullable: false),
                    DiasProgramadosPorSemana = table.Column<int>(type: "integer", nullable: false),
                    PocionesDeDescansoDisponibles = table.Column<int>(type: "integer", nullable: false),
                    UltimaFechaEntrenada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rachas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rutinas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EsPlantillaPredeterminada = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutinas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SesionesEntrenamiento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HeroeId = table.Column<Guid>(type: "uuid", nullable: false),
                    RutinaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Completada = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesEntrenamiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RutinaEjercicios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RutinaId = table.Column<Guid>(type: "uuid", nullable: false),
                    EjercicioId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeriesObjetivo = table.Column<int>(type: "integer", nullable: false),
                    RepeticionesObjetivo = table.Column<int>(type: "integer", nullable: false),
                    PesoInicial = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutinaEjercicios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RutinaEjercicios_Rutinas_RutinaId",
                        column: x => x.RutinaId,
                        principalTable: "Rutinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesRealizadas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EjercicioId = table.Column<Guid>(type: "uuid", nullable: false),
                    PesoLevantado = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    RepeticionesRealizadas = table.Column<int>(type: "integer", nullable: false),
                    EsRecordPersonal = table.Column<bool>(type: "boolean", nullable: false),
                    SesionEntrenamientoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesRealizadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeriesRealizadas_SesionesEntrenamiento_SesionEntrenamientoId",
                        column: x => x.SesionEntrenamientoId,
                        principalTable: "SesionesEntrenamiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Heroes_UsuarioId",
                table: "Heroes",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logros_HeroeId",
                table: "Logros",
                column: "HeroeId");

            migrationBuilder.CreateIndex(
                name: "IX_Misiones_HeroeId",
                table: "Misiones",
                column: "HeroeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rachas_HeroeId",
                table: "Rachas",
                column: "HeroeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RutinaEjercicios_EjercicioId",
                table: "RutinaEjercicios",
                column: "EjercicioId");

            migrationBuilder.CreateIndex(
                name: "IX_RutinaEjercicios_RutinaId",
                table: "RutinaEjercicios",
                column: "RutinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesRealizadas_EjercicioId",
                table: "SeriesRealizadas",
                column: "EjercicioId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesRealizadas_SesionEntrenamientoId",
                table: "SeriesRealizadas",
                column: "SesionEntrenamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_SesionesEntrenamiento_HeroeId",
                table: "SesionesEntrenamiento",
                column: "HeroeId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ejercicios");

            migrationBuilder.DropTable(
                name: "Heroes");

            migrationBuilder.DropTable(
                name: "Logros");

            migrationBuilder.DropTable(
                name: "Misiones");

            migrationBuilder.DropTable(
                name: "Rachas");

            migrationBuilder.DropTable(
                name: "RutinaEjercicios");

            migrationBuilder.DropTable(
                name: "SeriesRealizadas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Rutinas");

            migrationBuilder.DropTable(
                name: "SesionesEntrenamiento");
        }
    }
}
