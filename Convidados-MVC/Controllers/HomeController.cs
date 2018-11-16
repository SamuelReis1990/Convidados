using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Convidados_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Convidados_MVC.Controllers {
    public class HomeController : Controller {
        public IActionResult Index () {
            return View ();
        }
        public async Task<IActionResult> LogOut () {
            HttpContext.Session.Clear ();
            await HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
            return View ("Index");
        }

        [HttpPost]
        public JsonResult VerificaUsuario (string login, string senha) {
            Usuario usuario = new Usuario ();
            string retorno = string.Empty;
            try {
                senha = GerarHashMd5 (senha);
                using (var db = new Contexto ()) {
                    if (!db.Set<Usuario> ().Any (u => u.Login.ToUpper ().Equals (login.ToUpper ()))) {
                        retorno = "Usuário não cadastrado!";
                    } else {
                        if (!db.Set<Usuario> ().Any (u => u.Login.ToUpper ().Equals (login.ToUpper ()) && u.Senha.Equals (senha))) {
                            retorno = "Senha incorreta!";
                        } else {
                            usuario = db.Usuario.Where (u => u.Login.ToUpper ().Equals (login.ToUpper ()) && u.Senha.Equals (senha)).FirstOrDefault () ?? new Usuario ();
                            retorno = "";
                            HttpContext.Session.SetString ("idUsuario", usuario.Id);
                        }
                    }
                }
            } catch (Exception e) { }
            return Json (new { retorno = retorno, nomeUsuario = usuario.Nome, idUsuario = usuario.Id });
        }

        [HttpPut]
        public JsonResult AtualizaConvidado (string codConfirmacao) {
            string retorno = string.Empty;
            try {
                using (var db = new Contexto ()) {
                    if (!db.Set<Convidado> ().Any (c => c.Id.ToUpper ().Equals (codConfirmacao.ToUpper ()))) {
                        retorno = "Código informado é inválido!";
                    } else {
                        Convidado convidado = db.Convidado.Single (c => c.Id.ToUpper ().Equals (codConfirmacao.ToUpper ()));
                        convidado.Confirmacao = "S";
                        db.SaveChanges ();
                        retorno = "";
                    }
                }
            } catch (Exception e) { }
            return Json (retorno);
        }

        public static string GerarHashMd5 (string senha) {
            MD5 md5Hash = MD5.Create ();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash (Encoding.UTF8.GetBytes (senha));
            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder ();
            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append (data[i].ToString ("x2"));
            }
            return sBuilder.ToString ();
        }

        public async Task<ActionResult> Login (string nomeUsuario, string idUsuario) {
            var db = new Contexto ();
            var claims = new [] {
                new Claim (ClaimTypes.Name, idUsuario),
                new Claim (ClaimTypes.Role, "")
            };

            var identity = new ClaimsIdentity (claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync (
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal (identity));

            return RedirectToAction ("ListaConvidado", "Convidado", new { nomeUsuario = nomeUsuario, idUsuario = idUsuario });
        }
    }
}