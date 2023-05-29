using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Server.Models.Agenda
{
    public class Contato
    {
        [Key]
        public int IdContato { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Bairro { get; set; }

        [Required]
        public string Cep { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Cidade { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Complemento { get; set; }

        public DateTime DataNascimento { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Estado { get; set; }

        [Required]
        public string NomeCompleto { get; set; }

        public int Numero { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string RuaAvenida { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Telefone1 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Telefone2 { get; set; }
    }
}
