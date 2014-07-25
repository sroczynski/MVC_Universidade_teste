using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MvcApplication1.Controllers
{
    public class UniversidadeController : Controller
    {
        //
        // GET: /Universidade/

        UniversidadeModel un = new UniversidadeModel();

        public ActionResult Index(string ordem, string busca, string filtro, int? pagina)
        {
            ViewBag.ordemCorrente = ordem;
            ViewBag.nome = ordem == "nomeAsc" ? "nomeDesc" : "nomeAsc";
            ViewBag.cidade = ordem == "cidadeAsc" ? "cidadeDesc" : "cidadeAsc";


            if (!String.IsNullOrEmpty(busca))
            {
                pagina = 1;
            }
            else
	        {
                busca = filtro;
	        }

            ViewBag.filtro = busca;

            var universidades = un.GetAllUniversidade(ordem, busca, filtro, pagina);

            if (universidades == null)
                return View("NotFound");




            int tamanhoPagina = 5;
            int numPagina = (pagina ?? 1);
            
            return View(universidades.ToPagedList(numPagina,tamanhoPagina));

        }

        /* 
         * Criar
         * 
         */
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UniversidadeDB nova)
        {
            if (ModelState.IsValid)
            {
                un.insert(nova.nome, nova.cidade);
            }

            return RedirectToAction("index");
        }

        /* 
         * Editar
         *  
         */
        public ActionResult Edit(int id)
        {
            var todos = un.GetAllUniversidade();
            var model = (from p in todos.Where(x => x.universidadeID == id)select p).FirstOrDefault();
            if (model == null)
                return View("NotFound");

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UniversidadeDB uniEdit)
        {
            if (ModelState.IsValid)
            {
                un.update(uniEdit.universidadeID, uniEdit.nome, uniEdit.cidade);
            }
            return RedirectToAction("index");
        }

        /* 
         * Detalhes
         * 
         */
        public ActionResult Details(int id)
        {
            var todos = un.GetAllUniversidade();
            var model = (from p in todos.Where(x => x.universidadeID == id)
                         select p).FirstOrDefault();
            if (model == null)
                return View("NotFound");

            return View(model);

        }

        /* 
         * Deletar
         * 
         */

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteView(int id)
        {
            var todos = un.GetAllUniversidade();
            var model = (from p in todos.Where(x => x.universidadeID == id)
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
                un.delete(id);
                return RedirectToAction("index");
            }
            catch (Exception)
            {
                ViewBag.mensagem = "Não é possível excluir está universidade pois a mesma possui cursos vinculados a ela. Elimine os cursos vinculados a está universidade para prosseguir com sua exclusão!";
                return View("NotFound");
            }

        }

    }
}
