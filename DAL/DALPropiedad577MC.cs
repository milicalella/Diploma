using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALPropiedad577MC
    {
        DALAcceso577MC acceso = new DALAcceso577MC();

        public DataTable ObtenerTodas()
        {
            string query = @"SELECT Id, Direccion, Tipo, Estado, Precio, SuperficieM2, Ambientes, Dormitorios, Banios, DVH
                             FROM Propiedad
                             ORDER BY Id";

            return acceso.executeDataTable(query);
        }

        public DataTable Buscar(string direccion, string tipo, string estado, decimal? precioMin, decimal? precioMax)
        {
            var condiciones = new List<string>();
            var parametros = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(direccion))
            {
                condiciones.Add("Direccion LIKE @direccion");
                parametros.Add("@direccion", "%" + direccion.Trim() + "%");
            }

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                condiciones.Add("Tipo = @tipo");
                parametros.Add("@tipo", tipo);
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                condiciones.Add("Estado = @estado");
                parametros.Add("@estado", estado);
            }

            if (precioMin.HasValue)
            {
                condiciones.Add("Precio >= @precioMin");
                parametros.Add("@precioMin", precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                condiciones.Add("Precio <= @precioMax");
                parametros.Add("@precioMax", precioMax.Value);
            }

            string where = condiciones.Count > 0 ? " WHERE " + string.Join(" AND ", condiciones) : "";

            string query = @"SELECT Id, Direccion, Tipo, Estado, Precio, SuperficieM2, Ambientes, Dormitorios, Banios, DVH
                             FROM Propiedad" + where + @"
                             ORDER BY Id";

            return acceso.executeDataTable(query, parametros);
        }
    }
}