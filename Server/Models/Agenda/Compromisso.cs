using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Server.Models.Agenda
{
    [Table("Compromisso", Schema = "dbo")]
    public class Compromisso
    {
        [Key]
        public int IdCompromisso { get; set; }

        [ForeignKey("Contato")]
        public int FkIdContato { get; set; }
        public Contato Contato { get; set; }

        [Required]
        public DateTime DataCompromisso { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Descricao { get; set; }

        [Required]
        public TimeSpan HorarioCompromisso { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Titulo { get; set; }
    }
}
