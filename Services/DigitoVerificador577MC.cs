using BE.Enum;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class DigitoVerificador577MC
    {
        static DALDigitoVerificador577MC dalControl = new DALDigitoVerificador577MC();
        static DALBackUpRestore577MC dalBackup = new DALBackUpRestore577MC();
        static DALBitacora577MC dalBitacora = new DALBitacora577MC();

        static DALUsuario577MC dalUsuario = new DALUsuario577MC();
        static DALRol577MC dalRol = new DALRol577MC();
        static DALFamilia577MC dalFamilia = new DALFamilia577MC();
        static DALPatente577MC dalPatente = new DALPatente577MC();
        static DALContrato577MC dalContrato = new DALContrato577MC();

        public static long CalcularDVH(string cadena)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(cadena));

                long resultado = BitConverter.ToInt64(hashBytes, 0);

                return Math.Abs(resultado);
            }
        }

        public static long CalcularDVHContrato(string nroContrato, DateTime fechaEmision, string dniPropietario,
            string dniCliente, int codigoPropiedad, string tipoContrato, DateTime fechaInicio, DateTime fechaFin,
            decimal montoAcordado, string formaPago, decimal depositoGarantia, string estado)
        {
            string cadena = (nroContrato ?? string.Empty) +
                            fechaEmision.ToString("yyyyMMdd") +
                            (dniPropietario ?? string.Empty) +
                            (dniCliente ?? string.Empty) +
                            codigoPropiedad.ToString(CultureInfo.InvariantCulture) +
                            (tipoContrato ?? string.Empty) +
                            fechaInicio.ToString("yyyyMMdd") +
                            fechaFin.ToString("yyyyMMdd") +
                            montoAcordado.ToString("0.00", CultureInfo.InvariantCulture) +
                            (formaPago ?? string.Empty) +
                            depositoGarantia.ToString("0.00", CultureInfo.InvariantCulture) +
                            (estado ?? string.Empty);

            return CalcularDVH(cadena);
        }

        public static long CalcularDVHContrato(DataRow row)
        {
            return CalcularDVHContrato(
                row["NroContrato"].ToString(),
                Convert.ToDateTime(row["FechaEmision"]),
                row["DNIPropietario"].ToString(),
                row["DNICliente"].ToString(),
                Convert.ToInt32(row["CodigoPropiedad"]),
                row["TipoContrato"].ToString(),
                Convert.ToDateTime(row["FechaInicio"]),
                Convert.ToDateTime(row["FechaFin"]),
                Convert.ToDecimal(row["MontoAcordado"]),
                row["FormaPago"].ToString(),
                Convert.ToDecimal(row["DepositoGarantia"]),
                row["Estado"].ToString());
        }

        public static long CalcularDVHPropiedad(int id, string direccion, string tipo, string estado, decimal precio,
            decimal superficieM2, int ambientes, int dormitorios, int banios)
        {
            string cadena = id.ToString(CultureInfo.InvariantCulture) +
                            (direccion ?? string.Empty) +
                            (tipo ?? string.Empty) +
                            (estado ?? string.Empty) +
                            precio.ToString("0.00", CultureInfo.InvariantCulture) +
                            superficieM2.ToString("0.00", CultureInfo.InvariantCulture) +
                            ambientes.ToString(CultureInfo.InvariantCulture) +
                            dormitorios.ToString(CultureInfo.InvariantCulture) +
                            banios.ToString(CultureInfo.InvariantCulture);

            return CalcularDVH(cadena);
        }

        public static long CalcularDVV(IEnumerable<long> dvhs)
        {
            long suma = 0;
            foreach (long dvh in dvhs)
            {
                suma += dvh;
            }
            return suma;
        }

        private static long CalcularDVHControl(string nombreTabla, long dvv)
        {
            return CalcularDVH(nombreTabla + dvv);
        }

        private static void GuardarOActualizarDVV(string nombreTabla, long dvv)
        {
            long dvh = CalcularDVHControl(nombreTabla, dvv);
            if (dalControl.ExisteTabla(nombreTabla))
                dalControl.ActualizarDVV(nombreTabla, dvv, dvh);
            else
                dalControl.GuardarDVV(nombreTabla, dvv, dvh);
        }

        private static long CalcularDVHUsuario(DataRow row)
        {
            string cadena = row["DNI"].ToString() + row["Nombre"].ToString() + row["Apellido"].ToString()
                + row["Email"].ToString() + row["IdRol"].ToString() + row["Username"].ToString() + row["PasswordHash"].ToString();
            return CalcularDVH(cadena);
        }

        private static long ObtenerSumaDVHUsuario(DataTable tabla)
        {
            long suma = 0;
            foreach (DataRow row in tabla.Rows) suma += CalcularDVHUsuario(row);
            return suma;
        }

        private static long ObtenerSumaDVHUsuario() => ObtenerSumaDVHUsuario(dalUsuario.obtenerTodos());

        private static void RepararTodoUsuario()
        {
            foreach (DataRow row in dalUsuario.obtenerTodos().Rows)
            {
                long guardado = row["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(row["DVH"]);
                long calculado = CalcularDVHUsuario(row);
                if (guardado != calculado) dalUsuario.ActualizarDVH(row["DNI"].ToString(), calculado);
            }
        }

        private static long CalcularDVHFilaGenerica(DataRow row)
        {
            return CalcularDVH(row["Id"].ToString() + row["Nombre"].ToString());
        }

        private static long ObtenerSumaDVHRol(DataTable tabla) { long s = 0; foreach (DataRow r in tabla.Rows) s += CalcularDVHFilaGenerica(r); return s; }
        private static long ObtenerSumaDVHRol() => ObtenerSumaDVHRol(dalRol.obtenerTodos());
        private static void RepararTodoRol()
        {
            foreach (DataRow row in dalRol.obtenerTodos().Rows)
            {
                long g = row["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(row["DVH"]);
                long c = CalcularDVHFilaGenerica(row);
                if (g != c) dalRol.ActualizarDVH(Convert.ToInt32(row["Id"]), c);
            }
        }

        private static long ObtenerSumaDVHFamilia(DataTable tabla) { long s = 0; foreach (DataRow r in tabla.Rows) s += CalcularDVHFilaGenerica(r); return s; }
        private static long ObtenerSumaDVHFamilia() => ObtenerSumaDVHFamilia(dalFamilia.obtenerTodos());
        private static void RepararTodoFamilia()
        {
            foreach (DataRow row in dalFamilia.obtenerTodos().Rows)
            {
                long g = row["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(row["DVH"]);
                long c = CalcularDVHFilaGenerica(row);
                if (g != c) dalFamilia.ActualizarDVH(Convert.ToInt32(row["Id"]), c);
            }
        }

        private static long ObtenerSumaDVHPatente(DataTable tabla) { long s = 0; foreach (DataRow r in tabla.Rows) s += CalcularDVHFilaGenerica(r); return s; }
        private static long ObtenerSumaDVHPatente() => ObtenerSumaDVHPatente(dalPatente.obtenerTodos());
        private static void RepararTodoPatente()
        {
            foreach (DataRow row in dalPatente.obtenerTodos().Rows)
            {
                long g = row["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(row["DVH"]);
                long c = CalcularDVHFilaGenerica(row);
                if (g != c) dalPatente.ActualizarDVH(Convert.ToInt32(row["Id"]), c);
            }
        }

        private static long ObtenerSumaDVHContrato(DataTable tabla)
        {
            long suma = 0;
            foreach (DataRow row in tabla.Rows)
            {
                suma += CalcularDVHContrato(row);
            }
            return suma;
        }

        private static long ObtenerSumaDVHContrato() => ObtenerSumaDVHContrato(dalContrato.ObtenerTodos());

        private static void RepararTodoContrato()
        {
            foreach (DataRow row in dalContrato.ObtenerTodos().Rows)
            {
                long guardado = row["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(row["DVH"]);
                long calculado = CalcularDVHContrato(row);
                if (guardado != calculado)
                {
                    dalContrato.ActualizarDVH(row["NroContrato"].ToString(), calculado);
                }
            }
        }

        private static bool VerificarTablaConDatos(string nombreTabla, DataRow filaControl, DataTable tabla, Func<DataTable, long> obtenerSuma, Action reparar)
        {
            if (filaControl == null)
            {
                reparar();
                GuardarOActualizarDVV(nombreTabla, obtenerSuma(tabla));
                return true;
            }

            long dvvGuardado = Convert.ToInt64(filaControl["DVV"]);
            long dvhGuardado = Convert.ToInt64(filaControl["DVH"]);

            if (dvhGuardado != CalcularDVHControl(nombreTabla, dvvGuardado))
            {
                RegistrarDeteccion(nombreTabla);
                return false;
            }

            bool consistente = dvvGuardado == obtenerSuma(tabla);
            if (!consistente) RegistrarDeteccion(nombreTabla);
            return consistente;
        }

        public class ResultadoVerificacion
        {
            public bool Usuario { get; set; }
            public bool Rol { get; set; }
            public bool Familia { get; set; }
            public bool Patente { get; set; }
            public bool Contrato { get; set; }
        }

        public static ResultadoVerificacion VerificarIntegridad()
        {
            DataTable[] datos = dalControl.ObtenerDatosVerificacion();
            DataTable dtUsuario = datos[0];
            DataTable dtRol = datos[1];
            DataTable dtFamilia = datos[2];
            DataTable dtPatente = datos[3];
            DataTable dtContrato = datos[4];
            DataTable dtControles = datos[5];

            return new ResultadoVerificacion
            {
                Usuario = VerificarTablaConDatos("Usuario", FilaControl(dtControles, "Usuario"), dtUsuario, ObtenerSumaDVHUsuario, RepararTodoUsuario),
                Rol = VerificarTablaConDatos("Rol", FilaControl(dtControles, "Rol"), dtRol, ObtenerSumaDVHRol, RepararTodoRol),
                Familia = VerificarTablaConDatos("Familia", FilaControl(dtControles, "Familia"), dtFamilia, ObtenerSumaDVHFamilia, RepararTodoFamilia),
                Patente = VerificarTablaConDatos("Patente", FilaControl(dtControles, "Patente"), dtPatente, ObtenerSumaDVHPatente, RepararTodoPatente),
                Contrato = VerificarTablaConDatos("Contrato", FilaControl(dtControles, "Contrato"), dtContrato, ObtenerSumaDVHContrato, RepararTodoContrato)
            };
        }

        private static DataRow FilaControl(DataTable dtControles, string nombreTabla)
        {
            foreach (DataRow r in dtControles.Rows)
            {
                if (string.Equals(r["NombreTabla"].ToString(), nombreTabla, StringComparison.OrdinalIgnoreCase))
                    return r;
            }
            return null;
        }

        public static bool VerificarUsuario() => VerificarIntegridad().Usuario;
        public static bool VerificarRol() => VerificarIntegridad().Rol;
        public static bool VerificarFamilia() => VerificarIntegridad().Familia;
        public static bool VerificarPatente() => VerificarIntegridad().Patente;
        public static bool VerificarContrato() => VerificarIntegridad().Contrato;

        public static void RepararUsuario() { RepararTodoUsuario(); GuardarOActualizarDVV("Usuario", ObtenerSumaDVHUsuario()); Registrar("Usuario"); }
        public static void RepararRol() { RepararTodoRol(); GuardarOActualizarDVV("Rol", ObtenerSumaDVHRol()); Registrar("Rol"); }
        public static void RepararFamilia() { RepararTodoFamilia(); GuardarOActualizarDVV("Familia", ObtenerSumaDVHFamilia()); Registrar("Familia"); }
        public static void RepararPatente() { RepararTodoPatente(); GuardarOActualizarDVV("Patente", ObtenerSumaDVHPatente()); Registrar("Patente"); }
        public static void RepararContrato() { RepararTodoContrato(); GuardarOActualizarDVV("Contrato", ObtenerSumaDVHContrato()); Registrar("Contrato"); }

        public static void RealizarRestore(string ruta) => dalBackup.realizarRestore(ruta);

        private static void RegistrarDeteccion(string tabla)
        {
            string dni = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            dalBitacora.insertarLog(dni, $"Se detectó una inconsistencia en la tabla {tabla}.", (int)Criticidad577MC.Alto, (int)Modulos577MC.Seguridad, DateTime.Now);
        }

        private static void Registrar(string tabla)
        {
            string dni = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            dalBitacora.insertarLog(dni, $"Se reparó la tabla {tabla}.", (int)Criticidad577MC.Alto, (int)Modulos577MC.Seguridad, DateTime.Now);
        }

        public static void ActualizarDVVUsuario() => GuardarOActualizarDVV("Usuario", ObtenerSumaDVHUsuario());
        public static void ActualizarDVVRol() => GuardarOActualizarDVV("Rol", ObtenerSumaDVHRol());
        public static void ActualizarDVVFamilia() => GuardarOActualizarDVV("Familia", ObtenerSumaDVHFamilia());
        public static void ActualizarDVVPatente() => GuardarOActualizarDVV("Patente", ObtenerSumaDVHPatente());
        public static void ActualizarDVVContrato() => GuardarOActualizarDVV("Contrato", ObtenerSumaDVHContrato());
    }
}
