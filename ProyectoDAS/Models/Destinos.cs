using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ProyectoDAS.Models
{
    public class Destinos
    {
        public List<SelectListItem> destinos { get; set; }
        public int DestinoID { get; set; }
        public string Nombre { get; set; }
        public string ImagenURL { get; set; }
        public string Pais { get; set; }
        public string Requisitos { get; set; }

    }

}