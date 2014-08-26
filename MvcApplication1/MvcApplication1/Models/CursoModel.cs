using MvcApplication1.Log;
using MvcApplication1.Tabelas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MvcApplication1.Models
{
    public class CursoModel
    {
        //Objeto da entidade
        private DBEntities db = new DBEntities();
        
        // Criado para teste
        public User UserDefault()
        {

            User user = db.User.FirstOrDefault();
            if (user == null)
            {
                user = new User();
                user.Name = "Nicolas";
                db.User.Add(user);
                db.Save(user);
            }
            return user;
        }

        public List<LogModel> Log(int id)
        {
            List<LogModel> log = new List<LogModel>();

            Cursos curso = new Cursos();
            curso = db.Cursos.Find(id);

            var changes = db.HistoryExplorer.ChangesTo(curso);

            foreach (var change in changes)
            {
                LogModel cur = new LogModel();
                cur.Autor = change.Author.ToString();
                cur.Horario = change.Timestamp;
                cur.Curso = change.Value.Nome;

                int uniId = change.Value.UniversidadeId;
                cur.Universidade = db.Universidades.Find(uniId).Nome;

                log.Add(cur);
            }
            return log.OrderBy(x =>x.Horario).ToList();
        }

        public List<CursoDB> GetAllCursos(string ordem, string busca, string filtro, int? pagina)
        {
            var cursos =    from cur
                            in db.Cursos 
                            select cur;

            if (cursos != null)
            {
                List<CursoDB> lista = new List<CursoDB>();
                foreach (var item in cursos)
                {
                    CursoDB cur = new CursoDB();
                    cur.cursoID = item.CursoId;
                    cur.nome = item.Nome;
                    cur.universidadeID = item.UniversidadeId;
                    cur.UniversidadeNome = item.Universidades.Nome;
                    lista.Add(cur);
                }

                // Carrega para a lista apenas os registro que possuam o indicado na busca
                if (!String.IsNullOrEmpty(busca))
                    lista = lista.Where(x => x.nome.ToUpper().Contains(busca.ToUpper()) ||
                                        x.UniversidadeNome.ToUpper().Contains(busca.ToUpper())).ToList();

                // Ordena a lista
                switch (ordem)
                {
                    case "cursoAsc":
                        lista = lista.OrderBy(x => x.nome).ToList();
                        break;
                    case "cursoDesc":
                        lista = lista.OrderByDescending(x => x.nome).ToList();
                        break;
                    case "universidadeAsc":
                        lista = lista.OrderBy(x => x.UniversidadeNome).ToList();
                        break;
                    case "universidadeDesc":
                        lista = lista.OrderByDescending(x => x.UniversidadeNome).ToList();
                        break;
                    default:
                        lista = lista.OrderBy(x => x.nome).ToList();
                        break;
                }

                return lista;
            }
            return null;
        }

        public List<CursoDB> GetAllCursos()
        {
            var cursos = db.Cursos.ToList();

            if (cursos != null)
            {

                List<CursoDB> lista = new List<CursoDB>();

                foreach (var item in cursos)
                {
                    CursoDB cur = new CursoDB();

                    cur.cursoID = item.CursoId;
                    cur.nome = item.Nome;
                    cur.universidadeID = item.UniversidadeId;
                    cur.UniversidadeNome = item.Universidades.Nome;
                    lista.Add(cur);
                }

                return lista;
            }
            return null;

        }

        
        // Inserir um curso
        public void insert(string nome, int universidadeID)
        {

            Cursos curso = new Cursos();
            curso.Nome = nome;
            curso.UniversidadeId = universidadeID;

            db.Cursos.Add(curso);
            db.Save(UserDefault());

        }
        // Alterar um curso
        public void update(int cursoID, string nome, int universidadeID)
        {
            //var cursoExistente = (from i in db.Curso where i.cursoID == cursoID select i).FirstOrDefault();

            Cursos curso = db.Cursos.Find(cursoID);
            curso.Nome = nome;
            curso.UniversidadeId = universidadeID;
            db.Save(UserDefault());
        }

        // excluir um curso
        public void delete(int cursoID)
        {
            Cursos curso = new Cursos();
            curso = db.Cursos.Find(cursoID);
            db.Cursos.Remove(curso);

            db.Save(UserDefault());
        }

    }


    // Representação dos campos da entidade Curso
    public class CursoDB
    {

        public int cursoID { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "O nome do curso deve possuir entre 5 e 50 caracteres")]
        [Required(ErrorMessage = "Deve ser informado um nome para o Curso")]
        public string nome { get; set; }

        [Range(1, 10, ErrorMessage = "Não existe a faculdade informada")]
        [Required(ErrorMessage = "O curso deve pertencer a uma universidade")]
        public int universidadeID { get; set; }

        public string UniversidadeNome { get; set; }

    }


    public partial class LogModel
    {
        public string Curso { get; set; }
    }

}