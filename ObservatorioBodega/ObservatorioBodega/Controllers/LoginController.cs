using Dapper;
using MySql.Data.MySqlClient;
using ObservatorioBodega.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ObservatorioBodega.Controllers
{
    public class LoginController : Controller
    {
        private IDbConnection Connection
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                return new MySqlConnection(connectionString);
            }
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Logeo model)
        {
            if (ModelState.IsValid)
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    string query = "SELECT * FROM Colaboradores WHERE Usuario = @Usuario";

                    var user = dbConnection.Query<Logeo>(query, new { Usuario = model.Usuario }).SingleOrDefault();

                    if (user != null && user.Contrasena == model.Contrasena)
                    {
                        // Almacenar información en la sesión
                        Session["UserID"] = user.ID;
                        Session["UserName"] = user.Usuario;
                        Session["UserRole"] = user.Rol;

                        // Iniciar sesión y redirigir al index de colaboradores
                        TempData["Message"] = "Inicio de sesión exitoso.";
                        return RedirectToAction("Index", "Menus");
                    }

                    ModelState.AddModelError("", "Credenciales no válidas.");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            // Eliminar la información de la sesión al cerrar sesión
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}