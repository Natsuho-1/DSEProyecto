using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO; //para pdfs
using Dapper;
using System.Data;
using System.Net; 
using MySql.Data.MySqlClient;
using System.Configuration;
using ObservatorioBodega.Models;


namespace ObservatorioBodega.Controllers
{
    public class DocumentosController : Controller
    {
        private IDbConnection Connection
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                return new MySqlConnection(connectionString);
            }
        }
        // GET: Documentos
        public ActionResult Index()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "SELECT D.*, C.Nombre, C.Apellido FROM Documentos D " +
                "INNER JOIN Colaboradores C ON D.ColaboradorID = C.ID";
                var documentos = dbConnection.Query<Documentos>(query);
                return View(documentos);
            }
        }

        // GET: Documentos/Details/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "SELECT * FROM Documentos WHERE ID = @Id";
                var documento = dbConnection.QueryFirstOrDefault<Documentos>(query, new { Id = id });

                if (documento == null)
                {
                    return HttpNotFound();
                }

                return View(documento);
            }
        }

        // GET: Documentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Documentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Titulo,ColaboradorID,URL")] Documentos documento, HttpPostedFileBase pdfFile)
        {
            if (ModelState.IsValid)
            {
                if (pdfFile != null && pdfFile.ContentLength > 0)
                {
                    // Validación del nombre del archivo
                    string fileName = Path.GetFileName(pdfFile.FileName);
                    if (!IsValidFileName(fileName))
                    {
                        ModelState.AddModelError("pdfFile", "El nombre del archivo no es válido.");
                        return View(documento);
                    }

                    // Guarda el archivo PDF en el servidor
                    string serverPath = Server.MapPath("~/Uploads/PDFs/");
                    string fullPath = Path.Combine(serverPath, fileName);

                    // Verifica si el archivo ya existe
                    if (System.IO.File.Exists(fullPath))
                    {
                        ModelState.AddModelError("pdfFile", "El archivo ya existe.");
                        return View(documento);
                    }

                    pdfFile.SaveAs(fullPath);

                    // Asigna la ruta absoluta al campo URL
                    documento.URL = Url.Content("/Uploads/PDFs/" + fileName);

                    using (IDbConnection dbConnection = Connection)
                    {
                        dbConnection.Open();
                        string query = "INSERT INTO Documentos (Titulo, ColaboradorID, URL) VALUES (@Titulo, @ColaboradorID, @URL)";
                        dbConnection.Execute(query, documento);
                    }

                    return RedirectToAction("Index");
                }

                return View(documento);
            }

            return View(documento);
        }


        // GET: Documentos/Edit/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "SELECT * FROM Documentos WHERE ID = @Id";
                var documento = dbConnection.QueryFirstOrDefault<Documentos>(query, new { Id = id });

                if (documento == null)
                {
                    return HttpNotFound();
                }

                return View(documento);
            }
        }

        // POST: Documentos/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Titulo,ColaboradorID,URL")] Documentos documento)
        {
            if (ModelState.IsValid)
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    string query = "UPDATE Documentos SET Titulo = @Titulo, ColaboradorID = @ColaboradorID, URL = @URL WHERE ID = @ID";
                    dbConnection.Execute(query, documento);
                }

                return RedirectToAction("Index");
            }

            return View(documento);
        }

        // GET: Documentos/Delete/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "SELECT * FROM Documentos WHERE ID = @Id";
                var documento = dbConnection.QueryFirstOrDefault<Documentos>(query, new { Id = id });

                if (documento == null)
                {
                    return HttpNotFound();
                }

                return View(documento);
            }
        }

        // POST: Documentos/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "DELETE FROM Documentos WHERE ID = @Id";
                dbConnection.Execute(query, new { Id = id });
            }

            return RedirectToAction("Index");
        }

        // GET: Documentos/ViewPDF/
        public ActionResult ViewPDF(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string query = "SELECT URL FROM Documentos WHERE ID = @Id";
                string pdfUrl = dbConnection.QueryFirstOrDefault<string>(query, new { Id = id });

                if (pdfUrl == null)
                {
                    return HttpNotFound();
                }
                
                // Abre el PDF en una nueva ventana utilizando JavaScript
                return Content($"<script>window.open('{pdfUrl}','_blank');</script>");
            }
        }
        // Función para validar el nombre del archivo
        private bool IsValidFileName(string fileName)
        {
            // Puedes implementar tus reglas de validación aquí
            // Por ejemplo, verificar la longitud máxima y caracteres especiales no permitidos
            if (string.IsNullOrWhiteSpace(fileName) || fileName.Length > 100 || fileName.Contains(".."))
            {
                return false;
            }
            return true;
        }

    }
}
