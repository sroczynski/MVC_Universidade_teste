using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MvcApplication1.Controllers
{
    public class NotasController : Controller
    {
        //
        // GET: /Notas/

        NotasModel no = new NotasModel();

        public ActionResult Index(string ordem, string busca, string filtro, int? pagina)
        {
            ViewBag.ordemCorrente = ordem;
            ViewBag.aluno = ordem == "alunoAsc" ? "alunoDesc" : "alunoAsc";
            ViewBag.disciplina = ordem == "disciplinaAsc" ? "disciplinaDesc" : "disciplinaAsc";
            ViewBag.nota = ordem == "notaAsc" ? "notaDesc" : "notaAsc";

            if (!String.IsNullOrEmpty(busca))
            {
                pagina = 1;
            }
            else
            {
                busca = filtro;
            }

            ViewBag.filtro = busca;

            var universidades = no.GetAllNotas(ordem, busca, filtro, pagina);

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
            ViewBag.aluno = carregaAluno();
            ViewBag.disciplina = carregaDisciplinaList(cursoAluno);
            return View();
        }

        [HttpPost]
        public ActionResult Create(NotasDB novo, int aluno, int disciplina)
        {
            try
            {

                novo.alunoID = aluno;
                novo.disciplinaID = disciplina;

                no.insert(novo.alunoID, novo.disciplinaID, novo.nota);
            }
            catch(Exception)
            {   
                ViewBag.mensagem = "Não foram informados todos os parâmetros necessários";
                return View("NotFound");
            }

            return RedirectToAction("index");
        }

        /* 
         * Editar
         *  
         */
        public ActionResult Edit(int alunoID, int disciplinaID)
        {
            NotasDB model = retornaUmRegistro(alunoID, disciplinaID);

            if (model == null)
                return View("NotFound");

            ViewBag.curso = carregaAluno(alunoID);
            ViewBag.disciplina = carregaDisciplinaList(cursoAluno);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(NotasDB notas)
        {
            if (ModelState.IsValid)
            {
                no.update(notas.alunoID, notas.disciplinaID, notas.nota);
            }
            return RedirectToAction("index");
        }

        /* 
         * Detalhes
         * 
         */
        public ActionResult Details(int alunoID, int disciplinaID)
        {

            NotasDB model = retornaUmRegistro(alunoID, disciplinaID);

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
        public ActionResult DeleteView(int alunoID, int disciplinaID)
        {
            NotasDB model = retornaUmRegistro(alunoID, disciplinaID);

            if (model == null)
                return View("NotFound");

            return View(model);

        }

        [HttpPost]
        public ActionResult Delete(NotasDB notas, int alunoID, int disciplinaID)
        {
            no.delete(alunoID, disciplinaID);

            return RedirectToAction("index");
        }

        /*
         *Alterações e métodos gerais.
         * utilizados pelas rotinas principais
         * da manutenção da base de dados
         */


        public NotasDB retornaUmRegistro(int alunoID, int disciplinaID)
        {
            var todos = no.GetAllNotas();
            var model = (from p in todos.Where(x => x.alunoID == alunoID && x.disciplinaID == disciplinaID)
                         select p).FirstOrDefault();

            return model;
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
        AlunoModel al = new AlunoModel();
        DisciplinaModel dis = new DisciplinaModel();
        CursoModel cur = new CursoModel();

        public int cursoAluno { get; set; }
        public int alunoSelect { get; set; }

        // Chamado no Create
        public IEnumerable<SelectListItem> carregaAluno()
        {
            var alunos = al.GetAllAlunos();

            List<SelectListItem> itens = new List<SelectListItem>();

            itens.Add(new SelectListItem { Text = "Selecione...", Value = 0.ToString(), Selected = true });

            foreach (var item in alunos)
            {
                itens.Add(new SelectListItem { Text = item.alunoNome, Value = item.alunoID.ToString() });
            }

            return itens;
        }

        // Chamado no Edit
        public IEnumerable<SelectListItem> carregaAluno(int aluno)
        {

            var alunos = al.GetAllAlunos();

            List<SelectListItem> itens = new List<SelectListItem>();

            alunoSelect = 0;

            itens.Add(new SelectListItem { Text = "Selecione...", Value = 0.ToString(), Selected = false });
            foreach (var item in alunos)
            {
                if (alunoSelect == 0)
                {
                    alunoSelect = aluno;
                }

                if (item.universidadeID == aluno)
                    itens.Add(new SelectListItem { Text = item.alunoNome, Value = item.alunoID.ToString(), Selected = true });
                else
                    itens.Add(new SelectListItem { Text = item.alunoNome, Value = item.alunoID.ToString(), Selected = false });
            }

            return itens;
        }

        //Chamado via Ajax/Jquery
        [ActionName("carregaDisciplinas")]
        [HttpPost]
        public JsonResult carregaDisciplinas(int aluno)
        {
            //Seleciona o objeto especifico do aluno que foi selecionado
            var todos = al.GetAllAlunos();
            var alunoDB = (from p in todos.Where(x => x.alunoID == aluno) select p).FirstOrDefault();

            var listaDisciplinas = dis.GetAllDisciplinas();

            List<SelectListItem> itens = new List<SelectListItem>();

            foreach (var item in listaDisciplinas)
            {
                if (item.cursoID == alunoDB.cursoID)
                {
                    itens.Add(new SelectListItem { Text = item.nomeDisciplina, Value = item.disciplinaID.ToString() });
                }
            }

            return Json(itens, JsonRequestBehavior.AllowGet);
        }

        // Chamado no e no create edit
        public IEnumerable<SelectListItem> carregaDisciplinaList(int curso)
        {
            DisciplinaModel dis = new DisciplinaModel();

            var listaDisciplinas = dis.GetAllDisciplinas();

            List<SelectListItem> itens = new List<SelectListItem>();

            itens.Add(new SelectListItem { Text = "Selecione...", Value = 0.ToString(), Selected = true });

            if (curso != 0)
                foreach (var item in listaDisciplinas)
                {
                    if (item.cursoID == curso)
                    {
                        itens.Add(new SelectListItem { Text = item.nomeDisciplina, Value = item.disciplinaID.ToString(), Selected = false });
                    }
                }
            return itens;
        }
    }
}
