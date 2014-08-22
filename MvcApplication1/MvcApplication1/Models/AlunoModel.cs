using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{
    public class AlunoModel
    {
        //Objeto da entidade
        private DBEntities db = new DBEntities();
        // String de conexão
        ConnectionStringSettings getString = new ConnectionStringSettings("TESTE", "Data Source=localhost\\SQLEXPRESS;Initial Catalog=TESTE;Integrated Security=True");

        public List<AlunoDB> GetAllAlunos(string ordem, string busca, string filtro, int? pagina)
        {
            var alunos = from alu in db.Aluno select alu;

            if (alunos != null)
            {

                List<AlunoDB> lista = new List<AlunoDB>();

                foreach (var item in alunos)
                {
                    AlunoDB alu = new AlunoDB();

                    alu.alunoID = item.alunoID;
                    alu.alunoNome = item.nome;
                    alu.alunoEmail = item.email;
                    alu.dataNascimento = item.dataNascimento.Value.Date;
                    alu.cursoID = item.cursoID;
                    alu.cursoNome = item.Curso.nome;
                    alu.universidadeID = item.Curso.Universidade.universidadeID;
                    alu.universidadeNome = item.Curso.Universidade.nome;

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

            var alunos = db.Aluno.ToList();

            if (alunos != null)
            {

                List<AlunoDB> lista = new List<AlunoDB>();

                foreach (var item in alunos)
                {
                    AlunoDB alu = new AlunoDB();

                    alu.alunoID = item.alunoID;
                    alu.alunoNome = item.nome;
                    alu.alunoEmail = item.email;
                    alu.dataNascimento = item.dataNascimento.Value.Date;
                    alu.cursoID = item.cursoID;
                    alu.cursoNome = item.Curso.nome;
                    alu.universidadeID = item.Curso.Universidade.universidadeID;
                    alu.universidadeNome = item.Curso.Universidade.nome;

                    lista.Add(alu);
                }

                return lista;
            }
            return null;
        }


        //public List<AlunoDB> GetAllAlunos()
        //{
        //    if (getString != null)
        //    {
        //        String query = "select al.alunoID as alunoID, al.nome as alunoNome, al.email as alunoEmail, al.dataNascimento as dataNascimento, c.cursoID as cursoID, c.nome as cursoNome, un.universidadeID as universidadeID, un.nome as universidadeNome from aluno al inner join curso c on al.cursoID = c.cursoID inner join universidade un on c.universidadeID = un.universidadeID order by al.alunoID";

        //        using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
        //        {
        //            List<AlunoDB> lst = new List<AlunoDB>();
        //            SqlDataReader r = null;
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            conn.Open();

        //            r = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //            while (r.Read())
        //            {
        //                int alunoID = r.GetOrdinal("alunoID");
        //                int alunoNome = r.GetOrdinal("alunoNome");
        //                int alunoEmail = r.GetOrdinal("alunoEmail");
        //                int dataNascimento = r.GetOrdinal("dataNascimento");
        //                int cursoID = r.GetOrdinal("cursoID");
        //                int cursoNome = r.GetOrdinal("cursoNome");
        //                int universidadeID = r.GetOrdinal("universidadeID");
        //                int universidadeNome = r.GetOrdinal("universidadeNome");

        //                // Cria um objeto de alunos
        //                AlunoDB a = new AlunoDB();

        //                a.alunoID = r.GetInt32(alunoID);
        //                a.alunoNome = r.GetString(alunoNome).ToString();
        //                a.alunoEmail = r.GetString(alunoEmail).ToString();
        //                a.dataNascimento = r.GetDateTime(dataNascimento);
        //                a.cursoID = r.GetInt32(cursoID);
        //                a.cursoNome = r.GetString(cursoNome).ToString();
        //                a.universidadeID = r.GetInt32(universidadeID);
        //                a.universidadeNome = r.GetString(universidadeNome).ToString();

        //                lst.Add(a);
        //            }
        //            return lst;
        //        }
        //    }
        //    return null;
        //}


        // Inserir um aluno

        public void insert(string nome, string email, DateTime dataNascimento, int cursoID)
        {

            Aluno aluno = new Aluno();

            aluno.nome = nome;
            aluno.email = email;
            aluno.dataNascimento = dataNascimento.Date;
            aluno.cursoID = cursoID;

            db.Aluno.Add(aluno);
            
            /*
             *  TODO:
             */
            db.SaveChanges(new Log.User("Nicolas"));
        }
        // Alterar um aluno
        public void update(int alunoID, string nome, string email, DateTime dataNascimento, int cursoID)
        {

            Aluno aluno = new Aluno();
            aluno = db.Aluno.Find(alunoID);

            if (aluno != null)
            {
                aluno.email = email;
                aluno.nome = nome;
                aluno.dataNascimento = dataNascimento.Date;
                aluno.cursoID = cursoID;

                db.Aluno.Attach(aluno);

                db.Entry(aluno).Property("nome").IsModified = true;
                db.Entry(aluno).Property("email").IsModified = true;
                db.Entry(aluno).Property("dataNascimento").IsModified = true;
                db.Entry(aluno).Property("cursoID").IsModified = true;

                db.SaveChanges(new Log.User("Nicolas"));
            }
        }

        // excluir um curso
        public void delete(int alunoID)
        {
            Aluno aluno = new Aluno();
            aluno = db.Aluno.Find(alunoID);
            db.Aluno.Remove(aluno);
            db.SaveChanges(new Log.User("Nicolas"));
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
        public DateTime dataNascimento { get; set; }

        public int cursoID { get; set; }

        public string cursoNome { get; set; }

        public int universidadeID { get; set; }

        public string universidadeNome { get; set; }


    }
}