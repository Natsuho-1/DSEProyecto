using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using ObservatorioBodega.Models;

namespace ObservatorioBodega.Controllers
{
    public class BodegasController : Controller
    {
        //private DefaultConnection db = new DefaultConnection();
        private IDbConnection Connection
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                return new MySqlConnection(connectionString);
            }
        }
        // GET: Bodegas
        public ActionResult Index()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "SELECT * FROM Bodegas";
                var bodegas = dbConnection.Query<Bodega>(query);
                return View(bodegas);
            }
        }

        //FUNCION QUE PERMITE NAVEGAR AL FORMULARIO AL DAR CLICK EN EL BOTON
        public ActionResult Create()
        {
            return View("Create");
        }
        //FUNCION PARA INSERTAR LA DATA A LA BASE
        [HttpPost]
        public ActionResult InsertarDatos(Bodega modelo)
        {
            if (ModelState.IsValid)
            {
                // Realiza la inserción de datos en la base de datos utilizando Dapper
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (var dbConnection = new MySqlConnection(connectionString))
                {
                    dbConnection.Open();
                    string query = "INSERT INTO Bodegas (ID,Nombre,Descripcion,Cantidad) VALUES (@ID, @Nombre, @Descripcion, @Cantidad)";
                    dbConnection.Execute(query, modelo);
                }

                TempData["Exito"] = "Los datos se insertaron correctamente.";
                // Redirecciona a la página de éxito o a donde desees
                return RedirectToAction("Index");
            }

            return View("Create", modelo); // Muestra el formulario nuevamente en caso de errores
        }

        public ActionResult Eliminar(int id)
        {
            // Lógica para eliminar el dato con el ID proporcionado
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                string query = "DELETE FROM Bodegas WHERE ID = @ID";
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
                var modelo = dbConnection.QueryFirstOrDefault<Bodega>("SELECT * FROM Bodegas WHERE ID = @ID", new { ID = id });

                if (modelo == null)
                {
                    return HttpNotFound();
                }

                // Agregar el ID al modelo
                //modelo.ID = id;
                return View("Edit", modelo);
            }
        }
        [HttpPost]
        public ActionResult GuardarEdicion(Bodega modelo)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (var dbConnection = new MySqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var query = "UPDATE Bodegas SET Nombre = @Nombre, Descripcion = @Descripcion, Cantidad = @Cantidad WHERE ID = @ID";
                    dbConnection.Execute(query, modelo);

                    TempData["Exito"] = "Los cambios se guardaron correctamente.";
                }
                return RedirectToAction("Index");
            }

            return View("Edit");
        }
        //FUNCION PARA REGRESAR AL INDEX
        public ActionResult backToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
