using FrameLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Tabelas
{
    public class Universidades : ICloneable, IHasLoggingReference
    {
        public Universidades()
        {
            this.Curso = new HashSet<Cursos>();
        }
        
        [Key]
        public int UniversidadeId { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }

        public virtual ICollection<Cursos> Curso { get; set; }


        public object Clone()
        {
            return Copy();
        }
        public Universidades Copy() {
            return new Universidades()
            {
                UniversidadeId = this.UniversidadeId,
                Nome = this.Nome,
                Cidade = this.Cidade,
            };
        }

        public override string ToString()
        {
            return Nome;
        }


        public object Reference
        {
            get { return UniversidadeId; }
        }
    }
}