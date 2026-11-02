using BE;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Servicios
{
    /// <summary>
    /// Comprobante de visita pactada (RN1).
    /// </summary>
    public static class ReporteRN1577MC
    {
        public static void Emitir(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            string titulo = ReportePdf577MC.Tr("ReporteRN1.titulo");

            try
            {
                string rutaPdf = GuardarPdf(visita, cliente, direccionPropiedad);

                MessageBox.Show(
                    string.Format(ReportePdf577MC.Tr("ReporteRN1.msgPdfGenerado"), rutaPdf),
                    titulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(ReportePdf577MC.Tr("ReporteRN1.msgErrorPdf"), ex.Message),
                    titulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            try
            {
                MostrarVistaPrevia(visita, cliente, direccionPropiedad);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(ReportePdf577MC.Tr("ReporteRN1.msgErrorVistaPrevia"), ex.Message),
                    titulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static string GuardarPdf(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            return ReportePdf577MC.GuardarPdf(
                "Comprobantes",
                string.Format("RN1_{0}.pdf", visita.IdVisita),
                (g, s) => DibujarReporte(g, s, visita, cliente, direccionPropiedad));
        }

        private static void MostrarVistaPrevia(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            ReportePdf577MC.MostrarVistaPrevia(
                ReportePdf577MC.Tr("ReporteRN1.titulo"),
                (g, s) => DibujarReporte(g, s, visita, cliente, direccionPropiedad));
        }

        private static void DibujarReporte(Graphics g, float s, Visita577MC visita, Cliente577MC cliente,
            string direccionPropiedad)
        {
            int paso;
            Color fondoEstado;
            Color colorEstado;
            string estado = EstadoLegible(visita.Estado, out paso, out fondoEstado, out colorEstado);
            string codigo = "RN1-" + visita.IdVisita.ToString("D4");
            string generado = ReportePdf577MC.Tr("ReporteRN1.generado") + ": " +
                              DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            string nombreCliente = ObtenerNombreCliente(cliente);
            string propiedad = string.IsNullOrWhiteSpace(direccionPropiedad)
                ? ReportePdf577MC.Tr("ReporteRN1.sinDatos")
                : direccionPropiedad.Trim();

            ReportePdf577MC.DibujarEncabezado(g, s);

            // Título y datos del comprobante.
            ReportePdf577MC.DibujarTexto(g, s, ReportePdf577MC.Tr("ReporteRN1.titulo"), 55, 112, 20, true,
                ReportePdf577MC.Oscuro);

            float anchoCodigo = ReportePdf577MC.AnchoPildora(g, codigo);
            ReportePdf577MC.DibujarPildora(g, s, codigo, 545 - anchoCodigo, 110,
                ReportePdf577MC.AzulSuave, ReportePdf577MC.Azul, ReportePdf577MC.Azul);

            float anchoEstado = ReportePdf577MC.AnchoPildora(g, estado);
            ReportePdf577MC.DibujarPildora(g, s, estado, 545 - anchoEstado, 140,
                fondoEstado, colorEstado, colorEstado);

            // Tarjeta principal: cliente + fecha/horario.
            ReportePdf577MC.DibujarPanel(g, s, 55, 174, 490, 104, 10, ReportePdf577MC.Papel, ReportePdf577MC.Linea);

            ReportePdf577MC.DibujarAvatar(g, s, Iniciales(cliente), 72, 200, 52);

            ReportePdf577MC.DibujarTextoAjustado(g, s, nombreCliente, 140, 206, 158, 14, true,
                ReportePdf577MC.Oscuro);

            ReportePdf577MC.DibujarTexto(g, s,
                ReportePdf577MC.Tr("ReporteRN1.dni") + " " + (visita.DNICliente ?? "-"),
                140, 232, 9, false, ReportePdf577MC.Gris);

            ReportePdf577MC.DibujarTexto(g, s, ReportePdf577MC.Tr("ReporteRN1.cliente").ToUpperInvariant(),
                140, 252, 7, true, ReportePdf577MC.GrisClaro);

            DibujarDato(g, s, 310, 198, ReportePdf577MC.Tr("ReporteRN1.fecha"),
                ReportePdf577MC.FormatearFecha(visita.Fecha));
            DibujarDato(g, s, 424, 198, ReportePdf577MC.Tr("ReporteRN1.horario"), visita.RangoHorario);

            // Propiedad.
            ReportePdf577MC.DibujarSeccion(g, s, "01", ReportePdf577MC.Tr("ReporteRN1.direccion"), 296);

            ReportePdf577MC.DibujarPanel(g, s, 55, 310, 490, 60, 10, ReportePdf577MC.Papel, ReportePdf577MC.Linea);
            ReportePdf577MC.DibujarIconoPin(g, s, 80, 326, ReportePdf577MC.Azul);
            ReportePdf577MC.DibujarTextoAjustado(g, s, propiedad, 100, 326, 420, 12.5f, true,
                ReportePdf577MC.Oscuro);

            // Línea de tiempo del estado.
            ReportePdf577MC.DibujarSeccion(g, s, "02", ReportePdf577MC.Tr("ReporteRN1.estado"), 388);
            DibujarLineaTiempo(g, s, paso);

            // Nota informativa.
            ReportePdf577MC.DibujarSeccion(g, s, "03", ReportePdf577MC.Tr("ReporteRN1.visita"), 478);

            string nota = ReportePdf577MC.Tr("ReporteRN1.nota");

            if (!string.IsNullOrWhiteSpace(nota))
            {
                ReportePdf577MC.DibujarNota(g, s, nota, 55, 492, 490, 46);
            }

            // Recomendaciones.
            ReportePdf577MC.DibujarSeccion(g, s, "04", ReportePdf577MC.Tr("ReporteRN1.recomendaciones"), 562);

            for (int i = 1; i <= 4; i++)
            {
                float y = 578 + (i - 1) * 19.5f;

                ReportePdf577MC.DibujarCheck(g, s, 56, y + 1, 9);
                ReportePdf577MC.DibujarTexto(g, s, ReportePdf577MC.Tr("ReporteRN1.rec" + i),
                    73, y, 8.8f, false, ReportePdf577MC.Oscuro);
            }

            // Código de verificación.
            ReportePdf577MC.DibujarBloqueCodigoBarras(g, s, codigo, 300, 700, 26, 0.8f,
                ReportePdf577MC.Tr("Reporte.codigoVerificacion"));

            ReportePdf577MC.DibujarPie(g, s, generado);
        }

        private static void DibujarDato(Graphics g, float s, float x, float y, string etiqueta, string valor)
        {
            ReportePdf577MC.DibujarPanel(g, s, x, y, 104, 58, 8, Color.White, ReportePdf577MC.Linea);

            ReportePdf577MC.DibujarTexto(g, s, etiqueta.ToUpperInvariant(), x + 10, y + 9, 6.8f, true,
                ReportePdf577MC.Gris);

            ReportePdf577MC.DibujarTextoAjustado(g, s, valor, x + 10, y + 26, 84, 10.5f, true,
                ReportePdf577MC.Oscuro);
        }

        private static void DibujarLineaTiempo(Graphics g, float s, int paso)
        {
            float[] nodos = { 130, 300, 470 };
            float y = 420;

            using (Pen pendiente = new Pen(ReportePdf577MC.Linea, 2f * s))
            using (Pen completado = new Pen(ReportePdf577MC.Azul, 2.4f * s))
            using (SolidBrush azul = new SolidBrush(ReportePdf577MC.Azul))
            using (SolidBrush blanco = new SolidBrush(Color.White))
            using (Pen bordeNodo = new Pen(ReportePdf577MC.Linea, 1.6f * s))
            {
                completado.StartCap = LineCap.Round;
                completado.EndCap = LineCap.Round;
                pendiente.StartCap = LineCap.Round;
                pendiente.EndCap = LineCap.Round;

                for (int i = 0; i < 2; i++)
                {
                    Pen pen = i < paso ? completado : pendiente;
                    g.DrawLine(pen, nodos[i] * s, y * s, nodos[i + 1] * s, y * s);
                }

                for (int i = 0; i < 3; i++)
                {
                    float x = nodos[i] * s;

                    if (i < paso)
                    {
                        g.FillEllipse(azul, x - 6 * s, y * s - 6 * s, 12 * s, 12 * s);
                    }
                    else if (i == paso)
                    {
                        g.FillEllipse(azul, x - 7 * s, y * s - 7 * s, 14 * s, 14 * s);
                        g.FillEllipse(blanco, x - 3 * s, y * s - 3 * s, 6 * s, 6 * s);
                    }
                    else
                    {
                        g.FillEllipse(blanco, x - 6 * s, y * s - 6 * s, 12 * s, 12 * s);
                        g.DrawEllipse(bordeNodo, x - 6 * s, y * s - 6 * s, 12 * s, 12 * s);
                    }
                }
            }

            string[] pasos =
            {
                ReportePdf577MC.Tr("ReporteRN1.pasoPactada"),
                ReportePdf577MC.Tr("ReporteRN1.pasoConfirmada"),
                ReportePdf577MC.Tr("ReporteRN1.pasoRealizada")
            };

            for (int i = 0; i < 3; i++)
            {
                Color color = i == paso ? ReportePdf577MC.Oscuro : ReportePdf577MC.Gris;
                ReportePdf577MC.DibujarTextoCentrado(g, s, pasos[i], nodos[i], 436, 8, i == paso, color);
            }
        }

        private static string EstadoLegible(string estado, out int paso, out Color fondo, out Color color)
        {
            paso = 0;
            fondo = ReportePdf577MC.NaranjaSuave;
            color = ReportePdf577MC.Naranja;

            if (string.Equals(estado, "Confirmada", StringComparison.OrdinalIgnoreCase))
            {
                paso = 1;
                fondo = ReportePdf577MC.VerdeSuave;
                color = ReportePdf577MC.Verde;

                return ReportePdf577MC.Tr("ReporteRN1.estadoConfirmada");
            }

            if (string.Equals(estado, "Realizada", StringComparison.OrdinalIgnoreCase))
            {
                paso = 2;
                fondo = ReportePdf577MC.VerdeSuave;
                color = ReportePdf577MC.Verde;

                return ReportePdf577MC.Tr("ReporteRN1.estadoRealizada");
            }

            if (string.Equals(estado, "Cancelada", StringComparison.OrdinalIgnoreCase))
            {
                fondo = ReportePdf577MC.RojoSuave;
                color = ReportePdf577MC.Rojo;

                return ReportePdf577MC.Tr("ReporteRN1.estadoCancelada");
            }

            return ReportePdf577MC.Tr("ReporteRN1.estadoPendiente");
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

        private static string ObtenerNombreCliente(Cliente577MC cliente)
        {
            if (cliente == null)
            {
                return ReportePdf577MC.Tr("ReporteRN1.sinNombre");
            }

            string nombre = (cliente.Nombre + " " + cliente.Apellido).Trim();

            return string.IsNullOrWhiteSpace(nombre)
                ? ReportePdf577MC.Tr("ReporteRN1.sinNombre")
                : nombre;
        }
    }
}
