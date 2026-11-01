using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oficina.Infra.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "token",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Valor = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                    ExpiraEm = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Revogado = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataUltimaModificacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataExclusao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_token_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "auth",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_token_UsuarioId",
                schema: "auth",
                table: "token",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_token_Valor",
                schema: "auth",
                table: "token",
                column: "Valor",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "token",
                schema: "auth");
        }
    }
}
