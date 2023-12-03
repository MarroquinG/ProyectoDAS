using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ProyectoDAS.Models
{
    public class DestinosBuscados
    {
        public List<SelectListItem> destinosBuscados { get; set; }
        public int DestinoBuscadoID { get; set; }
        public string UsuarioNombre { get; set; }
        public string DestinoNombre { get; set; }
        public string imagen { get; set; }
        public DateTime FechaBusqueda { get; set; }

    }

}