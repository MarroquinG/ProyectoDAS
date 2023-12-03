using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ProyectoDAS.Models
{
    public class Usuarios
    {
        public List<SelectListItem> usuarios { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }

        public string contra  { get; set; }
    public string TipoUsuario { get; set; }

    }

}