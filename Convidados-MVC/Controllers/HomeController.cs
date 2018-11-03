using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Convidados_MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Convidados_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpPost]
        public JsonResult VerificaUsuario(string login, string senha)
        {
            Usuario usuario = new Usuario();
            string retorno = string.Empty;
            try
            {
                senha = GerarHashMd5(senha);
                using (var db = new Contexto())
                {                    
                    if (!db.Set<Usuario>().Any(u => u.Login.ToUpper().Equals(login.ToUpper())))
                    {
                        retorno = "Usuário não cadastrado!";
                    }
                    else
                    {
                        if (!db.Set<Usuario>().Any(u => u.Login.ToUpper().Equals(login.ToUpper()) && u.Senha.Equals(senha)))
                        {
                            retorno = "Senha incorreta!";
                        }
                        else
                        {
                            usuario = db.Usuario.Where(u => u.Login.ToUpper().Equals(login.ToUpper()) && u.Senha.Equals(senha)).FirstOrDefault() ?? new Usuario();
                            retorno = "";
                            HttpContext.Session.SetString("idUsuario", usuario.Id);
                        }
                    }
                }
            }
            catch (Exception e) { }
            return Json(new { retorno = retorno, nomeUsuario = usuario.Nome, idUsuario = usuario.Id });
        }

        public static string GerarHashMd5(string senha)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));
            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
