using BE;
using BE.Enum;
using DAL;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLVisita577MC
    {
        private static readonly string[] EstadosValidos = { "Pendiente", "Realizada", "Cancelada" };
        private static readonly string[] EstadosActualizables = { "Realizada", "Cancelada" };

        private static string Tr(string key, params object[] args)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;
            return string.Format(idioma != null ? idioma.Translate(key) : key, args);
        }

        DALVisita577MC dal = new DALVisita577MC();
        BLLBitacora577MC bit = new BLLBitacora577MC();

        public List<Visita577MC> ObtenerVisitasPorPropiedadYFecha(int idPropiedad, DateTime fecha)
        {
            DataTable dt = dal.ObtenerVisitasPorPropiedadYFecha(idPropiedad, fecha);
            return Mapear(dt);
        }

        public List<Visita577MC> ObtenerVisitas(string estado = null, DateTime? fecha = null)
        {
            DataTable dt = dal.ObtenerVisitas(estado, fecha);
            return Mapear(dt);
        }

        public Visita577MC ObtenerVisitaPorId(int idVisita)
        {
            DataTable dt = dal.ObtenerVisitaPorId(idVisita);

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return Mapear(dt).First();
        }

        public bool ConsultarDisponibilidad(int idPropiedad, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            ValidarConsulta(idPropiedad, fecha, horaInicio, horaFin);

            List<Visita577MC> visitas = ObtenerVisitasPorPropiedadYFecha(idPropiedad, fecha);
            bool disponible = !HaySuperposicion(visitas, horaInicio, horaFin);

            return disponible;
        }

        public int RegistrarVisita(Visita577MC visita)
        {
            ValidarConsulta(visita.IdPropiedad, visita.Fecha, visita.HoraInicio, visita.HoraFin);
            ValidarDNICliente(visita.DNICliente);
            ValidarEstado(visita.Estado);

            List<Visita577MC> visitas = ObtenerVisitasPorPropiedadYFecha(visita.IdPropiedad, visita.Fecha);
            if (HaySuperposicion(visitas, visita.HoraInicio, visita.HoraFin))
            {
                throw new Exception(Tr("VisitaException.msgSuperposicion"));
            }

            long dvh = CalcularDVH(visita);

            int idVisita = dal.InsertarVisita(visita.IdPropiedad, visita.DNICliente.Trim(), visita.Fecha, visita.HoraInicio, visita.HoraFin, visita.Estado, visita.Observaciones, dvh);

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Se registró una visita {(visita.Estado)} del cliente {visita.DNICliente} para la propiedad {visita.IdPropiedad} el {visita.Fecha:dd/MM/yyyy} ({visita.RangoHorario}).", Criticidad577MC.Medio, Modulos577MC.Visita);

            return idVisita;
        }

        public void ActualizarEstadoVisita(int idVisita, string nuevoEstado, string observaciones)
        {
            ValidarEstadoActualizable(nuevoEstado);

            Visita577MC visita = ObtenerVisitaPorId(idVisita);

            if (visita == null)
            {
                throw new Exception(Tr("VisitaException.msgNoEncontrada"));
            }

            if (visita.EsFinalizada)
            {
                throw new Exception(Tr("VisitaException.msgFinalizada", visita.Estado));
            }

            visita.Estado = nuevoEstado;
            visita.Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim();

            long dvh = CalcularDVH(visita);

            int afectadas = dal.ActualizarEstadoVisita(idVisita, visita.Estado, visita.Observaciones, dvh);

            if (afectadas == 0)
            {
                throw new Exception(Tr("VisitaException.msgNoActualizada"));
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Se actualizó el estado de la visita {idVisita} a '{visita.Estado}' para el cliente {visita.DNICliente}.", Criticidad577MC.Medio, Modulos577MC.Visita);
        }

        private void ValidarConsulta(int idPropiedad, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
        {
            if (idPropiedad <= 0)
            {
                throw new Exception(Tr("VisitaException.msgSeleccionarPropiedad"));
            }

            if (fecha.Date < DateTime.Today)
            {
                throw new Exception(Tr("VisitaException.msgFechaAnterior"));
            }

            if (horaInicio >= horaFin)
            {
                throw new Exception(Tr("VisitaException.msgHoraInvalida"));
            }
        }

        private void ValidarDNICliente(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                throw new Exception(Tr("VisitaException.msgDNIRequerido"));
            }

            string limpio = dni.Trim();

            if (limpio.Length < 7 || limpio.Length > 8 || limpio.Any(c => !char.IsDigit(c)))
            {
                throw new Exception(Tr("VisitaException.msgDNIInvalido"));
            }
        }

        private void ValidarEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado) || !EstadosValidos.Any(e => string.Equals(e, estado, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception(Tr("VisitaException.msgEstadoInvalido", estado, string.Join(", ", EstadosValidos)));
            }
        }

        private void ValidarEstadoActualizable(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado) || !EstadosActualizables.Any(e => string.Equals(e, estado, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception(Tr("VisitaException.msgEstadoActualizableInvalido", estado, string.Join(", ", EstadosActualizables)));
            }
        }

        private bool HaySuperposicion(List<Visita577MC> visitas, TimeSpan horaInicio, TimeSpan horaFin)
        {
            return visitas.Any(v =>
                (string.Equals(v.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(v.Estado, "Realizada", StringComparison.OrdinalIgnoreCase)) &&
                horaInicio < v.HoraFin &&
                v.HoraInicio < horaFin);
        }

        private long CalcularDVH(Visita577MC visita)
        {
            string cadena = visita.IdPropiedad.ToString() +
                            visita.DNICliente.Trim() +
                            visita.Fecha.ToString("yyyyMMdd") +
                            visita.HoraInicio.Hours.ToString("00") +
                            visita.HoraInicio.Minutes.ToString("00") +
                            visita.HoraFin.Hours.ToString("00") +
                            visita.HoraFin.Minutes.ToString("00") +
                            visita.Estado +
                            (visita.Observaciones ?? "");

            return DigitoVerificador577MC.CalcularDVH(cadena);
        }

        private List<Visita577MC> Mapear(DataTable dt)
        {
            List<Visita577MC> lista = new List<Visita577MC>();

            foreach (DataRow r in dt.Rows)
            {
                lista.Add(new Visita577MC
                {
                    IdVisita = Convert.ToInt32(r["IdVisita"]),
                    IdPropiedad = Convert.ToInt32(r["IdPropiedad"]),
                    DNICliente = r["DNICliente"].ToString(),
                    Fecha = Convert.ToDateTime(r["Fecha"]),
                    HoraInicio = (TimeSpan)r["HoraInicio"],
                    HoraFin = (TimeSpan)r["HoraFin"],
                    Estado = r["Estado"].ToString(),
                    Observaciones = r["Observaciones"] == DBNull.Value ? null : r["Observaciones"].ToString(),
                    DVH = r["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(r["DVH"]),
                    PropiedadDireccion = r["PropiedadDireccion"] == DBNull.Value ? "" : r["PropiedadDireccion"].ToString()
                });
            }

            return lista;
        }
    }
}