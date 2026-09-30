using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALCliente577MC
    {
        DALAcceso577MC acceso = new DALAcceso577MC();

        public bool ExisteCliente(string dni)
        {
            string query = "SELECT 1 FROM Cliente WHERE DNI = @dni";

            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
            };

            DataTable dt = acceso.executeDataTable(query, parametros);

            return dt.Rows.Count > 0;
        }

        public DataTable ObtenerTodos()
        {
            string query = @"SELECT DNI, Nombre, Apellido, Telefono, Email, DVH
                             FROM Cliente
                             ORDER BY Apellido, Nombre";

            return acceso.executeDataTable(query);
        }

        public DataTable ObtenerPorDNI(string dni)
        {
            string query = @"SELECT DNI, Nombre, Apellido, Telefono, Email, DVH
                             FROM Cliente
                             WHERE DNI = @dni";

            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni.Trim() }
            };

            return acceso.executeDataTable(query, parametros);
        }

        public bool TieneVisitas(string dni)
        {
            string query = "SELECT 1 FROM Visita WHERE DNICliente = @dni";

            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
            };

            DataTable dt = acceso.executeDataTable(query, parametros);

            return dt.Rows.Count > 0;
        }

        public int ModificarCliente(string dni, string nombre, string apellido, string telefono, string email, long dvh)
        {
            string query = @"UPDATE Cliente
                             SET Nombre = @nombre, Apellido = @apellido, Telefono = @telefono, Email = @email, DVH = @dvh
                             WHERE DNI = @dni";

            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni },
                { "@nombre", nombre },
                { "@apellido", apellido },
                { "@telefono", telefono },
                { "@email", email },
                { "@dvh", dvh }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public int EliminarCliente(string dni)
        {
            string query = "DELETE FROM Cliente WHERE DNI = @dni";

            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public int InsertarCliente(string dni, string nombre, string apellido, string telefono, string email, long dvh)
        {
            string query = @"INSERT INTO Cliente (DNI, Nombre, Apellido, Telefono, Email, DVH)
                     VALUES (@dni, @nombre, @apellido, @telefono, @email, @dvh)";

            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni },
                { "@nombre", nombre },
                { "@apellido", apellido },
                { "@telefono", telefono },
                { "@email", email },
                { "@dvh", dvh }
            };

            int resultado = acceso.executeNonQuery(query, parametros);

            return resultado;
        }
    }
}