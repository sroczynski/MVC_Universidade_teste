//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Universidade
    {
        public Universidade()
        {
            this.Curso = new HashSet<Curso>();
        }
    
        public int universidadeID { get; set; }
        public string nome { get; set; }
        public string cidade { get; set; }
    
        public virtual ICollection<Curso> Curso { get; set; }
    }
}