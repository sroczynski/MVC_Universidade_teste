using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MvcApplication1.Controllers
{
    public class DisciplinaController : Controller
    {
        //
        // GET: /Disciplina/

        DisciplinaModel di = new DisciplinaModel();

        public ActionResult Index(string ordem, string busca, string filtro, int? pagina)
        {
            ViewBag.ordemCorrente = ordem;
            ViewBag.disciplina = ordem == "disciplinaAsc" ? "disciplinaDesc" : "disciplinaAsc";
            ViewBag.universidade = ordem == "universidadeAsc" ? "universidadeDesc" : "universidadeAsc";
            ViewBag.curso = ordem == "cursoAsc" ? "cursoDesc" : "cursoAsc";

            if (!String.IsNullOrEmpty(busca))
            {
                pagina = 1;
            }
            else
            {
                busca = filtro;
            }

            ViewBag.filtro = busca;

            var universidades = di.GetAllDisciplinas(ordem, busca, filtro, pagina);

            if (universidades == null)
                return View("NotFound");

            int tamanhoPagina = 5;
            int numPagina = (pagina ?? 1);

            return View(universidades.ToPagedList(numPagina, tamanhoPagina));
        }

        /* 
         * Criar
         * 
         */
        public ActionResult Create()
        {
            ViewBag.universidade = carregaUniversidade();
            ViewBag.curso = carregaCursoList(universidadeSelect, 0);

            return View();
        }

        [HttpPost]
        public ActionResult Create(DisciplinaDB novo, int universidade, int curso)
        {
            try
            {
                novo.universidadeID = universidade;
                novo.cursoID = curso;
                di.insert(novo.cursoID, novo.nomeDisciplina);

            }
            catch (Exception)
            {
                ViewBag.mensagem = "Não foi selecionado curso para a disciplina";
                return View("NotFound");
            }

            return RedirectToAction("index");
        }

        /* 
         * Editar
         *  
         */
        public ActionResult Edit(int id)
        {
            var todos = di.GetAllDisciplinas();
            var model = (from p in todos.Where(x => x.disciplinaID == id) select p).FirstOrDefault();
            if (model == null)
                return View("NotFound");

            ViewBag.universidade = carregaUniversidade(model.universidadeID);
            ViewBag.curso = carregaCursoList(universidadeSelect, model.cursoID);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DisciplinaDB disciplinaEdit, int universidade, int curso)
        {
            try
            {
                disciplinaEdit.universidadeID = universidade;
                disciplinaEdit.cursoID = curso;

                if (ModelState.IsValid)
                {
                    di.update(disciplinaEdit.disciplinaID, disciplinaEdit.cursoID, disciplinaEdit.nomeDisciplina);
                }
            }
            catch (Exception)
            {
                ViewBag.mensagem = "Não foi selecionado curso a qual pertence a disciplina";
                return View("NotFound");
            }
            return RedirectToAction("index");
        }

        /* 
         * Detalhes
         * 
         */
        public ActionResult Details(int id)
        {
            var log = di.Log(id);
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
            var todos = di.GetAllDisciplinas();
            var model = (from p in todos.Where(x => x.disciplinaID == id)
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
                di.delete(id);
            }
            catch (Exception)
            {
                ViewBag.mensagem = "Existem notas de alunos vinculados a essa disciplina, para eliminar está disciplina elimine as notas vinculadas";
                return View("NotFound");
            }
            return RedirectToAction("index");
        }


        /*********************************************************
         * 
         * 
         * 
         * 
         * Métodos auxiares
         * 
         * 
         *********************************************************/
        UniversidadeModel uni = new UniversidadeModel();
        CursoModel cur = new CursoModel();
        public int universidadeSelect { get; set; }

        // Chamado no Create
        public IEnumerable<SelectListItem> carregaUniversidade()
        {
            var listaUni = uni.GetAllUniversidade();

            List<SelectListItem> itens = new List<SelectListItem>();

            universidadeSelect = 0;

            itens.Add(new SelectListItem { Text = "Selecione...", Value = 0.ToString(), Selected = true });

            foreach (var item in listaUni)
            {
                itens.Add(new SelectListItem { Text = item.nome, Value = item.universidadeID.ToString() });
            }

            return itens;
        }

        // Chamado no Edit
        public IEnumerable<SelectListItem> carregaUniversidade(int universidade)
        {

            var listaUni = uni.GetAllUniversidade();

            List<SelectListItem> itens = new List<SelectListItem>();

            universidadeSelect = 0;

            foreach (var item in listaUni)
            {
                if (universidadeSelect == 0)
                {
                    itens.Add(new SelectListItem { Text = "Selecione...", Value = 0.ToString(), Selected = false });
                    universidadeSelect = universidade;
                }

                if (item.universidadeID == universidade)
                    itens.Add(new SelectListItem { Text = item.nome, Value = item.universidadeID.ToString(), Selected = true });
                else
                    itens.Add(new SelectListItem { Text = item.nome, Value = item.universidadeID.ToString(), Selected = false });
            }

            return itens;
        }

        //Chamado via Ajax/Jquery
        [ActionName("carregaCurso")]
        [HttpPost]
        public JsonResult carregaCurso(int universidade)
        {

            var listaCur = cur.GetAllCursos();

            List<SelectListItem> itens = new List<SelectListItem>();

            foreach (var item in listaCur)
            {
                if (item.universidadeID == universidade)
                {
                    itens.Add(new SelectListItem { Text = item.nome, Value = item.cursoID.ToString() });
                }
            }

            return Json(itens, JsonRequestBehavior.AllowGet);
        }

        // Chamado no e no create edit
        public IEnumerable<SelectListItem> carregaCursoList(int universidade, int curso)
        {

            var listaCur = cur.GetAllCursos();

            List<SelectListItem> itens = new List<SelectListItem>();


            itens.Add(new SelectListItem { Text = "Selecione...", Value = 0.ToString(), Selected = true });

            if (universidade != 0)
                foreach (var item in listaCur)
                {
                    if (item.universidadeID == universidade && universidade != 0)
                    {
                        if (curso != 0 && item.cursoID == curso)
                            itens.Add(new SelectListItem { Text = item.nome, Value = item.cursoID.ToString(), Selected = true });
                        else
                            itens.Add(new SelectListItem { Text = item.nome, Value = item.cursoID.ToString(), Selected = false });
                    }
                }

            return itens;
        }
    }
}
