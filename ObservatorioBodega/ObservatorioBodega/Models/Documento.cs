using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObservatorioBodega.Models
{
    public class Documentos
    {
        public int ID { get; set; }
        public string Titulo { get; set; }

        public string UsuarioID { get; set; }
        public int Usuario { get; set; }

        public int Rol { get; set; }
        public int RolUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime Fecha { get; set; }
        public string URL { get; set; }

    }

}

