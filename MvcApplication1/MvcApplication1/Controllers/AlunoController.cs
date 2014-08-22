using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MvcApplication1.Controllers
{
    public class AlunoController : Controller
    {

        //
        // GET: /Aluno/
        AlunoModel al = new AlunoModel();

        public ActionResult Index(string ordem, string busca, string filtro, int? pagina)
        {
            ViewBag.ordemCorrente = ordem;
            ViewBag.aluno = ordem == "alunoAsc" ? "alunoDesc" : "alunoAsc";
            ViewBag.email = ordem == "emailAsc" ? "emailDesc" : "emailAsc";
            ViewBag.data = ordem == "dataAsc" ? "dataDesc" : "dataAsc";
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

            var universidades = al.GetAllAlunos(ordem, busca, filtro, pagina);

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
        public ActionResult Create(AlunoDB novo, int universidade, int curso)
        {
            novo.cursoID = curso;
            novo.universidadeID = universidade;

            try
            {
                al.insert(novo.alunoNome, novo.alunoEmail, novo.dataNascimento, novo.cursoID);
            }
            catch (Exception)
            {
                Console.WriteLine("Não foi possível inserir o aluno");
            }

            return RedirectToAction("index");
        }

        /* 
         * Editar
         *  
         */
        public ActionResult Edit(int id)
        {
            var todos = al.GetAllAlunos();
            var model = (from p in todos.Where(x => x.alunoID == id)
                         select p).FirstOrDefault();


            ViewBag.universidade = carregaUniversidade(model.universidadeID);

            ViewBag.curso = carregaCursoList(model.universidadeID, model.cursoID);

            if (model == null)
                return View("NotFound");

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AlunoDB alunoEdit, int universidade, int curso)
        {

            if (verificaSePossuiNota(alunoEdit) ||
               (alunoEdit.universidadeID == universidade && alunoEdit.cursoID == curso))
            {
                alunoEdit.universidadeID = universidade;
                alunoEdit.cursoID = curso;

                al.update(alunoEdit.alunoID, alunoEdit.alunoNome, alunoEdit.alunoEmail, alunoEdit.dataNascimento, alunoEdit.cursoID);
            }
            else
            {
                ViewBag.mensagem = "O aluno " + alunoEdit.alunoNome + " possui notas informadas, sendo que não é possível alterar o curso ou a universidade neste caso.";
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

            var todos = al.GetAllAlunos();
            var model = (from p in todos.Where(x => x.alunoID == id)
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

            var todos = al.GetAllAlunos();
            var model = (from p in todos.Where(x => x.alunoID == id)
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
                al.delete(id);
            }
            catch (Exception)
            {
                ViewBag.mensagem = "O aluno possui notas vinculadas, não sendo possível apagar o mesmo antes que suas notas sejam apagadas";
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



        public Boolean verificaSePossuiNota(AlunoDB aluno)
        {

            NotasModel notas = new NotasModel();
            var listaNotas = notas.GetAllNotas();


            foreach (var item in listaNotas)
            {
                if (item.alunoID == aluno.alunoID)
                    return false;
            }
            return true;
        }



    }
}