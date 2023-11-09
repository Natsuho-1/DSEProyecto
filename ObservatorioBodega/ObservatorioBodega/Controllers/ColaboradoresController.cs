using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using ObservatorioBodega.Models;

namespace ObservatorioBodega.Controllers
{
    public class ColaboradoresController : Controller
    {
        private IDbConnection Connection
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                return new MySqlConnection(connectionString);
            }
        }
        // GET: Colaboradores
        public ActionResult Index()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "SELECT * FROM Colaboradores";
                var contribuidores = dbConnection.Query<Colaborador>(query);
                return View(contribuidores);
            }
        }
        //FUNCION QUE PERMITE NAVEGAR AL FORMULARIO AL DAR CLICK EN EL BOTON
        public ActionResult Formulario()
        {
            return View("formAddCollaborators");
        }
        //FUNCION PARA INSERTAR LA DATA A LA BASE
        [HttpPost]
        public ActionResult InsertarDatos(Colaborador modelo)
        {
            if (ModelState.IsValid)
            {
                // Realiza la inserción de datos en la base de datos utilizando Dapper
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (var dbConnection = new MySqlConnection(connectionString))
                {
                    dbConnection.Open();
                    string query = "INSERT INTO Colaboradores (Usuario, Correo, Contrasena, Nombre, Apellido, Rol) VALUES (@Usuario, @Correo, @Contrasena, @Nombre, @Apellido, @Rol)";
                    dbConnection.Execute(query, modelo);
                }

                TempData["Exito"] = "Los datos se insertaron correctamente.";
                // Redirecciona a la página de éxito o a donde desees
                return RedirectToAction("Index");
            }

            return View("formAddCollaborators", modelo); // Muestra el formulario nuevamente en caso de errores
        }
        public ActionResult Eliminar(int id)
        {
            // Lógica para eliminar el dato con el ID proporcionado
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                string query = "DELETE FROM Colaboradores WHERE ID = @ID";
                dbConnection.Execute(query, new { ID = id });
            }
            return RedirectToAction("Index"); // Redirige a la página principal o a donde desees
        }
        public ActionResult Editar(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                var modelo = dbConnection.QueryFirstOrDefault<Colaborador>("SELECT * FROM Colaboradores WHERE ID = @ID", new { ID = id });

                if (modelo == null)
                {
                    return HttpNotFound();
                }

                // Agregar el ID al modelo
                //modelo.ID = id;
                return View("formEditCollaborators", modelo);
            }
        }
        [HttpPost]
        public ActionResult GuardarEdicion(Colaborador modelo)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (var dbConnection = new MySqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var query = "UPDATE Colaboradores SET Usuario = @Usuario, Correo = @Correo, Contrasena = @Contrasena, Nombre = @Nombre, Apellido = @Apellido, Rol = @Rol WHERE ID = @ID";
                    dbConnection.Execute(query, modelo);

                    TempData["Exito"] = "Los cambios se guardaron correctamente.";
                }
                return RedirectToAction("Index");
            }

            return View("formEditCollaborators");
        }
        //FUNCION PARA REGRESAR AL INDEX
        public ActionResult backToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}