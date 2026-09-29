using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALVisita577MC
    {
        DALAcceso577MC acceso = new DALAcceso577MC();

        public DataTable ObtenerVisitasPorPropiedadYFecha(int idPropiedad, DateTime fecha)
        {
            string query = @"SELECT v.IdVisita, v.IdPropiedad, v.DNICliente, v.Fecha, v.HoraInicio, v.HoraFin, v.Estado, v.Observaciones, v.DVH,
                                    p.Direccion AS PropiedadDireccion
                             FROM Visita v
                             LEFT JOIN Propiedad p ON p.Id = v.IdPropiedad
                             WHERE v.IdPropiedad = @idPropiedad AND v.Fecha = @fecha
                             ORDER BY v.HoraInicio";

            var parametros = new Dictionary<string, object>
            {
                { "@idPropiedad", idPropiedad },
                { "@fecha", fecha.Date }
            };

            return acceso.executeDataTable(query, parametros);
        }

        public DataTable ObtenerVisitas(string estado = null, DateTime? fecha = null)
        {
            var condiciones = new List<string>();
            var parametros = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(estado))
            {
                condiciones.Add("v.Estado = @estado");
                parametros.Add("@estado", estado);
            }

            if (fecha.HasValue)
            {
                condiciones.Add("v.Fecha = @fecha");
                parametros.Add("@fecha", fecha.Value.Date);
            }

            string where = condiciones.Count > 0 ? " WHERE " + string.Join(" AND ", condiciones) : "";

            string query = @"SELECT v.IdVisita, v.IdPropiedad, v.DNICliente, v.Fecha, v.HoraInicio, v.HoraFin, v.Estado, v.Observaciones, v.DVH,
                                    p.Direccion AS PropiedadDireccion
                             FROM Visita v
                             LEFT JOIN Propiedad p ON p.Id = v.IdPropiedad" + where + @"
                             ORDER BY v.Fecha DESC, v.HoraInicio";

            return acceso.executeDataTable(query, parametros);
        }

        public DataTable ObtenerVisitaPorId(int idVisita)
        {
            string query = @"SELECT v.IdVisita, v.IdPropiedad, v.DNICliente, v.Fecha, v.HoraInicio, v.HoraFin, v.Estado, v.Observaciones, v.DVH,
                                    p.Direccion AS PropiedadDireccion
                             FROM Visita v
                             LEFT JOIN Propiedad p ON p.Id = v.IdPropiedad
                             WHERE v.IdVisita = @idVisita";

            var parametros = new Dictionary<string, object>
            {
                { "@idVisita", idVisita }
            };

            return acceso.executeDataTable(query, parametros);
        }

        public int ActualizarEstadoVisita(int idVisita, string estado, string observaciones, long dvh)
        {
            string query = @"UPDATE Visita
                             SET Estado = @estado, Observaciones = @observaciones, DVH = @dvh
                             WHERE IdVisita = @idVisita";

            var parametros = new Dictionary<string, object>
            {
                { "@idVisita", idVisita },
                { "@estado", estado },
                { "@observaciones", observaciones },
                { "@dvh", dvh }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public int InsertarVisita(int idPropiedad, string dniCliente, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, string estado, string observaciones, long dvh)
        {
            string query = @"INSERT INTO Visita (IdPropiedad, DNICliente, Fecha, HoraInicio, HoraFin, Estado, Observaciones, DVH)
                             VALUES (@idPropiedad, @dniCliente, @fecha, @horaInicio, @horaFin, @estado, @observaciones, @dvh);
                             SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parametros = new Dictionary<string, object>
            {
                { "@idPropiedad", idPropiedad },
                { "@dniCliente", dniCliente },
                { "@fecha", fecha.Date },
                { "@horaInicio", horaInicio },
                { "@horaFin", horaFin },
                { "@estado", estado },
                { "@observaciones", observaciones },
                { "@dvh", dvh }
            };

            return Convert.ToInt32(acceso.executeScalar(query, parametros));
        }
    }
}