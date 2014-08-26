using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Tabelas
{
    public partial class Notas
    {
        [Key]
        [Column(Order = 1)] 
        public int AlunoId { get; set; }
        [Key]
        [Column(Order = 2)] 
        public int DisciplinaId { get; set; }
        [Key]
        [Column(Order = 3)]
        public int CursoId { get; set; }
        
        public int Nota { get; set; }

        [ForeignKey("AlunoId")]
        public virtual Alunos Alunos { get; set; }
        
        [ForeignKey("DisciplinaId")]
        public virtual Disciplinas Disciplina { get; set; }
    }
}
