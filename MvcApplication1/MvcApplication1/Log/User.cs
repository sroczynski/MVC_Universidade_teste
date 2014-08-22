using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Log
{
    public class User
    {

        public User(string nome) {
            this.Nome = nome;
        }

        public string Nome { get; set; }
    }
}