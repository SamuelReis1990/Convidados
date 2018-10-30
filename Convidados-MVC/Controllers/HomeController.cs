using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Convidados_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Convidados_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            IList<Convidado> model = new List<Convidado>();
            using (var db = new Contexto())
            {
                model = db.Convidado.OrderByDescending(o => o.DataInclusao).ToList() ?? new List<Convidado>();
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult GravarConvidado(string nome)
        {
            string retorno = string.Empty;
            using (var db = new Contexto())
            {
                var convidado = db.Set<Convidado>();
                if (convidado.Any(c => c.Nome.Equals(nome)))
                {
                    retorno = "Convidado já cadastrado!";
                }
                else
                {
                    convidado.Add(entity: new Convidado { Nome = nome });
                    db.SaveChanges();
                }
            }

            return Json(retorno);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
