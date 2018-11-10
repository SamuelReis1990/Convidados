using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Convidados_MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Convidados_MVC.Controllers
{
    public class ConvidadoController : Controller
    {
        public IActionResult ListaConvidado(string nomeUsuario, string idUsuario)
        {
            ViewData["nomeUsuario"] = nomeUsuario;
            ViewBag.NomeUsuario = nomeUsuario;

            IList<Convidado> model = new List<Convidado>();
            try
            {
                using (var db = new Contexto())
                {
                    model = db.Convidado.OrderByDescending(o => o.DataInclusao).ToList() ?? new List<Convidado>();
                }
            }
            catch (Exception e) { }

            return View(model);
        }

        [HttpPost]
        public JsonResult GravarConvidado(string nome)
        {
            string retorno = string.Empty;
            try
            {
                using (var db = new Contexto())
                {
                    var convidado = db.Set<Convidado>();
                    if (convidado.Any(c => c.Nome.ToUpper().Equals(nome.ToUpper())))
                    {
                        retorno = "";
                    }
                    else
                    {
                        var teste = HttpContext.Session.GetString("idUsuario");
                        retorno = convidado.Add(entity: new Convidado { Nome = nome, TipoConvidado = TipoConvidado.Adulto.ToString(), IdUsuario = HttpContext.Session.GetString("idUsuario") }).Entity.Id;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e) { }

            return Json(retorno);
        }

        [HttpPut]
        public JsonResult AtualizaTipoConvidado(string codConfirmacao, string tipoConvidado)
        {
            string retorno = string.Empty;
            try
            {
                using (var db = new Contexto())
                {
                    Convidado convidado = db.Convidado.Single(c => c.Id.ToUpper().Equals(codConfirmacao.ToUpper()));
                    convidado.TipoConvidado = tipoConvidado;
                    db.SaveChanges();
                    retorno = "";
                }
            }
            catch (Exception e) { }
            return Json(retorno);
        }

        [HttpDelete]
        public JsonResult ExcluirConvidado(string id)
        {
            string retorno = string.Empty;
            try
            {
                using (var db = new Contexto())
                {
                    var convidado = db.Set<Convidado>();
                    convidado.Remove(entity: new Convidado { Id = id });
                    db.SaveChanges();
                }
            }
            catch (Exception e) { }

            return Json(retorno);
        }
    }
}
