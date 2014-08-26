using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;

namespace MvcApplication1.Controllers
{
    public class CursoController : Controller
    {
        //
        // GET: /Curso/ 

        CursoModel cm = new CursoModel();

        public ActionResult Index(string ordem, string busca, string filtro, int? pagina)
        {
            ViewBag.ordemCorrente = ordem;
            ViewBag.curso = ordem == "cursoAsc" ? "cursoDesc" : "cursoAsc";
            ViewBag.universidade = ordem == "universidadeAsc" ? "universidadeDesc" : "universidadeAsc";

            if (!String.IsNullOrEmpty(busca))
            {
                pagina = 1;
            }
            else
            {
                busca = filtro;
            }

            ViewBag.filtro = busca;

            var cursos = cm.GetAllCursos(ordem, busca, filtro, pagina);

            if (cursos == null)
                return View("NotFound");

            int tamanhoPagina = 5;
            int numPagina = (pagina ?? 1);

            return View(cursos.ToPagedList(numPagina, tamanhoPagina));
        }
        /* 
         * Criar
         * 
         */
        public ActionResult Create()
        {

            ViewBag.listaUniversidade = carregaListaUni();


            return View();
        }

        [HttpPost]
        public ActionResult Create(CursoDB novo, int listaUniversidade)
        {

            novo.universidadeID = listaUniversidade;

            cm.insert(novo.nome, novo.universidadeID);


            return RedirectToAction("index");
        }






        /* 
         * Editar
         *  
         */
        public ActionResult Edit(int id)
        {
            var todos = cm.GetAllCursos();
            var model = (from p in todos.Where(x => x.cursoID == id)
                         select p).FirstOrDefault();

            ViewBag.listaUniversidade = carregaListaUni(model.universidadeID);

            if (model == null)
                return View("NotFound");

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CursoDB cursoEdit, int listaUniversidade)
        {
            // move o retorno da DropDownView, caso contrário ficará com o valor antigo na hora de atualizar o registro
            cursoEdit.universidadeID = listaUniversidade;

            if (ModelState.IsValid)
            {
                cm.update(cursoEdit.cursoID, cursoEdit.nome, cursoEdit.universidadeID);
            }

            return RedirectToAction("index");
        }





        /* 
         * Detalhes
         * 
         */
        public ActionResult Details(int id)
        {
            var log = cm.Log(id);
            
            if (log == null)
                return View("NotFound");

            return PartialView(log);

        }

        /* 
         * Deletar
         * 
         */

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteView(int id)
        {

            var todos = cm.GetAllCursos();
            var model = (from p in todos.Where(x => x.cursoID == id)
                         select p).FirstOrDefault();

            if (model == null)
                return View("NotFound");

            return View(model);

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                cm.delete(id);
            }
            catch (Exception)
            {
                ViewBag.mensagem = "O curso possui alunos vinculados, não sendo permitida a exclusão do mesmo";
                return View("NotFound");
                
            }
            return RedirectToAction("index");
        }





        /*
         * Métodos auxiliares para as principais funções do controller 
         * 
         */
        public IEnumerable<SelectListItem> carregaListaUni()
        {

            UniversidadeModel uni = new UniversidadeModel();
            var lista = uni.GetAllUniversidade();

            List<SelectListItem> itens = new List<SelectListItem>();

            foreach (var item in lista)
            {
                itens.Add(new SelectListItem { Text = item.nome, Value = item.universidadeID.ToString() });
            }

            return itens;
        }

        public IEnumerable<SelectListItem> carregaListaUni(int universidade)
        {

            UniversidadeModel uni = new UniversidadeModel();
            var lista = uni.GetAllUniversidade();

            List<SelectListItem> itens = new List<SelectListItem>();

            foreach (var item in lista)
            {
                if (item.universidadeID == universidade)
                    itens.Add(new SelectListItem { Text = item.nome, Value = item.universidadeID.ToString(), Selected = true });
                else
                    itens.Add(new SelectListItem { Text = item.nome, Value = item.universidadeID.ToString(), Selected = false });


            }

            return itens;
        }




        // Tentativa de criar um DropDownList, porém não consegui mover o mesmo para a view
        //public DropDownList carregaListaUni()
        //{

        //    UniversidadeModel uni = new UniversidadeModel();
        //    var lista = uni.GetAllUniversidade();

        //    CursoDB cdb = new CursoDB();

        //    DropDownList ddlUniversidade = new DropDownList();
        //    ddlUniversidade.DataSource = lista;
        //    ddlUniversidade.DataTextField = "nome";
        //    ddlUniversidade.DataValueField = "universidadeID";
        //    ddlUniversidade.DataBind();

        //    return ddlUniversidade;
        //}
    }
}
