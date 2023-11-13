using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ObservatorioBodega.Models
{
    public class Colaborador
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        public string Apellido { get; set; }
        public int Rol { get; set; }
    }

}