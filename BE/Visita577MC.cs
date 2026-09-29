using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Visita577MC
    {
        public int IdVisita { get; set; }
        public int IdPropiedad { get; set; }
        public string DNICliente { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public long DVH { get; set; }

        public string PropiedadDireccion { get; set; }

        public bool EsFinalizada
        {
            get
            {
                return string.Equals(Estado, "Realizada", StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(Estado, "Cancelada", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string RangoHorario
        {
            get { return HoraInicio.ToString(@"hh\:mm") + " - " + HoraFin.ToString(@"hh\:mm"); }
        }
    }
}