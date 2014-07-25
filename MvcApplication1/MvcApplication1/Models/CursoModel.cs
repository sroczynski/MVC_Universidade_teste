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
        // String de conexão
        //ConnectionStringSettings getString = new ConnectionStringSettings("TESTE", "Data Source=localhost\\SQLEXPRESS;Initial Catalog=TESTE;Integrated Security=True");

        public List<CursoDB> GetAllCursos(string ordem, string busca, string filtro, int? pagina)
        {
            var cursos = from cur in db.Curso select cur;

            if (cursos != null)
            {

                List<CursoDB> lista = new List<CursoDB>();

                foreach (var item in cursos)
                {
                    CursoDB cur = new CursoDB();

                    cur.cursoID = item.cursoID;
                    cur.nome = item.nome;
                    cur.universidadeID = item.universidadeID;
                    cur.universidadeNome = item.Universidade.nome;
                    

                    lista.Add(cur);
                }

                // Carrega para a lista apenas os registro que possuam o indicado na busca
                if (!String.IsNullOrEmpty(busca))
                    lista = lista.Where(x => x.nome.ToUpper().Contains(busca.ToUpper()) ||
                                        x.universidadeNome.ToUpper().Contains(busca.ToUpper())).ToList();

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
                        lista = lista.OrderBy(x => x.universidadeNome).ToList();
                        break;
                    case "universidadeDesc":
                        lista = lista.OrderByDescending(x => x.universidadeNome).ToList();
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
            var cursos = db.Curso.ToList();

            if (cursos != null)
            {

                List<CursoDB> lista = new List<CursoDB>();

                foreach (var item in cursos)
                {
                    CursoDB cur = new CursoDB();

                    cur.cursoID = item.cursoID;
                    cur.nome = item.nome;
                    cur.universidadeID = item.universidadeID;
                    cur.universidadeNome = item.Universidade.nome;
                    lista.Add(cur);
                }

                return lista;
            }
            return null;

        }

        //public List<CursoDB> GetAllCursos()
        //{
        //    if (getString != null)
        //    {
        //        String query = "select c.cursoID as cursoID, c.nome as cursoNome, un.universidadeID as universidadeID, un.nome as universidadeNome from curso c inner join universidade un on c.universidadeID = un.universidadeID order by c.cursoID";

        //        using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
        //        {
        //            List<CursoDB> lst = new List<CursoDB>();
        //            SqlDataReader r = null;
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            conn.Open();

        //            r = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //            while (r.Read())
        //            {
        //                int cursoID = r.GetOrdinal("cursoID");
        //                int cursoNome = r.GetOrdinal("cursoNome");
        //                int universidadeID = r.GetOrdinal("universidadeID");
        //                int universidadeNome = r.GetOrdinal("universidadeNome");

        //                // Cria um objeto de cursos
        //                CursoDB c = new CursoDB();

        //                c.cursoID = r.GetInt32(cursoID);
        //                c.nome = r.GetString(cursoNome).ToString();
        //                c.universidadeID = r.GetInt32(universidadeID);
        //                c.universidadeNome = r.GetString(universidadeNome).ToString();

        //                lst.Add(c);
        //            }
        //            return lst;
        //        }
        //    }
        //    return null;
        //}


        // Inserir um curso
        public void insert(string nome, int universidadeID)
        {

            Curso curso = new Curso();
            curso.nome = nome;
            curso.universidadeID = universidadeID;

            db.Curso.Add(curso);
            db.SaveChanges();

        }
        // Alterar um curso
        public void update(int cursoID, string nome, int universidadeID)
        {
            //var cursoExistente = (from i in db.Curso where i.cursoID == cursoID select i).FirstOrDefault();

            Curso curso = db.Curso.Find(cursoID);
            curso.nome = nome;
            curso.universidadeID = universidadeID;

            db.Curso.Attach(curso);
            db.Entry(curso).Property("nome").IsModified = true;
            db.Entry(curso).Property("universidadeID").IsModified = true;
            db.SaveChanges();
        }

        // excluir um curso
        public void delete(int cursoID)
        {
            Curso curso = new Curso();
            curso = db.Curso.Find(cursoID);
            db.Curso.Remove(curso);
            db.SaveChanges();
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

        public string universidadeNome { get; set; }



    }
}