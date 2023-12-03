using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ProyectoDAS.Models
{
    public class Actividades
    {
        public List<SelectListItem> actividades { get; set; }
        public int ActividadID { get; set; }
        public string Nombre { get; set; }
        public string TipoActividad { get; set; }
        public int Dias { get; set; }
        public decimal Precio { get; set; }

    }

}