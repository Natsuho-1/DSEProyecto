using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ObservatorioBodega.Models
{
    public class Logeo
    {
        //Los datos de Usuario y Contraseña se vuelven obligatorios 
        [Required]
        public string Usuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        public int Rol { get; set; }
    }
}