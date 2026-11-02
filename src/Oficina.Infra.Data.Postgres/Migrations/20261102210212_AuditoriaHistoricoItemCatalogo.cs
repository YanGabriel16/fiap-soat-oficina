using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oficina.Infra.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AuditoriaHistoricoItemCatalogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auditoria");

            migrationBuilder.CreateTable(
                name: "historico_itens_catalogo",
                schema: "auditoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCatalogoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Acao = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Preco = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    QuantidadeEstoque = table.Column<int>(type: "integer", nullable: false),
                    DataEvento = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_itens_catalogo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_historico_itens_catalogo_ItemCatalogoId_DataEvento",
                schema: "auditoria",
                table: "historico_itens_catalogo",
                columns: new[] { "ItemCatalogoId", "DataEvento" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historico_itens_catalogo",
                schema: "auditoria");
        }
    }
}
