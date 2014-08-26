using FrameLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Tabelas
{
    public partial class Disciplinas : ICloneable, IHasLoggingReference
    {
        public Disciplinas()
        {
            this.Notas = new HashSet<Notas>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)] 
        public int DisciplinaId { get; set; }
        public int CursoId { get; set; }
        public string Nome { get; set; }

        [ForeignKey("CursoId")]
        public virtual Cursos Curso { get; set; }
        public virtual ICollection<Notas> Notas { get; set; }

        public object Clone()
        {
            return Copy();
        }
        public Disciplinas Copy() {
            return new Disciplinas()
            {
                DisciplinaId = this.DisciplinaId,
                CursoId = this.CursoId,
                Nome = this.Nome,
            };
        }

        public object Reference
        {
            get { return DisciplinaId; }
        }
    }
}