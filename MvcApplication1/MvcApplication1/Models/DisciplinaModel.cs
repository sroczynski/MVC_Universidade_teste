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
    public class DisciplinaModel
    {

        //Objeto da entidade
        private DBEntities db = new DBEntities();
        // String de conexão
        ConnectionStringSettings getString = new ConnectionStringSettings("TESTE", "Data Source=localhost\\SQLEXPRESS;Initial Catalog=TESTE;Integrated Security=True");

        public List<DisciplinaDB> GetAllDisciplinas(string ordem, string busca, string filtro, int? pagina)
        {
            var disciplinas = from dis in db.Disciplina select dis;

            if (disciplinas != null)
            {

                List<DisciplinaDB> lista = new List<DisciplinaDB>();

                foreach (var item in disciplinas)
                {
                    DisciplinaDB dis = new DisciplinaDB();

                    dis.disciplinaID = item.disciplinaID;
                    dis.nomeDisciplina = item.nome;
                    dis.cursoID = item.cursoID;
                    dis.nomeCurso = item.Curso.nome;
                    dis.universidadeID = item.Curso.Universidade.universidadeID;
                    dis.nomeUniversidade = item.Curso.Universidade.nome;

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
            var disciplinas = db.Disciplina.ToList();

            if (disciplinas != null)
            {
                List<DisciplinaDB> lista = new List<DisciplinaDB>();

                foreach (var item in disciplinas)
                {
                    DisciplinaDB dis = new DisciplinaDB();

                    dis.disciplinaID = item.disciplinaID;
                    dis.nomeDisciplina = item.nome;
                    dis.cursoID = item.cursoID;
                    dis.nomeCurso = item.Curso.nome;
                    dis.universidadeID = item.Curso.Universidade.universidadeID;
                    dis.nomeUniversidade = item.Curso.Universidade.nome;

                    lista.Add(dis);
                }

                return lista;
            }
            return null;

        }


        //public List<DisciplinaDB> GetAllDisciplinas()
        //{
        //    if (getString != null)
        //    {
        //        String query = "select di.disciplinaID as disciplinaID, di.nome as nomeDisciplina, cur.cursoID as cursoID, cur.nome as nomeCurso, un.universidadeID as universidadeID, un.nome as nomeUniversidade from Disciplina di inner join curso cur on di.cursoID = cur.cursoID inner join universidade un on cur.universidadeID = un.universidadeID";

        //        using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
        //        {
        //            List<DisciplinaDB> lst = new List<DisciplinaDB>();
        //            SqlDataReader r = null;
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            conn.Open();

        //            r = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //            while (r.Read())
        //            {
        //                int disciplinaID = r.GetOrdinal("disciplinaID");
        //                int nomeDisciplina = r.GetOrdinal("nomeDisciplina");
        //                int cursoID = r.GetOrdinal("cursoID");
        //                int nomeCurso = r.GetOrdinal("nomeCurso");
        //                int universidadeID = r.GetOrdinal("universidadeID");
        //                int nomeUniversidade = r.GetOrdinal("nomeUniversidade");

        //                // Cria um objeto de cursos
        //                DisciplinaDB d = new DisciplinaDB();

        //                d.disciplinaID = r.GetInt32(disciplinaID);
        //                d.cursoID = r.GetInt32(cursoID);
        //                d.universidadeID = r.GetInt32(universidadeID);

        //                d.nomeDisciplina = r.GetString(nomeDisciplina).ToString();
        //                d.nomeCurso = r.GetString(nomeCurso).ToString();
        //                d.nomeUniversidade = r.GetString(nomeUniversidade).ToString();

        //                lst.Add(d);
        //            }
        //            return lst;
        //        }
        //    }
        //    return null;
        //}


        // Inserir um curso
        public void insert(int curso, string nome)
        {

            Disciplina disciplina = new Disciplina();
            disciplina.cursoID = curso;
            disciplina.nome = nome;

            db.Disciplina.Add(disciplina);
            db.SaveChanges();

        }
        // Alterar um curso
        public void update(int disciplinaID, int cursoID, string nome)
        {
            Disciplina disciplina = db.Disciplina.Find(disciplinaID);
            disciplina.nome = nome;
            disciplina.cursoID = cursoID;

            db.Disciplina.Attach(disciplina);
            db.Entry(disciplina).Property("nome").IsModified = true;
            db.Entry(disciplina).Property("cursoID").IsModified = true;
            db.SaveChanges();
        }

        // excluir um curso
        public void delete(int disciplinaID)
        {
            Disciplina disciplina = new Disciplina();
            disciplina = db.Disciplina.Find(disciplinaID);
            db.Disciplina.Remove(disciplina);
            db.SaveChanges();
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

}