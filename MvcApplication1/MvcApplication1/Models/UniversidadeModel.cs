using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MvcApplication1.Models
{
    public class UniversidadeModel
    {

        //Objeto da entidade
        private DBEntities db = new DBEntities();

        public List<UniversidadeDB> GetAllUniversidade(string ordem, string busca, string filtro, int? pagina)
        {

            var universidade = from uni in db.Universidade select uni;


            if (!String.IsNullOrEmpty(busca))
                universidade = universidade.Where(uni => uni.nome.ToUpper().Contains(busca.ToUpper()) || uni.cidade.ToUpper().Contains(busca.ToUpper()));

   
            
            if (universidade != null)
            {
                switch (ordem)
                {
                    case "nomeAsc":
                        universidade = universidade.OrderBy(uni => uni.nome);
                        break;
                    case "nomeDesc":
                        universidade = universidade.OrderByDescending(uni => uni.nome);
                        break;
                    case "cidadeAsc":
                        universidade = universidade.OrderBy(uni => uni.cidade);
                        break;
                    case "cidadeDesc":
                        universidade = universidade.OrderByDescending(uni => uni.cidade);
                        break;
                    default:
                        universidade = universidade.OrderBy(uni => uni.nome);
                        break;
                }

                List<UniversidadeDB> lista = new List<UniversidadeDB>();

                foreach (var item in universidade)
                {
                    UniversidadeDB udb = new UniversidadeDB();
                    udb.universidadeID = item.universidadeID;
                    udb.nome = item.nome;
                    udb.cidade = item.cidade;
                    lista.Add(udb);
                }

                return lista;
            }
            return null;
        }

        public List<UniversidadeDB> GetAllUniversidade()
        {

            var universidades = db.Universidade.ToList();

            if (universidades != null)
            {

                List<UniversidadeDB> lista = new List<UniversidadeDB>();

                foreach (var item in universidades)
                {
                    UniversidadeDB udb = new UniversidadeDB();
                    udb.universidadeID = item.universidadeID;
                    udb.nome = item.nome;
                    udb.cidade = item.cidade;
                    lista.Add(udb);
                }

                return lista;
            }
            return null;

        }


        //public List<UniversidadeDB> GetAllUniversidade()
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
        //        String query = "select universidadeID, nome, cidade from universidade";

        //        using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
        //        {
        //            List<UniversidadeDB> lst = new List<UniversidadeDB>();
        //            SqlDataReader r = null;
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            conn.Open();

        //            r = cmd.ExecuteReader(CommandBehavior.CloseConnection);


        //            while (r.Read())
        //            {
        //                int universidadeID = r.GetOrdinal("universidadeID");
        //                int nome = r.GetOrdinal("nome");
        //                int cidade = r.GetOrdinal("cidade");

        //                // Cria um objeto de notas
        //                UniversidadeDB u = new UniversidadeDB();

        //                u.universidadeID = r.GetInt32(universidadeID);
        //                u.nome = r.GetString(nome).ToString();
        //                u.cidade= r.GetString(cidade).ToString();

        //                lst.Add(u);
        //            }
        //            return lst;
        //        }
        //    }

        //    return null;
        //}
        // Inserir uma nota
        public void insert(string nome, string cidade)
        {

            Universidade universidade = new Universidade();

            universidade.nome = nome;
            universidade.cidade = cidade;

            db.Universidade.Add(universidade);
            db.SaveChanges();

        }


        // Alterar uma nota
        public void update(int universidadeID, string nome, string cidade)
        {
            Universidade uni = db.Universidade.Find(universidadeID);
            uni.nome = nome;
            uni.cidade = cidade;

            db.Universidade.Attach(uni);
            db.Entry(uni).Property("nome").IsModified = true;
            db.Entry(uni).Property("cidade").IsModified = true;

            db.SaveChanges();
        }

        // excluir uma nota
        public void delete(int universidadeID)
        {
            Universidade uni = new Universidade();
            uni = db.Universidade.Find(universidadeID);
            db.Universidade.Remove(uni);
            db.SaveChanges();
        }
    }


    public class UniversidadeDB
    {

        public int universidadeID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required(ErrorMessage = "Informe o nome da universidade")]
        public string nome { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required(ErrorMessage = "Deve ser informada uma cidade para a universidade")]
        public string cidade { get; set; }

    }
}