using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Servicios
{
    /// <summary>
    /// Copia final del contrato firmado (RN2). Reutiliza el motor visual de RN1.
    /// </summary>
    public static class ReporteRN2577MC
    {
        public static void Emitir(Contrato577MC contrato)
        {
            Emitir(contrato, true);
        }

        public static void Emitir(Contrato577MC contrato, bool esFirma)
        {
            string titulo = ReportePdf577MC.Tr("ReporteRN2.tituloVentana");
            DatosReporte datos = null;

            try
            {
                datos = ObtenerDatos(contrato);
                string rutaPdf = GuardarPdf(contrato, datos);

                MessageBox.Show(
                    string.Format(ReportePdf577MC.Tr(esFirma
                        ? "ReporteRN2.msgPdfGenerado"
                        : "ReporteRN2.msgPdfReimpreso"), rutaPdf),
                    titulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(ReportePdf577MC.Tr("ReporteRN2.msgErrorPdf"), ex.Message),
                    titulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (datos == null)
            {
                return;
            }

            try
            {
                MostrarVistaPrevia(contrato, datos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(ReportePdf577MC.Tr("ReporteRN2.msgErrorVistaPrevia"), ex.Message),
                    titulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static string GuardarPdf(Contrato577MC contrato)
        {
            return GuardarPdf(contrato, ObtenerDatos(contrato));
        }

        private static string GuardarPdf(Contrato577MC contrato, DatosReporte datos)
        {
            if (contrato == null)
            {
                throw new ArgumentNullException("contrato");
            }

            return ReportePdf577MC.GuardarPdf(
                "Contratos",
                string.Format("RN2_{0}.pdf", contrato.NroContrato),
                (g, s) => DibujarReporte(g, s, contrato, datos));
        }

        private static void MostrarVistaPrevia(Contrato577MC contrato, DatosReporte datos)
        {
            ReportePdf577MC.MostrarVistaPrevia(
                ReportePdf577MC.Tr("ReporteRN2.tituloVentana"),
                (g, s) => DibujarReporte(g, s, contrato, datos));
        }

        private static void DibujarReporte(Graphics g, float s, Contrato577MC contrato, DatosReporte datos)
        {
            string sinDatos = ReportePdf577MC.Tr("ReporteRN2.sinDatos");
            string generado = ReportePdf577MC.Tr("ReporteRN2.generado") + ": " +
                              DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            Propiedad577MC propiedad = datos == null ? null : datos.Propiedad;

            string direccion = propiedad != null && !string.IsNullOrWhiteSpace(propiedad.Direccion)
                ? propiedad.Direccion.Trim()
                : (string.IsNullOrWhiteSpace(contrato.PropiedadDireccion) ? sinDatos : contrato.PropiedadDireccion.Trim());

            ReportePdf577MC.DibujarEncabezado(g, s);

            // Título y número.
            ReportePdf577MC.DibujarTexto(g, s, TituloContrato(contrato.TipoContrato), 55, 112, 20, true,
                ReportePdf577MC.Oscuro);

            string numero = contrato.NroContrato ?? sinDatos;
            float anchoNumero = ReportePdf577MC.AnchoPildora(g, numero);
            ReportePdf577MC.DibujarPildora(g, s, numero, 545 - anchoNumero, 110,
                ReportePdf577MC.AzulSuave, ReportePdf577MC.Azul, ReportePdf577MC.Azul);

            string emision = ReportePdf577MC.Tr("ReporteRN2.emision") + ": " +
                             ReportePdf577MC.FormatearFecha(contrato.FechaEmision);
            ReportePdf577MC.DibujarTexto(g, s, emision,
                545 - ReportePdf577MC.AnchoTexto(g, emision, 7.5f, false), 142, 7.5f, false, ReportePdf577MC.Gris);

            // Partes intervinientes.
            ReportePdf577MC.DibujarSeccion(g, s, "01", ReportePdf577MC.Tr("ReporteRN2.partes"), 174);

            ReportePdf577MC.DibujarPanel(g, s, 55, 188, 490, 96, 10, ReportePdf577MC.Papel, ReportePdf577MC.Linea);

            using (Pen divisor = new Pen(ReportePdf577MC.Linea, 0.8f * s))
            {
                g.DrawLine(divisor, 300 * s, 202 * s, 300 * s, 270 * s);
            }

            DibujarPersona(g, s, 72, 214, 124,
                datos == null ? null : datos.Propietario,
                contrato.DNIPropietario,
                ReportePdf577MC.Tr("ReporteRN2.propietario"));

            DibujarPersona(g, s, 320, 214, 372,
                datos == null ? null : datos.Cliente,
                contrato.DNICliente,
                ReportePdf577MC.Tr("ReporteRN2.cliente"));

            // Propiedad.
            ReportePdf577MC.DibujarSeccion(g, s, "02", ReportePdf577MC.Tr("ReporteRN2.propiedad"), 302);

            ReportePdf577MC.DibujarPanel(g, s, 55, 316, 490, 104, 10, ReportePdf577MC.Papel, ReportePdf577MC.Linea);

            ReportePdf577MC.DibujarIconoPin(g, s, 78, 330, ReportePdf577MC.Azul);
            ReportePdf577MC.DibujarTextoAjustado(g, s, direccion, 98, 330, 420, 13, true,
                ReportePdf577MC.Oscuro);

            DibujarChipsPropiedad(g, s, contrato, propiedad, sinDatos);

            // Términos y condiciones.
            ReportePdf577MC.DibujarSeccion(g, s, "03", ReportePdf577MC.Tr("ReporteRN2.terminos"), 436);

            ReportePdf577MC.DibujarPanel(g, s, 55, 450, 490, 104, 10, ReportePdf577MC.Papel, ReportePdf577MC.Linea);

            using (Pen divisor = new Pen(ReportePdf577MC.Linea, 0.8f * s))
            {
                g.DrawLine(divisor, 72 * s, 496 * s, 528 * s, 496 * s);
            }

            ReportePdf577MC.DibujarEtiqueta(g, s, ReportePdf577MC.Tr("ReporteRN2.tipoContrato"), 72, 464);
            ReportePdf577MC.DibujarValor(g, s, contrato.TipoContrato ?? sinDatos, 72, 478, 10.5f);

            ReportePdf577MC.DibujarEtiqueta(g, s, ReportePdf577MC.Tr("ReporteRN2.fechaInicio"), 240, 464);
            ReportePdf577MC.DibujarValor(g, s, ReportePdf577MC.FormatearFecha(contrato.FechaInicio), 240, 478, 10.5f);

            ReportePdf577MC.DibujarEtiqueta(g, s, ReportePdf577MC.Tr("ReporteRN2.fechaFin"), 390, 464);
            ReportePdf577MC.DibujarValor(g, s, ReportePdf577MC.FormatearFecha(contrato.FechaFin), 390, 478, 10.5f);

            ReportePdf577MC.DibujarEtiqueta(g, s, ReportePdf577MC.Tr("ReporteRN2.formaPago"), 72, 506);
            ReportePdf577MC.DibujarValor(g, s,
                string.IsNullOrWhiteSpace(contrato.FormaPago) ? sinDatos : contrato.FormaPago, 72, 520, 10.5f);

            ReportePdf577MC.DibujarEtiqueta(g, s, ReportePdf577MC.Tr("ReporteRN2.deposito"), 240, 506);
            ReportePdf577MC.DibujarValor(g, s, ReportePdf577MC.FormatearMoneda(contrato.DepositoGarantia),
                240, 520, 10.5f);

            ReportePdf577MC.DibujarEtiqueta(g, s, ReportePdf577MC.Tr("ReporteRN2.monto"), 390, 506);
            ReportePdf577MC.DibujarTexto(g, s, ReportePdf577MC.FormatearMoneda(contrato.MontoAcordado),
                390, 518, 13.5f, true, ReportePdf577MC.Azul);

            // Estado y código de verificación.
            string estado = EstadoLegible(contrato.Estado);
            Color fondoEstado = ReportePdf577MC.VerdeSuave;
            Color colorEstado = ReportePdf577MC.Verde;

            if (string.Equals(contrato.Estado, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                fondoEstado = ReportePdf577MC.RojoSuave;
                colorEstado = ReportePdf577MC.Rojo;
            }
            else if (!string.Equals(contrato.Estado, "Firmado", StringComparison.OrdinalIgnoreCase))
            {
                fondoEstado = ReportePdf577MC.NaranjaSuave;
                colorEstado = ReportePdf577MC.Naranja;
            }

            ReportePdf577MC.DibujarEtiqueta(g, s, ReportePdf577MC.Tr("ReporteRN2.estado"), 72, 563);
            ReportePdf577MC.DibujarPildora(g, s, estado, 72, 578, fondoEstado, colorEstado, colorEstado);

            ReportePdf577MC.DibujarBloqueCodigoBarras(g, s, numero, 400, 577, 24, 0.55f,
                ReportePdf577MC.Tr("Reporte.codigoVerificacion"));

            // Cierre.
            ReportePdf577MC.DibujarNota(g, s, ReportePdf577MC.Tr("ReporteRN2.cierre"), 55, 622, 490, 54);

            // Firmas.
            DibujarFirmas(g, s, contrato, datos, sinDatos);

            ReportePdf577MC.DibujarPie(g, s, generado);
        }

        private static void DibujarPersona(Graphics g, float s, float xAvatar, float yAvatar, float xTexto,
            Cliente577MC persona, string dni, string rol)
        {
            ReportePdf577MC.DibujarAvatar(g, s, Iniciales(persona), xAvatar, yAvatar, 42);

            ReportePdf577MC.DibujarTextoAjustado(g, s, NombrePersona(persona, dni), xTexto, yAvatar - 4,
                160, 12, true, ReportePdf577MC.Oscuro);

            ReportePdf577MC.DibujarTexto(g, s, ReportePdf577MC.Tr("ReporteRN2.dni") + " " + (dni ?? "-"),
                xTexto, yAvatar + 18, 8.5f, false, ReportePdf577MC.Gris);

            ReportePdf577MC.DibujarTexto(g, s, rol.ToUpperInvariant(), xTexto, yAvatar + 36, 7, true,
                ReportePdf577MC.GrisClaro);
        }

        private static void DibujarChipsPropiedad(Graphics g, float s, Contrato577MC contrato,
            Propiedad577MC propiedad, string sinDatos)
        {
            List<string> chips = new List<string>();

            if (propiedad != null)
            {
                chips.Add(string.IsNullOrWhiteSpace(propiedad.Tipo) ? sinDatos : propiedad.Tipo);
                chips.Add(ReportePdf577MC.FormatearNumero(propiedad.SuperficieM2) + " m²");
                chips.Add(string.Format(ReportePdf577MC.Tr("ReporteRN2.chipAmbientes"), propiedad.Ambientes));
                chips.Add(string.Format(ReportePdf577MC.Tr("ReporteRN2.chipDormitorios"), propiedad.Dormitorios));
                chips.Add(string.Format(ReportePdf577MC.Tr("ReporteRN2.chipBanios"), propiedad.Banios));
            }
            else
            {
                chips.Add(sinDatos);
                chips.Add(sinDatos);
            }

            chips.Add(string.Format(ReportePdf577MC.Tr("ReporteRN2.chipCodigo"), contrato.CodigoPropiedad));

            float x = 72;
            float y = 368;

            foreach (string chip in chips)
            {
                float ancho = ReportePdf577MC.AnchoChip(g, chip);

                if (x + ancho > 528 && x > 72)
                {
                    x = 72;
                    y += 26;
                }

                ReportePdf577MC.DibujarChip(g, s, chip, x, y, ReportePdf577MC.AzulSuave, ReportePdf577MC.Azul);
                x += ancho + 7;
            }
        }

        private static void DibujarFirmas(Graphics g, float s, Contrato577MC contrato, DatosReporte datos,
            string sinDatos)
        {
            ReportePdf577MC.DibujarPanel(g, s, 55, 688, 490, 88, 10, ReportePdf577MC.Papel, ReportePdf577MC.Linea);

            float[] centros = { 137, 300, 463 };

            using (Pen linea = new Pen(ReportePdf577MC.Gris, 1f * s))
            {
                foreach (float centro in centros)
                {
                    g.DrawLine(linea, (centro - 56) * s, 706 * s, (centro + 56) * s, 706 * s);
                }
            }

            string nombrePropietario = NombrePersona(datos == null ? null : datos.Propietario,
                contrato.DNIPropietario);
            string nombreCliente = NombrePersona(datos == null ? null : datos.Cliente, contrato.DNICliente);

            DibujarFirma(g, s, 137, ReportePdf577MC.Tr("ReporteRN2.firmaPropietario"),
                nombrePropietario, contrato.DNIPropietario, sinDatos);

            DibujarFirma(g, s, 300, ReportePdf577MC.Tr("ReporteRN2.firmaCliente"),
                nombreCliente, contrato.DNICliente, sinDatos);

            DibujarFirma(g, s, 463, ReportePdf577MC.Tr("ReporteRN2.firmaEmpresa"),
                ReportePdf577MC.Marca, string.Empty, sinDatos);
        }

        private static void DibujarFirma(Graphics g, float s, float centro, string rol, string nombre,
            string dni, string sinDatos)
        {
            ReportePdf577MC.DibujarTextoCentrado(g, s, rol.ToUpperInvariant(), centro, 713, 6.8f, true,
                ReportePdf577MC.Gris);

            ReportePdf577MC.DibujarTextoCentrado(g, s, nombre, centro, 727, 8.5f, true,
                ReportePdf577MC.Oscuro);

            if (!string.IsNullOrWhiteSpace(dni))
            {
                ReportePdf577MC.DibujarTextoCentrado(g, s,
                    ReportePdf577MC.Tr("ReporteRN2.dni") + " " + dni, centro, 741, 7, false,
                    ReportePdf577MC.Gris);
            }
        }

        private static string TituloContrato(string tipoContrato)
        {
            if (string.Equals(tipoContrato, "Venta", StringComparison.OrdinalIgnoreCase))
            {
                return ReportePdf577MC.Tr("ReporteRN2.tituloVenta");
            }

            if (string.Equals(tipoContrato, "Alquiler", StringComparison.OrdinalIgnoreCase))
            {
                return ReportePdf577MC.Tr("ReporteRN2.tituloAlquiler");
            }

            return ReportePdf577MC.Tr("ReporteRN2.tituloGenerico");
        }

        private static string EstadoLegible(string estado)
        {
            if (string.Equals(estado, "Firmado", StringComparison.OrdinalIgnoreCase))
            {
                return ReportePdf577MC.Tr("ReporteRN2.estadoFirmado");
            }

            return string.IsNullOrWhiteSpace(estado)
                ? ReportePdf577MC.Tr("ReporteRN2.sinDatos")
                : estado.ToUpperInvariant();
        }

        private static string Iniciales(Cliente577MC cliente)
        {
            if (cliente == null)
            {
                return "?";
            }

            string nombre = (cliente.Nombre ?? string.Empty).Trim();
            string apellido = (cliente.Apellido ?? string.Empty).Trim();
            string iniciales = string.Empty;

            if (nombre.Length > 0)
            {
                iniciales += nombre.Substring(0, 1);
            }

            if (apellido.Length > 0)
            {
                iniciales += apellido.Substring(0, 1);
            }

            return string.IsNullOrWhiteSpace(iniciales) ? "?" : iniciales.ToUpperInvariant();
        }

        private static string NombrePersona(Cliente577MC cliente, string dni)
        {
            if (cliente == null)
            {
                return string.IsNullOrWhiteSpace(dni) ? ReportePdf577MC.Tr("ReporteRN2.sinDatos") : dni;
            }

            string nombre = (cliente.Nombre + " " + cliente.Apellido).Trim();

            return string.IsNullOrWhiteSpace(nombre) ? dni : nombre;
        }

        private static DatosReporte ObtenerDatos(Contrato577MC contrato)
        {
            if (contrato == null)
            {
                throw new ArgumentNullException("contrato");
            }

            BLLCliente577MC clientes = new BLLCliente577MC();
            BLLPropiedad577MC propiedades = new BLLPropiedad577MC();

            return new DatosReporte
            {
                Propietario = clientes.ObtenerPorDNI(contrato.DNIPropietario),
                Cliente = clientes.ObtenerPorDNI(contrato.DNICliente),
                Propiedad = propiedades.ObtenerPorId(contrato.CodigoPropiedad)
            };
        }

        private sealed class DatosReporte
        {
            public Cliente577MC Propietario { get; set; }
            public Cliente577MC Cliente { get; set; }
            public Propiedad577MC Propiedad { get; set; }
        }
    }
}
