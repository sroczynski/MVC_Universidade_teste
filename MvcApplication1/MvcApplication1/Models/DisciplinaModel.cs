using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using MvcApplication1.Tabelas;
using MvcApplication1.Log;

namespace MvcApplication1.Models
{
    public class DisciplinaModel
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

            Disciplinas disciplina = new Disciplinas();
            disciplina = db.Disciplinas.Find(id);

            var changes = db.HistoryExplorer.ChangesTo(disciplina);

            foreach (var change in changes)
            {
                LogModel dis = new LogModel();
                dis.Autor = change.Author.ToString();
                dis.Horario = change.Timestamp;
                dis.Curso = change.Value.Nome;
                dis.Disciplina = change.Value.Nome;

                int curId = change.Value.CursoId;
                var cursoTab = db.Cursos.Find(curId);
                dis.Curso = cursoTab.Nome;

                dis.Universidade = db.Universidades.Find(cursoTab.UniversidadeId).Nome;

                log.Add(dis);
            }
            return log.OrderBy(x => x.Horario).ToList();
        }


        public List<DisciplinaDB> GetAllDisciplinas(string ordem, string busca, string filtro, int? pagina)
        {
            var disciplinas = from dis in db.Disciplinas select dis;

            if (disciplinas != null)
            {

                List<DisciplinaDB> lista = new List<DisciplinaDB>();

                foreach (var item in disciplinas)
                {
                    DisciplinaDB dis = new DisciplinaDB();

                    dis.disciplinaID = item.DisciplinaId;
                    dis.nomeDisciplina = item.Nome;
                    dis.cursoID = item.CursoId;
                    dis.nomeCurso = item.Curso.Nome;
                    dis.universidadeID = item.Curso.Universidades.UniversidadeId;
                    dis.nomeUniversidade = item.Curso.Universidades.Nome;

                    lista.Add(dis);
                }

                // Carrega para a lista apenas os registro que possuam o indicado na busca
                if (!String.IsNullOrEmpty(busca))
                    lista = lista.Where(x => x.nomeDisciplina.ToUpper().Contains(busca.ToUpper()) ||
                                        x.nomeUniversidade.ToUpper().Contains(busca.ToUpper()) ||
                                        x.nomeCurso.ToUpper().Contains(busca.ToUpper())).ToList();
                // Ordena a lista
                switch (ordem)
                {
                    case "disciplinaAsc":
                        lista = lista.OrderBy(x => x.nomeDisciplina).ToList();
                        break;
                    case "disciplinaDesc":
                        lista = lista.OrderByDescending(x => x.nomeDisciplina).ToList();
                        break;
                    case "universidadeAsc":
                        lista = lista.OrderBy(x => x.nomeUniversidade).ToList();
                        break;
                    case "universidadeDesc":
                        lista = lista.OrderByDescending(x => x.nomeUniversidade).ToList();
                        break;
                    case "cursoAsc":
                        lista = lista.OrderBy(x => x.nomeCurso).ToList();
                        break;
                    case "cursoDesc":
                        lista = lista.OrderByDescending(x => x.nomeCurso).ToList();
                        break;
                    default:
                        lista = lista.OrderBy(x => x.nomeDisciplina).ToList();
                        break;
                }

                return lista;
            }
            return null;
        }

        public List<DisciplinaDB> GetAllDisciplinas()
        {
            var disciplinas = db.Disciplinas.ToList();

            if (disciplinas != null)
            {
                List<DisciplinaDB> lista = new List<DisciplinaDB>();

                foreach (var item in disciplinas)
                {
                    DisciplinaDB dis = new DisciplinaDB();

                    dis.disciplinaID = item.DisciplinaId;
                    dis.nomeDisciplina = item.Nome;
                    dis.cursoID = item.CursoId;
                    dis.nomeCurso = item.Curso.Nome;
                    dis.universidadeID = item.Curso.Universidades.UniversidadeId;
                    dis.nomeUniversidade = item.Curso.Universidades.Nome;

                    lista.Add(dis);
                }

                return lista;
            }
            return null;

        }

        // Inserir um curso
        public void insert(int curso, string nome)
        {

            Disciplinas disciplina = new Disciplinas();
            disciplina.CursoId = curso;
            disciplina.Nome = nome;

            db.Disciplinas.Add(disciplina);
            db.Save(UserDefault());

        }
        // Alterar um curso
        public void update(int disciplinaID, int cursoID, string nome)
        {
            Disciplinas disciplina = db.Disciplinas.Find(disciplinaID);
            disciplina.Nome = nome;
            disciplina.CursoId = cursoID;

            db.Save(UserDefault());
        }

        // excluir um curso
        public void delete(int disciplinaID)
        {
            Disciplinas disciplina = new Disciplinas();
            disciplina = db.Disciplinas.Find(disciplinaID);
            db.Disciplinas.Remove(disciplina);
            db.Save(UserDefault());
        }
    }

    public class DisciplinaDB
    {
        public int disciplinaID { get; set; }
        public int cursoID { get; set; }
        public int universidadeID { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 50 caracteres")]
        [Required(ErrorMessage = "Informe o nome da disciplina")]
        public string nomeDisciplina { get; set; }

        public string nomeCurso { get; set; }
        public string nomeUniversidade { get; set; }

    }

    public partial class LogModel{
        public string Disciplina { get; set; }
    }
}