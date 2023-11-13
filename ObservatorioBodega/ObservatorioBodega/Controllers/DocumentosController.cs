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
                "INNER JOIN Colaboradores C ON D.UsuarioID = C.ID";
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
                string query = "SELECT D.*, C.Nombre, C.Apellido FROM Documentos D " +
                      "INNER JOIN Colaboradores C ON D.UsuarioID = C.ID " +
                      "WHERE D.ID = @Id";
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
        public ActionResult Create([Bind(Include = "ID,Titulo,URL")] Documentos documento, HttpPostedFileBase pdfFile)
        {
            if (!string.IsNullOrEmpty(documento.Titulo) && !string.IsNullOrEmpty(documento.URL))
            {
                if (ModelState.IsValid)
                {
                    int Usuario = Convert.ToInt32(Session["UserID"]);
                    int rolUsuario = Convert.ToInt32(Session["UserRole"]);

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
                            string alertScript = "<script>alert('El archivo ya existe.');</script>";
                            ViewBag.AlertScript = new HtmlString(alertScript);

                            return View(documento);
                        }

                        pdfFile.SaveAs(fullPath);

                        // Asigna la ruta absoluta al campo URL
                        documento.URL = Url.Content("/Uploads/PDFs/" + fileName);

                        // Asigna los valores de Usuario y RolUsuario
                        documento.Usuario = Usuario;
                        documento.RolUsuario = rolUsuario;

                        using (IDbConnection dbConnection = Connection)
                        {
                            dbConnection.Open();
                            string query = "INSERT INTO Documentos (Titulo, UsuarioID, Rol, URL) VALUES (@Titulo, @Usuario, @RolUsuario, @URL)";
                            dbConnection.Execute(query, documento);
                        }

                        return RedirectToAction("Index");
                    }

                    return View(documento);
                }
            }
            ModelState.AddModelError("", "Todos los campos son obligatorios.");
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

        public ActionResult Edit(Documentos documento, HttpPostedFileBase pdfFile)
        {
            if (ModelState.IsValid)
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();

                    if (pdfFile != null && pdfFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(pdfFile.FileName);

                        if (!IsValidFileName(fileName))
                        {
                            ModelState.AddModelError("pdfFile", "El nombre del archivo no es válido.");
                            return View(documento);
                        }

                        string serverPath = Server.MapPath("~/Uploads/PDFs/");
                        string fullPath = Path.Combine(serverPath, fileName);

                        if (System.IO.File.Exists(fullPath))
                        {
                            string alertScript = "<script>alert('El archivo ya existe.');</script>";
                            ViewBag.AlertScript = new HtmlString(alertScript);
                            return View(documento);
                        }

                        pdfFile.SaveAs(fullPath);

                        string oldFilePath = Server.MapPath(documento.URL);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }

                        documento.URL = Url.Content("/Uploads/PDFs/" + fileName);
                    }

                    string query = "UPDATE Documentos SET Titulo = @Titulo, URL = @URL WHERE ID = @ID";
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

                // Redirige directamente a la URL del PDF
                return Redirect(pdfUrl);
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
