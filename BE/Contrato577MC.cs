using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contrato577MC
    {
        public string NroContrato { get; set; }
        public DateTime FechaEmision { get; set; }
        public string DNIPropietario { get; set; }
        public string DNICliente { get; set; }
        public int CodigoPropiedad { get; set; }
        public string TipoContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MontoAcordado { get; set; }
        public string FormaPago { get; set; }
        public decimal DepositoGarantia { get; set; }
        public string Estado { get; set; }
        public long DVH { get; set; }

        // Solo para presentación (JOIN con Propiedad); no se persiste.
        public string PropiedadDireccion { get; set; }

        public Contrato577MC()
        {
            Estado = "Pendiente";
        }
    }
}
