using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using MvcApplication1.Tabelas;
using MvcApplication1.Log;
using MvcApplication1.Log.Tabelas;

namespace MvcApplication1.Models
{
    public class UniversidadeModel
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

        public List<LogModel> LogUniversidade(int id)
        {
            List<LogModel> log = new List<LogModel>();

            Universidades universidade = new Universidades();
            universidade = db.Universidades.Find(id);

            var changes = db.HistoryExplorer.ChangesTo(universidade);
            
            foreach (var change in changes)
            {
                LogModel uni = new LogModel();
                uni.Autor = change.Author.ToString();
                uni.Horario = change.Timestamp;
                uni.Universidade = change.Value.Nome;
                uni.CidadeUniversidade = change.Value.Cidade;
                log.Add(uni);
            }


            return log.OrderBy(x => x.Horario).ToList();

        }




        public List<UniversidadeDB> GetAllUniversidade(string ordem, string busca, string filtro, int? pagina)
        {

            var universidade = from uni in db.Universidades select uni;


            if (!String.IsNullOrEmpty(busca))
                universidade = universidade.Where(uni => uni.Nome.ToUpper().Contains(busca.ToUpper()) || uni.Cidade.ToUpper().Contains(busca.ToUpper()));



            if (universidade != null)
            {
                switch (ordem)
                {
                    case "nomeAsc":
                        universidade = universidade.OrderBy(uni => uni.Nome);
                        break;
                    case "nomeDesc":
                        universidade = universidade.OrderByDescending(uni => uni.Nome);
                        break;
                    case "cidadeAsc":
                        universidade = universidade.OrderBy(uni => uni.Cidade);
                        break;
                    case "cidadeDesc":
                        universidade = universidade.OrderByDescending(uni => uni.Cidade);
                        break;
                    default:
                        universidade = universidade.OrderBy(uni => uni.Nome);
                        break;
                }

                List<UniversidadeDB> lista = new List<UniversidadeDB>();

                foreach (var item in universidade)
                {
                    UniversidadeDB udb = new UniversidadeDB();
                    udb.universidadeID = item.UniversidadeId;
                    udb.nome = item.Nome;
                    udb.cidade = item.Cidade;
                    lista.Add(udb);
                }

                return lista;
            }
            return null;
        }

        public List<UniversidadeDB> GetAllUniversidade()
        {

            var universidades = db.Universidades.ToList();

            if (universidades != null)
            {

                List<UniversidadeDB> lista = new List<UniversidadeDB>();

                foreach (var item in universidades)
                {
                    UniversidadeDB udb = new UniversidadeDB();
                    udb.universidadeID = item.UniversidadeId;
                    udb.nome = item.Nome;
                    udb.cidade = item.Cidade;
                    lista.Add(udb);
                }

                return lista;
            }
            return null;

        }

        // Inserir uma nota
        public void insert(string nome, string cidade)
        {

            Universidades universidade = new Universidades();

            universidade.Nome = nome;
            universidade.Cidade = cidade;

            db.Universidades.Add(universidade);
            db.Save(UserDefault());

        }


        // Alterar uma nota
        public void update(int universidadeID, string nome, string cidade)
        {
            Universidades uni = db.Universidades.Find(universidadeID);

            uni.Nome = nome;
            uni.Cidade = cidade;
            db.Save(UserDefault());
        }

        // excluir uma nota
        public void delete(int universidadeID)
        {
            Universidades uni = new Universidades();
            uni = db.Universidades.Find(universidadeID);
            db.Universidades.Remove(uni);
            db.Save(UserDefault());
        }
    }


    public class UniversidadeDB
    {

        public int universidadeID { get; set; }

        [StringLength(50, MinimumLength = 3,ErrorMessage = "O nome deve ter entre 3 e 50 caracteres.")]
        [Required(ErrorMessage = "Informe o nome da universidade.")]
        public string nome { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A cidade deve ter entre 2 e 50 caracteres.")]
        [Required(ErrorMessage = "Deve ser informada uma cidade para a universidade.")]
        public string cidade { get; set; }

    }
    public partial class LogModel
    {

        public string Universidade { get; set; }
        public string CidadeUniversidade { get; set; }
    }
}