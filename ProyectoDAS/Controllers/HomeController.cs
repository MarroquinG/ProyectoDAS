using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoDAS.Models;

namespace ProyectoDAS.Controllers
{
    public class HomeController : Controller
    {
        private Conexion conexion = new Conexion();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarDestinos(string nombre, string pais)
        {
            if (!conexion.Conectar())
            {
                return RedirectToAction("Error", "Home");
            }

            List<Destinos> destinos = conexion.ListarDestinos(nombre, pais);
            conexion.Desconectar();

            return View(destinos);
        }

        public ActionResult DetallesDestino(int destinoID)
        {
            if (!conexion.Conectar())
            {
                return RedirectToAction("Error", "Home");
            }

            Destinos destino = conexion.ObtenerDestinoPorID(destinoID);
            List<Actividades> actividades = conexion.ObtenerActividadesPorDestino(destinoID);

            ViewBag.Actividades = actividades;

            conexion.Desconectar();

            return View(destino);
        }

        public ActionResult ReservarDestino(int destinoID, int usuarioID)
        {
            if (!conexion.Conectar())
            {
                return RedirectToAction("Error", "Home");
            }

            conexion.ReservarDestino(usuarioID, destinoID);

            // Guardar la búsqueda reciente
            conexion.GuardarBusquedaReciente(usuarioID, destinoID);

            conexion.Desconectar();

            return RedirectToAction("ListarDestinos");
        }

        public ActionResult DestinosAleatorios()
        {
            if (!conexion.Conectar())
            {
                return RedirectToAction("Error", "Home");
            }

            List<Destinos> destinosAleatorios = conexion.ObtenerDestinosAleatorios();
            conexion.Desconectar();

            return View(destinosAleatorios);
        }

        public ActionResult BusquedasRecientes()
        {
            if (!conexion.Conectar())
            {
                return RedirectToAction("Error", "Home");
            }

            List<DestinosBuscados> busquedasRecientes = conexion.ObtenerBusquedasRecientes();
            conexion.Desconectar();

            return View(busquedasRecientes);
        }


        public ActionResult VerReservas()
        {
            if (!conexion.Conectar())
            {
                return RedirectToAction("Error", "Home");
            }

            List<Reservas> reservas = conexion.ObtenerReservasPorUsuario();

            conexion.Desconectar();

            return View(reservas);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
