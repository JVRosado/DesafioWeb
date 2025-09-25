using Microsoft.AspNetCore.Mvc;

namespace DesafioWeb.Controllers
{
    public class FundacoesController : Controller
    {
        // Método que vai retornar a view Index.cshtml
        public IActionResult Index()
        {
            return View(); // procura automaticamente Views/Fundacoes/Index.cshtml
        }

        public IActionResult Delete()
        {
            return View();
        }

    }
}
