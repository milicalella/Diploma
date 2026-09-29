using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALAcceso577MC
    {
        private string _stringConnection => DALConexionConfig577MC.ObtenerConnectionString();

        private static readonly object LockLog = new object();

        public DataTable executeDataTable(string query, Dictionary<string, object> parametros = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_stringConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parametros != null)
                    {
                        foreach (var p in parametros)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    try
                    {
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        RegistrarErrorLog("executeDataTable", query, parametros, ex);
                        throw new Exception("Error de lectura en la base de datos. " + DetalleDelError(ex), ex);
                    }
                }
            }
            return dt;
        }

        public DataTable[] executeDataTableBatch(string query, Dictionary<string, object> parametros = null)
        {
            List<DataTable> tablas = new List<DataTable>();

            using (SqlConnection conn = new SqlConnection(_stringConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parametros != null)
                    {
                        foreach (var p in parametros)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    try
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        foreach (DataTable t in ds.Tables)
                        {
                            tablas.Add(t);
                        }
                    }
                    catch (SqlException ex)
                    {
                        RegistrarErrorLog("executeDataTableBatch", query, parametros, ex);
                        throw new Exception("Error de lectura en la base de datos. " + DetalleDelError(ex), ex);
                    }
                }
            }

            return tablas.ToArray();
        }

        public int executeNonQuery(string consulta, Dictionary<string, object> parametros = null)
        {
            using (SqlConnection conn = new SqlConnection(_stringConnection))
            {
                using (SqlCommand cmd = new SqlCommand(consulta, conn))
                {
                    if (parametros != null)
                    {
                        foreach (var p in parametros)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                        }
                    }
                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        RegistrarErrorLog("executeNonQuery", consulta, parametros, ex);
                        throw new Exception("Error al escribir en la base de datos. " + DetalleDelError(ex), ex);
                    }
                }
            }
        }

        public object executeScalar(string consulta, Dictionary<string, object> parametros = null)
        {
            using (SqlConnection conn = new SqlConnection(_stringConnection))
            {
                using (SqlCommand cmd = new SqlCommand(consulta, conn))
                {
                    if (parametros != null)
                    {
                        foreach (var p in parametros)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                        }
                    }

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        RegistrarErrorLog("executeScalar", consulta, parametros, ex);
                        throw new Exception("Error al leer valor escalar en la base de datos. " + DetalleDelError(ex), ex);
                    }
                }
            }
        }

        private static string DetalleDelError(SqlException ex)
        {
            switch (ex.Number)
            {
                case 2627:
                case 2601:
                    return "Clave duplicada: ya existe un registro con el mismo identificador (primary key o unique).";
                case 515:
                    return "Se intentó insertar un valor NULL en una columna que no admite valores nulos.";
                case 8152:
                case 2628:
                    return "Alguno de los campos supera la longitud máxima permitida por su columna.";
                case 207:
                    return "La consulta hace referencia a una columna inexistente en la base de datos.";
                case 208:
                    return "La consulta hace referencia a una tabla inexistente en la base de datos.";
                default:
                    return ex.Message;
            }
        }

        private static void RegistrarErrorLog(string metodo, string consulta, Dictionary<string, object> parametros, SqlException ex)
        {
            try
            {
                string carpeta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "577MC",
                    "Logs");

                Directory.CreateDirectory(carpeta);

                string archivo = Path.Combine(carpeta, "ErroresDAL.log");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]");
                sb.AppendLine($"Metodo: {metodo}");
                sb.AppendLine($"Consulta: {consulta}");

                if (parametros != null)
                {
                    sb.AppendLine("Parametros:");
                    foreach (var p in parametros)
                    {
                        object valor = p.Value ?? "<NULL>";
                        sb.AppendLine($"  {p.Key} = [{valor.GetType().Name}] {valor}");
                    }
                }
                else
                {
                    sb.AppendLine("Parametros: ninguno");
                }

                sb.AppendLine($"SQL State/Number: {ex.State}/{ex.Number}");
                sb.AppendLine($"SQL Error: {ex.Message}");
                sb.AppendLine($"Stack: {ex.StackTrace}");
                sb.AppendLine(new string('-', 80));

                lock (LockLog)
                {
                    File.AppendAllText(archivo, sb.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
                // El registro del log nunca debe interrumpir el flujo original del error.
            }
        }
    }
}
