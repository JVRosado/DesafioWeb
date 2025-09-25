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
        [HttpGet]
        public IActionResult Edit()
        {
            return View(); // mostra o campo para pesquisar por CNPJ
        }

        // POST: Fundacoes/Edit (pesquisa a fundação pelo CNPJ)
        [HttpPost]
        public IActionResult Edit(string cnpj)
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

            return View(fundacao); // retorna os dados para edição
        }

        // POST: Fundacoes/ConfirmEdit (confirma edição e salva no banco)
        [HttpPost]
        public IActionResult ConfirmEdit(Fundacao fundacaoAtualizada)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mensagem = "Preencha todos os campos obrigatórios.";
                return View("Edit", fundacaoAtualizada);
            }

            try
            {
                _database.Atualizar(fundacaoAtualizada);
                ViewBag.Mensagem = "Fundação atualizada com sucesso!";
                return View("Edit");
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = "Erro ao atualizar: " + ex.Message;
                return View("Edit", fundacaoAtualizada);
            }
        }




        // GET: Fundacoes/Delete
        [HttpGet]
        public IActionResult Delete()
        {
            return View(); // mostra o formulário para digitar o CNPJ
        }

        // POST: Fundacoes/Delete (pesquisa o CNPJ antes de excluir)
        [HttpPost]
        public IActionResult Delete(string cnpj)
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

            return View(fundacao); // envia os dados para confirmação
        }

        // POST: Fundacoes/ConfirmDelete (confirma e executa a exclusão)
        [HttpPost]
        public IActionResult ConfirmDelete(string cnpj)
        {
            try
            {
                _database.Excluir(cnpj);
                ViewBag.Mensagem = "Fundação excluída com sucesso!";
                return View("Delete"); // volta para a página Delete limpa
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = "Erro ao excluir: " + ex.Message;
                return View("Delete");
            }
        }


        //Responsavel por fazer a busca de CNPJ e retornar as informaçoes da empresa caso encontre
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
