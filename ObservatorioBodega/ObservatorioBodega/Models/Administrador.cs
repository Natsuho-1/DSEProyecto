using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObservatorioBodega.Models
{
    public class Administrador
    {
        public int ID { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public int Rol { get; set; }
    }

}