using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agenda.Server.Migrations
{
    public partial class CriacaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contato",
                columns: table => new
                {
                    IdContato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bairro = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Complemento = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    RuaAvenida = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Telefone1 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Telefone2 = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contato", x => x.IdContato);
                });

            migrationBuilder.CreateTable(
                name: "Compromisso",
                columns: table => new
                {
                    IdCompromisso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContatoIdContato = table.Column<int>(type: "int", nullable: true),
                    DataCompromisso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    FkIdContato = table.Column<int>(type: "int", nullable: false),
                    HorarioCompromisso = table.Column<TimeSpan>(type: "time", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compromisso", x => x.IdCompromisso);
                    table.ForeignKey(
                        name: "FK_Compromisso_Contato_ContatoIdContato",
                        column: x => x.ContatoIdContato,
                        principalTable: "Contato",
                        principalColumn: "IdContato");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compromisso_ContatoIdContato",
                table: "Compromisso",
                column: "ContatoIdContato");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compromisso");

            migrationBuilder.DropTable(
                name: "Contato");
        }
    }
}
