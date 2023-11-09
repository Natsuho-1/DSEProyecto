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
        public int ColaboradorID { get; set; }
        public string Nombre { get; set; } // Propiedad para el nombre del colaborador
        public string Apellido { get; set; } // Propiedad para el apellido del colaborador
        public DateTime Fecha { get; set; }
        public string URL { get; set; }

    }

}

