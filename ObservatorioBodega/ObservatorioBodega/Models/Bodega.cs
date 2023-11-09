using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace ObservatorioBodega.Models
{
    public class Bodega
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
    }
    public class DefaultConnection : DbContext
    {
        public DbSet<Bodega> Bodegas { get; set; }
    }
}