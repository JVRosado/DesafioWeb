using DesafioWeb.Data;
using DesafioWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioWeb.Controllers
{
    public class FundacoesController : Controller
    {
        private readonly Database _database;

        public FundacoesController()
        {
            _database = new Database();
            _database.CriarTabela(); // garante que a tabela exista
        }

        // GET: Fundacoes/Index
        public IActionResult Index()
        {
            // opcional: listar todas as fundações
            var lista = _database.ListarTodas();
            return View(lista); // envia a lista para a view Index.cshtml
        }

        // GET: Fundacoes/Create
        public IActionResult Create()
        {
            return View(); // apenas exibe o formulário
        }

        // POST: Fundacoes/Create
        [HttpPost]
        public IActionResult Create(Fundacao fundacao)
        {
            try
            {
                var existente = _database.BuscarPorCNPJ(fundacao.CNPJ);
                if (existente != null)
                {
                    ViewBag.Mensagem = "CNPJ já cadastrado!";
                    return View();
                }

                _database.Inserir(fundacao);
                ViewBag.Mensagem = "Cadastro efetuado com sucesso!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = "Erro ao cadastrar: " + ex.Message;
                return View();
            }
        }

        // GET: Fundacoes/Edit
        public IActionResult Edit()
        {
            return View();
        }


        // GET: Fundacoes/Delete
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        // GET: Fundacoes/Search
        [HttpGet]
        public IActionResult Search()
        {

            return View();
        }
        // POST: Fundacoes/Search
        [HttpPost]
        public IActionResult Search(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                ViewBag.Mensagem = "Informe um CNPJ válido.";
                return View();
            }

            var fundacao = _database.BuscarPorCNPJ(cnpj);

            if (fundacao == null)
            {
                ViewBag.Mensagem = "Fundação não encontrada.";
                return View();
            }

            return View(fundacao); // envia a fundação encontrada
        }

    }

}
