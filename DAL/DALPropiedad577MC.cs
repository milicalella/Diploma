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

        public bool TieneVisitas(int idPropiedad)
        {
            string query = "SELECT 1 FROM Visita WHERE IdPropiedad = @idPropiedad";

            var parametros = new Dictionary<string, object>
            {
                { "@idPropiedad", idPropiedad }
            };

            DataTable dt = acceso.executeDataTable(query, parametros);

            return dt.Rows.Count > 0;
        }

        public int InsertarPropiedad(string direccion, string tipo, string estado, decimal precio, decimal superficieM2,
            int ambientes, int dormitorios, int banios)
        {
            string query = @"INSERT INTO Propiedad (Direccion, Tipo, Estado, Precio, SuperficieM2, Ambientes, Dormitorios, Banios)
                             VALUES (@direccion, @tipo, @estado, @precio, @superficieM2, @ambientes, @dormitorios, @banios);
                             SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parametros = new Dictionary<string, object>
            {
                { "@direccion", direccion },
                { "@tipo", tipo },
                { "@estado", estado },
                { "@precio", precio },
                { "@superficieM2", superficieM2 },
                { "@ambientes", ambientes },
                { "@dormitorios", dormitorios },
                { "@banios", banios }
            };

            return Convert.ToInt32(acceso.executeScalar(query, parametros));
        }

        public int ModificarPropiedad(int id, string direccion, string tipo, string estado, decimal precio, decimal superficieM2,
            int ambientes, int dormitorios, int banios, long dvh)
        {
            string query = @"UPDATE Propiedad
                             SET Direccion = @direccion, Tipo = @tipo, Estado = @estado,
                                 Precio = @precio, SuperficieM2 = @superficieM2,
                                 Ambientes = @ambientes, Dormitorios = @dormitorios, Banios = @banios, DVH = @dvh
                             WHERE Id = @id";

            var parametros = new Dictionary<string, object>
            {
                { "@id", id },
                { "@direccion", direccion },
                { "@tipo", tipo },
                { "@estado", estado },
                { "@precio", precio },
                { "@superficieM2", superficieM2 },
                { "@ambientes", ambientes },
                { "@dormitorios", dormitorios },
                { "@banios", banios },
                { "@dvh", dvh }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public int EliminarPropiedad(int id)
        {
            string query = "DELETE FROM Propiedad WHERE Id = @id";

            var parametros = new Dictionary<string, object>
            {
                { "@id", id }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public void ActualizarDVH(int id, long dvh)
        {
            string query = "UPDATE Propiedad SET DVH = @dvh WHERE Id = @id";

            var parametros = new Dictionary<string, object>
            {
                { "@dvh", dvh },
                { "@id", id }
            };

            acceso.executeNonQuery(query, parametros);
        }
    }
}