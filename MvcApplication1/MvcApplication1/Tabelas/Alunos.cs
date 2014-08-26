using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Tabelas
{
    using FrameLog;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Alunos : ICloneable , IHasLoggingReference
    {
        public Alunos()
        {
            this.Notas = new HashSet<Notas>();
        }
        [Key]
        public int AlunoId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DataNascimento { get; set; }
        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        public virtual Cursos Curso { get; set; }
        public virtual ICollection<Notas> Notas { get; set; }

        

        public object Clone()
        {
            return Copy();
        }
        public Alunos Copy() {
            return new Alunos()
            {
                AlunoId = this.AlunoId,
                Nome = this.Nome,
                Email = this.Email,
                DataNascimento = this.DataNascimento,
                CursoId = this.CursoId
            };
        }
        public object Reference
        {
            get { return AlunoId; }
        }
    }
}