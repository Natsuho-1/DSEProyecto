using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObservatorioBodega.Models
{
    public class Colaborador
    {
        public int ID { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Rol { get; set; }
    }

}