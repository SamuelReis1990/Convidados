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

namespace Convidados_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult VerificaUsuario(string login, string senha)
        {
            string retorno = string.Empty;
            try
            {
                senha = GerarHashMd5(senha);
                using (var db = new Contexto())
                {
                    var usuario = db.Set<Usuario>();
                    if (!usuario.Any(u => u.Login.ToUpper().Equals(login.ToUpper())))
                    {
                        retorno = "Usuário não cadastrado!";
                    }
                    else
                    {
                        if (!usuario.Any(u => u.Login.ToUpper().Equals(login.ToUpper()) && u.Senha.Equals(senha)))
                        {
                            retorno = "Senha incorreta!";
                        }
                        else
                        {
                            retorno = "";
                        }
                    }
                }
            }
            catch (Exception e) { }
            return Json(retorno);
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
