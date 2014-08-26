using FrameLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication1.Tabelas
{
    public partial class Cursos : ICloneable, IHasLoggingReference
    {
        public Cursos()
        {
            this.Alunos = new HashSet<Alunos>();
            this.Disciplinas = new HashSet<Disciplinas>();
        }
        
        [Key]
        public int CursoId { get; set; }
        public string Nome { get; set; }
        
        public int UniversidadeId { get; set; }

        public virtual ICollection<Alunos> Alunos { get; set; }

        [ForeignKey("UniversidadeId")]
        public virtual Universidades Universidades { get; set; }
        
        public virtual ICollection<Disciplinas> Disciplinas { get; set; }

        public object Clone()
        {
            return Copy();
        }

        public Cursos Copy()
        {
            return new Cursos()
            {
                CursoId = this.CursoId,
                Nome = this.Nome,
                UniversidadeId = this.UniversidadeId,
            };
        }

        public object Reference
        {
            get { return CursoId; }
        }
    }
}
