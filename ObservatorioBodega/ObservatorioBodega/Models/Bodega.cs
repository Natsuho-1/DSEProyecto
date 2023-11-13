using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ObservatorioBodega.Models
{
    public class Bodega
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Decripcion es obligatorio.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es obligatorio.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Por favor, ingrese solo números.")]
        public int Cantidad { get; set; }
    }
    //public class DefaultConnection : DbContext
    //{
    //    public DbSet<Bodega> Bodegas { get; set; }
    //}
}