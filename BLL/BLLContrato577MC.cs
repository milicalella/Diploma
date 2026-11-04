using BE;
using BE.Enum;
using DAL;
using Services;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLContrato577MC
    {
        private const int MaxIntentosNumeracion = 5;

        DALContrato577MC _dalContrato = new DALContrato577MC();
        DALCliente577MC _dalCliente = new DALCliente577MC();
        DALPropiedad577MC _dalPropiedad = new DALPropiedad577MC();
        BLLBitacora577MC _bitacora = new BLLBitacora577MC();

        private static string Tr(string key, params object[] args)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;
            return string.Format(idioma != null ? idioma.Translate(key) : key, args);
        }

        public bool ClienteExiste(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return false;
            }

            return _dalCliente.ExisteCliente(dni.Trim());
        }

        public bool PropiedadExiste(int idPropiedad)
        {
            if (idPropiedad <= 0)
            {
                return false;
            }

            return _dalPropiedad.ExistePropiedad(idPropiedad);
        }

        public void ValidarOperabilidadPropiedad(int idPropiedad)
        {
            if (idPropiedad <= 0)
            {
                throw new Exception(Tr("ContratoException.msgPropiedadInvalida"));
            }

            Propiedad577MC propiedad = ObtenerPropiedad(idPropiedad);

            if (propiedad == null)
            {
                throw new Exception(Tr("ContratoException.msgPropiedadNoExiste"));
            }

            string estado = (propiedad.Estado ?? string.Empty).Trim();

            if (!EsEstadoOperable(estado))
            {
                throw new Exception(Tr(
                    "ContratoException.msgPropiedadNoOperable",
                    propiedad.Id,
                    propiedad.Direccion,
                    estado));
            }

            string contratoPendiente = _dalContrato.ObtenerContratoPendiente(propiedad.Id);

            if (!string.IsNullOrWhiteSpace(contratoPendiente))
            {
                throw new Exception(Tr(
                    "ContratoException.msgPropiedadConContratoPendiente",
                    propiedad.Id,
                    propiedad.Direccion,
                    contratoPendiente));
            }
        }

        public List<Contrato577MC> ObtenerContratosPendientes()
        {
            return MapearContratos(_dalContrato.ObtenerPendientes());
        }

        public List<Contrato577MC> ObtenerContratosFirmados()
        {
            return MapearContratos(_dalContrato.ObtenerFirmados());
        }

        public void ActualizarEstadoContrato(string nroContrato, string decision)
        {
            if (string.IsNullOrWhiteSpace(nroContrato))
            {
                throw new Exception(Tr("ContratoException.msgDebeSeleccionarContratoPendiente"));
            }

            decision = (decision ?? string.Empty).Trim();

            if (!EsDecisionValida(decision))
            {
                throw new Exception(Tr("ContratoException.msgDebeSeleccionarNuevoEstado"));
            }

            Contrato577MC contrato = ObtenerContrato(nroContrato.Trim());

            if (contrato == null)
            {
                throw new Exception(Tr("ContratoException.msgContratoNoExiste"));
            }

            if (!string.Equals(contrato.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(Tr("ContratoException.msgContratoNoPendiente"));
            }

            if (string.Equals(decision, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                CancelarContrato(contrato);
                return;
            }

            FirmarContrato(contrato);
        }

        private void CancelarContrato(Contrato577MC contrato)
        {
            contrato.Estado = "Cancelado";
            contrato.DVH = CalcularDVHContrato(contrato);

            int afectadas = _dalContrato.ActualizarEstado(contrato.NroContrato, contrato.Estado, contrato.DVH);

            if (afectadas == 0)
            {
                throw new Exception(Tr("ContratoException.msgContratoNoPendiente"));
            }

            DigitoVerificador577MC.ActualizarDVVContrato();

            RegistrarBitacora(Modulos577MC.Contrato, string.Format(
                "Se actualizó el contrato {0} a 'Cancelado'. La propiedad {1} mantiene su disponibilidad.",
                contrato.NroContrato,
                contrato.CodigoPropiedad));
        }

        private void FirmarContrato(Contrato577MC contrato)
        {
            string estadoPropiedad = EstadoPropiedadPorTipo(contrato.TipoContrato);

            Propiedad577MC propiedad = ObtenerPropiedad(contrato.CodigoPropiedad);

            if (propiedad == null)
            {
                throw new Exception(Tr("ContratoException.msgPropiedadContratoNoExiste"));
            }

            if (!EsEstadoFirmable(propiedad.Estado))
            {
                throw new Exception(Tr(
                    "ContratoException.msgPropiedadNoFirmable",
                    contrato.NroContrato,
                    propiedad.Id,
                    propiedad.Direccion,
                    propiedad.Estado));
            }

            contrato.Estado = "Firmado";
            contrato.DVH = CalcularDVHContrato(contrato);

            propiedad.Estado = estadoPropiedad;

            long dvhPropiedad = DigitoVerificador577MC.CalcularDVHPropiedad(
                propiedad.Id,
                propiedad.Direccion.Trim(),
                propiedad.Tipo.Trim(),
                propiedad.Estado.Trim(),
                propiedad.Precio,
                propiedad.SuperficieM2,
                propiedad.Ambientes,
                propiedad.Dormitorios,
                propiedad.Banios);

            int afectadas = _dalContrato.ActualizarEstadoConPropiedad(
                contrato.NroContrato,
                contrato.Estado,
                contrato.DVH,
                propiedad.Id,
                propiedad.Estado,
                dvhPropiedad);

            if (afectadas == 0)
            {
                throw new Exception(Tr("ContratoException.msgNoSePudoActualizar"));
            }

            DigitoVerificador577MC.ActualizarDVVContrato();

            RegistrarBitacora(Modulos577MC.Contrato, string.Format(
                "Se actualizó el contrato {0} a 'Firmado' (propietario DNI {1}, cliente DNI {2}).",
                contrato.NroContrato,
                contrato.DNIPropietario,
                contrato.DNICliente));

            RegistrarBitacora(Modulos577MC.Propiedades, string.Format(
                "Se actualizó la propiedad {0} a '{1}' por la firma del contrato {2}.",
                propiedad.Id,
                propiedad.Estado,
                contrato.NroContrato));
        }

        private Contrato577MC ObtenerContrato(string nroContrato)
        {
            DataTable dt = _dalContrato.ObtenerPorNro(nroContrato);

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return MapearContrato(dt.Rows[0]);
        }

        private Propiedad577MC ObtenerPropiedad(int idPropiedad)
        {
            DataTable dt = _dalPropiedad.ObtenerPorId(idPropiedad);

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow r = dt.Rows[0];

            return new Propiedad577MC
            {
                Id = Convert.ToInt32(r["Id"]),
                Direccion = r["Direccion"].ToString(),
                Tipo = r["Tipo"].ToString(),
                Estado = r["Estado"].ToString(),
                Precio = Convert.ToDecimal(r["Precio"]),
                SuperficieM2 = Convert.ToDecimal(r["SuperficieM2"]),
                Ambientes = Convert.ToInt32(r["Ambientes"]),
                Dormitorios = Convert.ToInt32(r["Dormitorios"]),
                Banios = Convert.ToInt32(r["Banios"])
            };
        }

        private long CalcularDVHContrato(Contrato577MC contrato)
        {
            return DigitoVerificador577MC.CalcularDVHContrato(
                contrato.NroContrato,
                contrato.FechaEmision,
                contrato.DNIPropietario,
                contrato.DNICliente,
                contrato.CodigoPropiedad,
                contrato.TipoContrato,
                contrato.FechaInicio,
                contrato.FechaFin,
                contrato.MontoAcordado,
                contrato.FormaPago,
                contrato.DepositoGarantia,
                contrato.Estado);
        }

        private string EstadoPropiedadPorTipo(string tipoContrato)
        {
            string tipo = (tipoContrato ?? string.Empty).Trim();

            if (string.Equals(tipo, "Alquiler", StringComparison.OrdinalIgnoreCase))
            {
                return "Alquilada";
            }

            if (string.Equals(tipo, "Venta", StringComparison.OrdinalIgnoreCase))
            {
                return "Vendida";
            }

            throw new Exception(Tr(
                "ContratoException.msgTipoContratoNoPermiteEstado",
                tipo));
        }

        private bool EsEstadoFirmable(string estado)
        {
            return EsEstadoOperable(estado);
        }

        private bool EsEstadoOperable(string estado)
        {
            return string.Equals(estado, "Disponible", StringComparison.OrdinalIgnoreCase)
                || string.Equals(estado, "Reservada", StringComparison.OrdinalIgnoreCase);
        }

        private bool EsDecisionValida(string decision)
        {
            return string.Equals(decision, "Firmado", StringComparison.OrdinalIgnoreCase)
                || string.Equals(decision, "Cancelado", StringComparison.OrdinalIgnoreCase);
        }

        private List<Contrato577MC> MapearContratos(DataTable dt)
        {
            List<Contrato577MC> lista = new List<Contrato577MC>();

            foreach (DataRow r in dt.Rows)
            {
                lista.Add(MapearContrato(r));
            }

            return lista;
        }

        private Contrato577MC MapearContrato(DataRow r)
        {
            return new Contrato577MC
            {
                NroContrato = r["NroContrato"].ToString(),
                FechaEmision = Convert.ToDateTime(r["FechaEmision"]),
                DNIPropietario = r["DNIPropietario"].ToString(),
                DNICliente = r["DNICliente"].ToString(),
                CodigoPropiedad = Convert.ToInt32(r["CodigoPropiedad"]),
                TipoContrato = r["TipoContrato"].ToString(),
                FechaInicio = Convert.ToDateTime(r["FechaInicio"]),
                FechaFin = Convert.ToDateTime(r["FechaFin"]),
                MontoAcordado = Convert.ToDecimal(r["MontoAcordado"]),
                FormaPago = r["FormaPago"].ToString(),
                DepositoGarantia = Convert.ToDecimal(r["DepositoGarantia"]),
                Estado = r["Estado"].ToString(),
                DVH = r["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(r["DVH"]),
                PropiedadDireccion = r.Table.Columns.Contains("PropiedadDireccion") && r["PropiedadDireccion"] != DBNull.Value
                    ? r["PropiedadDireccion"].ToString()
                    : null
            };
        }

        public void ValidarTerminos(TerminosContrato577MC terminos)
        {
            if (terminos == null)
            {
                throw new Exception(Tr("ContratoException.msgTerminosObligatorios"));
            }

            terminos.TipoContrato = (terminos.TipoContrato ?? string.Empty).Trim();
            terminos.FormaPago = (terminos.FormaPago ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(terminos.TipoContrato))
            {
                throw new Exception(Tr("ContratoException.msgDebeCargarTipoContrato"));
            }

            if (string.IsNullOrWhiteSpace(terminos.FormaPago))
            {
                throw new Exception(Tr("ContratoException.msgDebeCargarFormaPago"));
            }

            if (terminos.FechaInicio == DateTime.MinValue || terminos.FechaFin == DateTime.MinValue)
            {
                throw new Exception(Tr("ContratoException.msgDebeCargarFechas"));
            }

            if (terminos.FechaFin <= terminos.FechaInicio)
            {
                throw new Exception(Tr("ContratoException.msgFechaFinPosterior"));
            }

            if (terminos.MontoAcordado <= 0)
            {
                throw new Exception(Tr("ContratoException.msgMontoMayorCero"));
            }

            if (terminos.DepositoGarantia <= 0)
            {
                throw new Exception(Tr("ContratoException.msgDepositoMayorCero"));
            }
        }

        public void ValidarYGenerarContrato(Contrato577MC contrato)
        {
            ValidarContrato(contrato);

            contrato.Estado = "Pendiente";

            if (contrato.FechaEmision == DateTime.MinValue)
            {
                contrato.FechaEmision = DateTime.Now;
            }

            Exception ultimoError = null;

            for (int intento = 0; intento < MaxIntentosNumeracion; intento++)
            {
                int secuencial = _dalContrato.ObtenerSiguienteSecuencial(contrato.FechaEmision);
                contrato.NroContrato = "C-" + contrato.FechaEmision.ToString("yyyyMMdd") + "-" + secuencial.ToString("0000");

                contrato.DVH = DigitoVerificador577MC.CalcularDVHContrato(
                    contrato.NroContrato,
                    contrato.FechaEmision,
                    contrato.DNIPropietario,
                    contrato.DNICliente,
                    contrato.CodigoPropiedad,
                    contrato.TipoContrato,
                    contrato.FechaInicio,
                    contrato.FechaFin,
                    contrato.MontoAcordado,
                    contrato.FormaPago,
                    contrato.DepositoGarantia,
                    contrato.Estado);

                try
                {
                    int afectadas = _dalContrato.InsertarContrato(
                        contrato.NroContrato,
                        contrato.FechaEmision,
                        contrato.DNIPropietario,
                        contrato.DNICliente,
                        contrato.CodigoPropiedad,
                        contrato.TipoContrato,
                        contrato.FechaInicio,
                        contrato.FechaFin,
                        contrato.MontoAcordado,
                        contrato.FormaPago,
                        contrato.DepositoGarantia,
                        contrato.Estado,
                        contrato.DVH);

                    if (afectadas == 0)
                    {
                        throw new Exception(Tr("ContratoException.msgNoSePudoGenerar"));
                    }

                    DigitoVerificador577MC.ActualizarDVVContrato();

                    RegistrarBitacora(Modulos577MC.Contrato, string.Format(
                        "Se generó el contrato {0} (propietario DNI {1}, cliente DNI {2}, propiedad {3}).",
                        contrato.NroContrato,
                        contrato.DNIPropietario,
                        contrato.DNICliente,
                        contrato.CodigoPropiedad));

                    return;
                }
                catch (Exception ex)
                {
                    if (!EsErrorDeDuplicado(ex))
                    {
                        throw;
                    }

                    ultimoError = ex;
                }
            }

            throw new Exception(Tr("ContratoException.msgNoSePudoGenerarNumero"), ultimoError);
        }

        private void ValidarContrato(Contrato577MC contrato)
        {
            if (contrato == null)
            {
                throw new Exception(Tr("ContratoException.msgDatosObligatorios"));
            }

            if (string.IsNullOrWhiteSpace(contrato.DNIPropietario))
            {
                throw new Exception(Tr("ContratoException.msgDebeIngresarDniPropietario"));
            }

            if (string.IsNullOrWhiteSpace(contrato.DNICliente))
            {
                throw new Exception(Tr("ContratoException.msgDebeIngresarDniCliente"));
            }

            contrato.DNIPropietario = contrato.DNIPropietario.Trim();
            contrato.DNICliente = contrato.DNICliente.Trim();

            if (!ClienteExiste(contrato.DNIPropietario))
            {
                throw new Exception(Tr("ContratoException.msgDniPropietarioNoRegistrado"));
            }

            if (!ClienteExiste(contrato.DNICliente))
            {
                throw new Exception(Tr("ContratoException.msgDniClienteNoRegistrado"));
            }

            if (contrato.CodigoPropiedad <= 0)
            {
                throw new Exception(Tr("ContratoException.msgPropiedadInvalida"));
            }

            ValidarOperabilidadPropiedad(contrato.CodigoPropiedad);

            TerminosContrato577MC terminos = new TerminosContrato577MC
            {
                TipoContrato = contrato.TipoContrato,
                FechaInicio = contrato.FechaInicio,
                FechaFin = contrato.FechaFin,
                MontoAcordado = contrato.MontoAcordado,
                FormaPago = contrato.FormaPago,
                DepositoGarantia = contrato.DepositoGarantia
            };

            ValidarTerminos(terminos);

            contrato.TipoContrato = terminos.TipoContrato;
            contrato.FormaPago = terminos.FormaPago;
        }

        private bool EsErrorDeDuplicado(Exception ex)
        {
            Exception actual = ex;

            while (actual != null)
            {
                SqlException sql = actual as SqlException;

                if (sql != null && (sql.Number == 2627 || sql.Number == 2601))
                {
                    return true;
                }

                actual = actual.InnerException;
            }

            return false;
        }

        private void RegistrarBitacora(Modulos577MC modulo, string evento)
        {
            string dniAutor = ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            _bitacora.registrarEvento(dniAutor, evento, Criticidad577MC.Medio, modulo);
        }
    }
}
