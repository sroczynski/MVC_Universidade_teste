using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations;

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

                    not.alunoID = item.alunoID;
                    not.nota = item.nota;
                    not.disciplinaID = item.disciplinaID;

                    not.alunoNome = item.Aluno.nome;
                    not.disciplinaNome = item.Disciplina.nome;

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

                    not.alunoID = item.alunoID;
                    not.nota = item.nota;
                    not.disciplinaID = item.disciplinaID;
                    
                    not.alunoNome = item.Aluno.nome;
                    not.disciplinaNome = item.Disciplina.nome;

                    lista.Add(not);
                }

                return lista;
            }
            return null;
        }
        
        
        //public List<NotasDB> GetAllNotas()
        //{

        //    ConnectionStringSettings getString = WebConfigurationManager.ConnectionStrings["TESTE"] as ConnectionStringSettings;
        //    // Teste feito pelo fato da instrução acima não retornar corretamente o nome e o connectionString do banco.
        //    if (getString == null)
        //    {
        //        ConnectionStringSettings tst = new ConnectionStringSettings("TESTE", "Data Source=localhost\\SQLEXPRESS;Initial Catalog=TESTE;Integrated Security=True");
        //        getString = tst;
        //    }

        //    if (getString != null)
        //    {
        //        String query = "select al.alunoID as alunoID, al.nome as NomeAluno,di.disciplinaID as disciplinaID, di.nome as NomeDisciplina, no.nota as Nota from notas no inner join aluno al on no.alunoID = al.alunoID inner join disciplina di on no.disciplinaID = di.disciplinaID";

        //        using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
        //        {
        //            List<NotasDB> lst = new List<NotasDB>();
        //            SqlDataReader r = null;
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            conn.Open();

        //            r = cmd.ExecuteReader(CommandBehavior.CloseConnection);


        //            while (r.Read())
        //            {
        //                int alunoID = r.GetOrdinal("alunoID");
        //                int NomeAluno = r.GetOrdinal("NomeAluno");
        //                int disciplinaID = r.GetOrdinal("disciplinaID");
        //                int NomeDisciplina = r.GetOrdinal("NomeDisciplina");
        //                int Nota = r.GetOrdinal("Nota");

        //                // Cria um objeto de notas
        //                NotasDB n = new NotasDB();

        //                n.alunoID = r.GetInt32(alunoID);
        //                n.alunoNome = r.GetString(NomeAluno).ToString();
        //                n.disciplinaID = r.GetInt32(disciplinaID);
        //                n.disciplinaNome = r.GetString(NomeDisciplina).ToString();
        //                n.nota = r.GetInt32(Nota);

        //                lst.Add(n);
        //            }
        //            return lst;
        //        }
        //    }

        //    return null;
        //}
        // Inserir uma nota

        public void insert(int alunoID, int disciplinaID, int nota)
        {
            try
            {
                Notas notas = new Notas();
                notas.alunoID = alunoID;
                notas.disciplinaID = disciplinaID;
                notas.nota = nota;

                db.Notas.Add(notas);
                db.SaveChanges();
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
            notas.nota = nota;

            db.Notas.Attach(notas);
            db.Entry(notas).Property("nota").IsModified = true;

            db.SaveChanges();
        }

        // excluir uma nota
        public void delete(int alunoID, int disciplinaID)
        {

            Notas notas = new Notas();
            notas = db.Notas.Find(alunoID, disciplinaID);
            db.Notas.Remove(notas);
            db.SaveChanges();

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