using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; // Asegúrate de importar el espacio de nombres

namespace ObservatorioBodega.Models
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasena { get; set; }
    }
}
