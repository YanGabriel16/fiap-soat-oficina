using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oficina.Infra.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTelefoneCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                schema: "clientes",
                table: "clientes",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefone",
                schema: "clientes",
                table: "clientes");
        }
    }
}
