using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppFinanzas.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosOrden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DescripcionEstado = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosOrden", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposActivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposActivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TipoActivoId = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activos_TiposActivos_TipoActivoId",
                        column: x => x.TipoActivoId,
                        principalTable: "TiposActivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCuenta = table.Column<int>(type: "int", nullable: false),
                    IdActivo = table.Column<int>(type: "int", nullable: false),
                    NombreActivo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Operacion = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MontoTotal = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordenes_Activos_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "Activos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ordenes_EstadosOrden_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadosOrden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EstadosOrden",
                columns: new[] { "Id", "DescripcionEstado" },
                values: new object[,]
                {
                    { 0, "En proceso" },
                    { 1, "Ejecutada" },
                    { 3, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "TiposActivos",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Acción" },
                    { 2, "Bono" },
                    { 3, "FCI" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activos_TipoActivoId",
                table: "Activos",
                column: "TipoActivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_EstadoId",
                table: "Ordenes",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_IdActivo",
                table: "Ordenes",
                column: "IdActivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Activos");

            migrationBuilder.DropTable(
                name: "EstadosOrden");

            migrationBuilder.DropTable(
                name: "TiposActivos");
        }
    }
}
