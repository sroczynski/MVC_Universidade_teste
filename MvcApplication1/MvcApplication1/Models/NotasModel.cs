using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations;
using MvcApplication1.Tabelas;

namespace MvcApplication1.Models
{
    public class NotasModel
    {
        //Objeto da entidade
        private DBEntities db = new DBEntities();

        public List<NotasDB> GetAllNotas(string ordem, string busca, string filtro, int? pagina)
        {
            var notas = from not in db.Notas select not;

            if (notas != null)
            {

                List<NotasDB> lista = new List<NotasDB>();

                foreach (var item in notas)
                {
                    NotasDB not = new NotasDB();

                    not.alunoID = item.AlunoId;
                    not.nota = item.Nota;
                    not.disciplinaID = item.DisciplinaId;

                    not.alunoNome = item.Alunos.Nome;
                    not.disciplinaNome = item.Disciplina.Nome;

                    lista.Add(not);
                }

                // Carrega para a lista apenas os registro que possuam o indicado na busca
                if (!String.IsNullOrEmpty(busca))
                    lista = lista.Where(x => x.alunoNome.ToUpper().Contains(busca.ToUpper()) ||
                                        x.disciplinaNome.ToUpper().Contains(busca.ToUpper()) ||
                                        x.nota.ToString().ToUpper().Contains(busca.ToUpper())).ToList();
                // Ordena a lista
                switch (ordem)
                {
                    case "alunoAsc":
                        lista = lista.OrderBy(x => x.alunoNome).ToList();
                        break;
                    case "alunoDesc":
                        lista = lista.OrderByDescending(x => x.alunoNome).ToList();
                        break;
                    case "disciplinaAsc":
                        lista = lista.OrderBy(x => x.disciplinaNome).ToList();
                        break;
                    case "disciplinaDesc":
                        lista = lista.OrderByDescending(x => x.disciplinaNome).ToList();
                        break;
                    case "notaAsc":
                        lista = lista.OrderBy(x => x.nota).ToList();
                        break;
                    case "notaDesc":
                        lista = lista.OrderByDescending(x => x.nota).ToList();
                        break;
                    default:
                        lista = lista.OrderBy(x => x.alunoNome).ToList();
                        break;
                }
                return lista;
            }
            return null;
        }


        public List<NotasDB> GetAllNotas()
        {

            var notas = db.Notas.ToList();

            if (notas != null)
            {

                List<NotasDB> lista = new List<NotasDB>();

                foreach (var item in notas)
                {
                    NotasDB not = new NotasDB();

                    not.alunoID = item.AlunoId;
                    not.nota = item.Nota;
                    not.disciplinaID = item.DisciplinaId;
                    
                    not.alunoNome = item.Alunos.Nome;
                    not.disciplinaNome = item.Disciplina.Nome;

                    lista.Add(not);
                }

                return lista;
            }
            return null;
        }
        
        
        
        // Inserir uma nota

        public void insert(int alunoID, int disciplinaID, int nota)
        {
            try
            {
                Notas notas = new Notas();
                notas.AlunoId = alunoID;
                notas.DisciplinaId = disciplinaID;
                notas.Nota = nota;

                db.Notas.Add(notas);
                //db.Save(new Log.User("Nicolas"));
            }
            catch (Exception)
            {
                update(alunoID, disciplinaID, nota);
            }
        }


        // Alterar uma nota
        public void update(int alunoID, int disciplinaID, int nota)
        {
            Notas notas = db.Notas.Find(alunoID, disciplinaID);
            notas.Nota = nota;

            db.Notas.Attach(notas);
            db.Entry(notas).Property("Nota").IsModified = true;

            //db.Save(new Log.User("Nicolas"));
        }

        // excluir uma nota
        public void delete(int alunoID, int disciplinaID)
        {
            Notas notas = new Notas();
            notas = db.Notas.Find(alunoID, disciplinaID);
            db.Notas.Remove(notas);
            //db.Save(new Log.User("Nicolas"));
        }

    }

    public class NotasDB
    {
        public int alunoID { get; set; }
        public string alunoNome { get; set; }

        public int disciplinaID { get; set; }
        public string disciplinaNome { get; set; }

        [Range(0, 10, ErrorMessage = "Nota inválida. Somente são válidas notas de 0 a 10")]
        public int nota { get; set; }
    }

}