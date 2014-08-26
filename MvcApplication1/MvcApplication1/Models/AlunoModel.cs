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
    public class AlunoModel
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

            Alunos aluno = new Alunos();
            aluno = db.Alunos.Find(id);

            var changes = db.HistoryExplorer.ChangesTo(aluno);

            foreach (var change in changes)
            {
                LogModel alu = new LogModel();
                
                alu.Autor = change.Author.ToString();
                alu.Horario = change.Timestamp;
                alu.Aluno = change.Value.Nome;
                alu.AlunoEmail = change.Value.Email;
                alu.AlunoNascimento = change.Value.DataNascimento.ToString().Substring(0,10);

                int curId = change.Value.CursoId;
                var cursoTab = db.Cursos.Find(curId);
                alu.Curso = cursoTab.Nome;
                alu.Universidade = db.Universidades.Find(cursoTab.UniversidadeId).Nome;

                log.Add(alu);
            }
            return log.OrderBy(x => x.Horario).ToList();
        }

        public List<AlunoDB> GetAllAlunos(string ordem, string busca, string filtro, int? pagina)
        {
            var alunos = from alu in db.Alunos select alu;

            if (alunos != null)
            {

                List<AlunoDB> lista = new List<AlunoDB>();

                foreach (var item in alunos)
                {
                    AlunoDB alu = new AlunoDB();

                    alu.alunoID = item.AlunoId;
                    alu.alunoNome = item.Nome;
                    alu.alunoEmail = item.Email;
                    alu.dataNascimento = item.DataNascimento.Value.Date;
                    alu.cursoID = item.CursoId;
                    alu.cursoNome = item.Curso.Nome;
                    alu.universidadeID = item.Curso.Universidades.UniversidadeId;
                    alu.universidadeNome = item.Curso.Universidades.Nome;

                    lista.Add(alu);
                }

                // Carrega para a lista apenas os registro que possuam o indicado na busca
                if (!String.IsNullOrEmpty(busca))
                    lista = lista.Where(x => x.alunoNome.ToUpper().Contains(busca.ToUpper()) ||
                                             x.cursoNome.ToUpper().Contains(busca.ToUpper()) ||
                                             x.universidadeNome.ToUpper().Contains(busca.ToUpper())).ToList();
                // Ordena a lista
                switch (ordem)
                {
                    case "alunoAsc":
                        lista = lista.OrderBy(x => x.alunoNome).ToList();
                        break;
                    case "alunoDesc":
                        lista = lista.OrderByDescending(x => x.alunoNome).ToList();
                        break;
                    case "emailAsc":
                        lista = lista.OrderBy(x => x.alunoEmail).ToList();
                        break;
                    case "emailDesc":
                        lista = lista.OrderByDescending(x => x.alunoEmail).ToList();
                        break;
                    case "dataAsc":
                        lista = lista.OrderBy(x => x.dataNascimento).ToList();
                        break;
                    case "dataDesc":
                        lista = lista.OrderByDescending(x => x.dataNascimento).ToList();
                        break;
                    case "universidadeAsc":
                        lista = lista.OrderBy(x => x.cursoNome).ToList();
                        break;
                    case "universidadeDesc":
                        lista = lista.OrderByDescending(x => x.cursoNome).ToList();
                        break;
                    case "cursoAsc":
                        lista = lista.OrderBy(x => x.cursoNome).ToList();
                        break;
                    case "cursoDesc":
                        lista = lista.OrderByDescending(x => x.universidadeNome).ToList();
                        break;
                    default:
                        lista = lista.OrderBy(x => x.universidadeNome).ToList();
                        break;
                }
                return lista;
            }
            return null;
        }


        public List<AlunoDB> GetAllAlunos()
        {

            var alunos = db.Alunos.ToList();

            if (alunos != null)
            {

                List<AlunoDB> lista = new List<AlunoDB>();

                foreach (var item in alunos)
                {
                    AlunoDB alu = new AlunoDB();

                    alu.alunoID = item.AlunoId;
                    alu.alunoNome = item.Nome;
                    alu.alunoEmail = item.Email;
                    alu.dataNascimento = item.DataNascimento.Value.Date;
                    alu.cursoID = item.CursoId;
                    alu.cursoNome = item.Curso.Nome;
                    alu.universidadeID = item.Curso.Universidades.UniversidadeId;
                    alu.universidadeNome = item.Curso.Universidades.Nome;

                    lista.Add(alu);
                }

                return lista;
            }
            return null;
        }

        // Inserir um aluno

        public void insert(string nome, string email, DateTime dataNascimento, int cursoID)
        {

            Alunos aluno = new Alunos();

            aluno.Nome = nome;
            aluno.Email = email;
            aluno.DataNascimento = dataNascimento.Date;
            aluno.CursoId = cursoID;

            db.Alunos.Add(aluno);

            db.Save(UserDefault());
        }


        // Alterar um aluno
        public void update(int alunoID, string nome, string email, DateTime dataNascimento, int cursoID)
        {

            Alunos aluno = new Alunos();
            aluno = db.Alunos.Find(alunoID);

            if (aluno != null)
            {
                aluno.Email = email;
                aluno.Nome = nome;
                aluno.DataNascimento = dataNascimento.Date;
                aluno.CursoId = cursoID;

                db.Save(UserDefault());
            }
        }

        // excluir um curso
        public void delete(int alunoID)
        {
            Alunos aluno = new Alunos();
            aluno = db.Alunos.Find(alunoID);
            db.Alunos.Remove(aluno);
            db.Save(UserDefault());
        }

    }

    public class AlunoDB
    {
        public int alunoID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Informe o nome do aluno")]
        public string alunoNome { get; set; }

        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "O email informado não é valido")]
        [Required(ErrorMessage = "Informe o email do aluno")]
        public string alunoEmail { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Informe a data de nascimento do aluno.")]
        public DateTime dataNascimento { get; set; }

        public int cursoID { get; set; }

        public string cursoNome { get; set; }

        public int universidadeID { get; set; }

        public string universidadeNome { get; set; }


    }

    public partial class LogModel
    {
        public string Aluno { get; set; }
        public string AlunoEmail { get; set; }
        public string AlunoNascimento { get; set; }
    }
}