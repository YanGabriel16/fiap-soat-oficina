using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oficina.Infra.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ImplementarOrcamentoRolesHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "auth",
                table: "usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<Guid>(
                name: "AtendenteId",
                schema: "oficina",
                table: "ordens_servico",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                schema: "oficina",
                table: "ordens_servico",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorMaoDeObra",
                schema: "oficina",
                table: "ordens_servico",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                schema: "clientes",
                table: "clientes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "historico_ordens_servico",
                schema: "oficina",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdemServicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusAnterior = table.Column<int>(type: "integer", nullable: true),
                    NovoStatus = table.Column<int>(type: "integer", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataUltimaModificacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataExclusao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_ordens_servico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_historico_ordens_servico_ordens_servico_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalSchema: "oficina",
                        principalTable: "ordens_servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_historico_ordens_servico_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "auth",
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orcamentos",
                schema: "oficina",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdemServicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorMaoDeObra = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataDecisao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataUltimaModificacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataExclusao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orcamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orcamentos_clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalSchema: "clientes",
                        principalTable: "clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orcamentos_ordens_servico_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalSchema: "oficina",
                        principalTable: "ordens_servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ordens_servico_itens",
                schema: "oficina",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdemServicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCatalogoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataUltimaModificacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataExclusao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordens_servico_itens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ordens_servico_itens_itens_catalogo_ItemCatalogoId",
                        column: x => x.ItemCatalogoId,
                        principalSchema: "oficina",
                        principalTable: "itens_catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ordens_servico_itens_ordens_servico_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalSchema: "oficina",
                        principalTable: "ordens_servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orcamentos_itens",
                schema: "oficina",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCatalogoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataUltimaModificacao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataExclusao = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orcamentos_itens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orcamentos_itens_itens_catalogo_ItemCatalogoId",
                        column: x => x.ItemCatalogoId,
                        principalSchema: "oficina",
                        principalTable: "itens_catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orcamentos_itens_orcamentos_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalSchema: "oficina",
                        principalTable: "orcamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ordens_servico_AtendenteId",
                schema: "oficina",
                table: "ordens_servico",
                column: "AtendenteId");

            migrationBuilder.CreateIndex(
                name: "IX_clientes_UsuarioId",
                schema: "clientes",
                table: "clientes",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historico_ordens_servico_OrdemServicoId",
                schema: "oficina",
                table: "historico_ordens_servico",
                column: "OrdemServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_historico_ordens_servico_UsuarioId",
                schema: "oficina",
                table: "historico_ordens_servico",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_orcamentos_ClienteId",
                schema: "oficina",
                table: "orcamentos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_orcamentos_OrdemServicoId",
                schema: "oficina",
                table: "orcamentos",
                column: "OrdemServicoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orcamentos_itens_ItemCatalogoId",
                schema: "oficina",
                table: "orcamentos_itens",
                column: "ItemCatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_orcamentos_itens_OrcamentoId",
                schema: "oficina",
                table: "orcamentos_itens",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ordens_servico_itens_ItemCatalogoId",
                schema: "oficina",
                table: "ordens_servico_itens",
                column: "ItemCatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_ordens_servico_itens_OrdemServicoId",
                schema: "oficina",
                table: "ordens_servico_itens",
                column: "OrdemServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_clientes_usuarios_UsuarioId",
                schema: "clientes",
                table: "clientes",
                column: "UsuarioId",
                principalSchema: "auth",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_clientes_ClienteId",
                schema: "oficina",
                table: "ordens_servico",
                column: "ClienteId",
                principalSchema: "clientes",
                principalTable: "clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_usuarios_AtendenteId",
                schema: "oficina",
                table: "ordens_servico",
                column: "AtendenteId",
                principalSchema: "auth",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientes_usuarios_UsuarioId",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_clientes_ClienteId",
                schema: "oficina",
                table: "ordens_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_usuarios_AtendenteId",
                schema: "oficina",
                table: "ordens_servico");

            migrationBuilder.DropTable(
                name: "historico_ordens_servico",
                schema: "oficina");

            migrationBuilder.DropTable(
                name: "orcamentos_itens",
                schema: "oficina");

            migrationBuilder.DropTable(
                name: "ordens_servico_itens",
                schema: "oficina");

            migrationBuilder.DropTable(
                name: "orcamentos",
                schema: "oficina");

            migrationBuilder.DropIndex(
                name: "IX_ordens_servico_AtendenteId",
                schema: "oficina",
                table: "ordens_servico");

            migrationBuilder.DropIndex(
                name: "IX_clientes_UsuarioId",
                schema: "clientes",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "auth",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "AtendenteId",
                schema: "oficina",
                table: "ordens_servico");

            migrationBuilder.DropColumn(
                name: "Descricao",
                schema: "oficina",
                table: "ordens_servico");

            migrationBuilder.DropColumn(
                name: "ValorMaoDeObra",
                schema: "oficina",
                table: "ordens_servico");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                schema: "clientes",
                table: "clientes");
        }
    }
}
