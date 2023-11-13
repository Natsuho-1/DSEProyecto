using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ObservatorioBodega.Models
{
    public class Documentos
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "El campo Titulo es obligatorio.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        public string UsuarioID { get; set; }
        public int Usuario { get; set; }

        [Required(ErrorMessage = "El campo Rol es obligatorio.")]
        public int Rol { get; set; }
        public int RolUsuario { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        public string Apellido { get; set; }
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo URL es obligatorio.")]
        public string URL { get; set; }

    }

}

