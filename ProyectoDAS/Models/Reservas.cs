using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ProyectoDAS.Models
{
    public class Reservas
    {
        public List<SelectListItem> reservas { get; set; }
        public int ReservaID { get; set; }
        public string UsuarioNombre { get; set; }
        public string DestinoNombre { get; set; }
        public string Imagen { get; set; }
        public DateTime FechaReserva { get; set; }

    }

}