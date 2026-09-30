using BE;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Servicios
{
    public static class ReporteRN1577MC
    {
        private const string RutaLogoProyecto = @"C:\Users\milag\Documents\ING EN SISTEMAS\2026\Proyecto\Recursos\6295da40-cde7-4762-840d-93eb00ef0610.jpg";
        private const int AnchoPt = 595;
        private const int AltoPt = 842;
        private const int Dpi = 200;
        private const int AnchoPx = (int)(AnchoPt * Dpi / 72f);
        private const int AltoPx = (int)(AltoPt * Dpi / 72f);

        private static readonly Encoding Enc = Encoding.GetEncoding(1252);

        private static string Tr(string key)
        {
            return ServiceSessionManager577MC.getIntancia().Idioma.Translate(key);
        }

        public static void Emitir(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            try
            {
                string rutaPdf = GuardarPdf(visita, cliente, direccionPropiedad);

                MessageBox.Show(
                    string.Format(Tr("ReporteRN1.msgPdfGenerado"), rutaPdf),
                    Tr("ReporteRN1.titulo"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Tr("ReporteRN1.msgErrorPdf"), ex.Message),
                    Tr("ReporteRN1.titulo"),
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
                    string.Format(Tr("ReporteRN1.msgErrorVistaPrevia"), ex.Message),
                    Tr("ReporteRN1.titulo"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static string GuardarPdf(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            string carpeta = Path.Combine(ObtenerCarpetaDescargas(), "577MC", "Comprobantes");

            Directory.CreateDirectory(carpeta);

            string ruta = Path.Combine(carpeta, string.Format("RN1_{0}.pdf", visita.IdVisita));

            File.WriteAllBytes(ruta, ConstruirPdf(visita, cliente, direccionPropiedad));

            return ruta;
        }

        #region Construcción del PDF (página completa renderizada)

        private static byte[] ConstruirPdf(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            using (Bitmap pagina = RenderizarPagina(visita, cliente, direccionPropiedad))
            {
                byte[] jpegPagina = GuardarJpeg(pagina);
                return EnsamblarPdfConImagen(jpegPagina, pagina.Width, pagina.Height);
            }
        }

        private static Bitmap RenderizarPagina(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            Bitmap bmp = new Bitmap(AnchoPx, AltoPx, PixelFormat.Format24bppRgb);
            bmp.SetResolution(Dpi, Dpi);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                DibujarReporte(g, visita, cliente, direccionPropiedad);
            }

            return bmp;
        }

        private static byte[] GuardarJpeg(Bitmap bmp)
        {
            ImageCodecInfo codificador = null;

            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
            {
                if (string.Equals(codec.MimeType, "image/jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    codificador = codec;
                    break;
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                using (EncoderParameters parametros = new EncoderParameters(1))
                {
                    parametros.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    bmp.Save(ms, codificador, parametros);
                }

                return ms.ToArray();
            }
        }

        private static byte[] EnsamblarPdfConImagen(byte[] jpeg, int anchoPx, int altoPx)
        {
            byte[] contenidoPagina = Enc.GetBytes("q " + AnchoPt + " 0 0 " + AltoPt + " 0 0 cm /Im0 Do\nQ\n");

            var buffer = new List<byte>();
            long offset = 0;

            Action<string> ascii = s =>
            {
                byte[] b = Enc.GetBytes(s);
                buffer.AddRange(b);
                offset += b.Length;
            };

            Action<byte[]> bytes = b =>
            {
                buffer.AddRange(b);
                offset += b.Length;
            };

            ascii("%PDF-1.4\n");
            bytes(new byte[] { 0x25, 0xE2, 0xE3, 0xCF, 0xD3 });
            ascii("\n");

            long obj1 = offset;
            ascii("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");

            long obj2 = offset;
            ascii("2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");

            long obj3 = offset;
            ascii("3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 " + AnchoPt + " " + AltoPt + "] /Resources << /XObject << /Im0 4 0 R >> >> /Contents 5 0 R >>\nendobj\n");

            long obj4 = offset;
            ascii("4 0 obj\n<< /Type /XObject /Subtype /Image /Width " + anchoPx.ToString(CultureInfo.InvariantCulture) +
                " /Height " + altoPx.ToString(CultureInfo.InvariantCulture) +
                " /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length " +
                jpeg.Length.ToString(CultureInfo.InvariantCulture) + " >>\nstream\n");
            bytes(jpeg);
            ascii("\nendstream\nendobj\n");

            long obj5 = offset;
            ascii("5 0 obj\n<< /Length " + contenidoPagina.Length.ToString(CultureInfo.InvariantCulture) + " >>\nstream\n");
            bytes(contenidoPagina);
            ascii("endstream\nendobj\n");

            long xref = offset;
            ascii("xref\n0 6\n0000000000 65535 f \n");
            ascii(EscribirEntradaXref(obj1));
            ascii(EscribirEntradaXref(obj2));
            ascii(EscribirEntradaXref(obj3));
            ascii(EscribirEntradaXref(obj4));
            ascii(EscribirEntradaXref(obj5));
            ascii("trailer\n<< /Size 6 /Root 1 0 R >>\nstartxref\n");
            ascii(xref.ToString(CultureInfo.InvariantCulture) + "\n%%EOF\n");

            return buffer.ToArray();
        }

        private static string EscribirEntradaXref(long desplazamiento)
        {
            return desplazamiento.ToString("D10", CultureInfo.InvariantCulture) + " 00000 n \n";
        }

        #endregion

        #region Diseño del reporte (coordenadas de PDF 595x842, tamaño de texto en puntos)

        private static Color Azul = Color.FromArgb(43, 76, 126);
        private static Color Oscuro = Color.FromArgb(31, 36, 48);
        private static Color Gris = Color.FromArgb(107, 114, 128);
        private static Color Linea = Color.FromArgb(184, 194, 204);

        private static void DibujarReporte(Graphics g, Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            float s = g.DpiX / 72f;

            bool pendiente = string.Equals(visita.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase);
            string estado = pendiente ? Tr("ReporteRN1.estadoPendiente") : Tr("ReporteRN1.estadoConfirmada");
            string numero = "RN1-" + visita.IdVisita.ToString(CultureInfo.InvariantCulture);
            string generado = Tr("ReporteRN1.generado") + ": " + DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            string nombreCliente = ObtenerNombreCliente(cliente);
            string propiedad = string.IsNullOrWhiteSpace(direccionPropiedad) ? Tr("ReporteRN1.sinDatos") : direccionPropiedad.Trim();

            using (Font marca = Fuente(g, 26, true))
            using (Font sub = Fuente(g, 9, false))
            using (Font titulo = Fuente(g, 15, true))
            using (Font recibo = Fuente(g, 9, true))
            using (Font seccion = Fuente(g, 8.5f, true))
            using (Font etiqueta = Fuente(g, 8, false))
            using (Font valor = Fuente(g, 12, true))
            using (Font estadoF = Fuente(g, 13, true))
            using (Font pie = Fuente(g, 8.5f, false))
            using (Pen pen = new Pen(Linea, 1f))
            using (SolidBrush azul = new SolidBrush(Azul))
            using (SolidBrush oscuro = new SolidBrush(Oscuro))
            using (SolidBrush gris = new SolidBrush(Gris))
            {
                // Encabezado: marca a la izquierda, logo arriba a la derecha.
                g.DrawString("577MC", marca, azul, 55 * s, 40 * s);
                g.DrawString(Tr("ReporteRN1.encabezado"), sub, gris, 57 * s, 78 * s);

                DibujarLogo(g, s);

                g.DrawLine(pen, 50 * s, 120 * s, 545 * s, 120 * s);

                // Título (renglón propio) y número de comprobante (renglón aparte).
                g.DrawString(Tr("ReporteRN1.titulo"), titulo, oscuro, 55 * s, 132 * s);
                string comprobante = Tr("ReporteRN1.numero") + ": " + numero;
                g.DrawString(comprobante, recibo, gris, 545 * s - AnchoTexto(g, comprobante, 9, true), 168 * s);

                // Datos del cliente
                g.DrawString(Tr("ReporteRN1.cliente").ToUpperInvariant(), seccion, gris, 55 * s, 190 * s);
                g.DrawString(Tr("ReporteRN1.cliente"), etiqueta, gris, 55 * s, 216 * s);
                g.DrawString(nombreCliente, valor, oscuro, 55 * s, 238 * s);
                g.DrawString(Tr("ReporteRN1.dni"), etiqueta, gris, 300 * s, 216 * s);
                g.DrawString(visita.DNICliente, valor, oscuro, 300 * s, 238 * s);

                // Datos de la visita
                g.DrawString(Tr("ReporteRN1.visita").ToUpperInvariant(), seccion, gris, 55 * s, 282 * s);
                g.DrawString(Tr("ReporteRN1.direccion"), etiqueta, gris, 55 * s, 308 * s);
                g.DrawString(propiedad, valor, oscuro, 55 * s, 330 * s);
                g.DrawString(Tr("ReporteRN1.fecha"), etiqueta, gris, 55 * s, 368 * s);
                g.DrawString(visita.Fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), valor, oscuro, 55 * s, 390 * s);
                g.DrawString(Tr("ReporteRN1.horario"), etiqueta, gris, 300 * s, 368 * s);
                g.DrawString(visita.RangoHorario, valor, oscuro, 300 * s, 390 * s);
                g.DrawString(Tr("ReporteRN1.estado"), etiqueta, gris, 55 * s, 428 * s);
                using (SolidBrush colorEstado = new SolidBrush(pendiente ? Color.FromArgb(230, 140, 26) : Color.FromArgb(41, 158, 61)))
                {
                    g.DrawString(estado, estadoF, colorEstado, 55 * s, 452 * s);
                }

                // Pie de página
                g.DrawLine(pen, 50 * s, 640 * s, 545 * s, 640 * s);
                g.DrawString(generado, pie, gris, 58 * s, 656 * s);
                g.DrawString("577MC", pie, gris, 540 * s - AnchoTexto(g, "577MC", 8.5f, false), 656 * s);
            }
        }

        private static void DibujarLogo(Graphics g, float s)
        {
            if (!File.Exists(RutaLogoProyecto))
            {
                return;
            }

            try
            {
                using (Image logo = Image.FromFile(RutaLogoProyecto))
                {
                    float escala = Math.Min(130f / logo.Width, 42f / logo.Height);
                    float ancho = logo.Width * escala;
                    float alto = logo.Height * escala;
                    g.DrawImage(logo, 545 * s - ancho * s, 40 * s, ancho * s, alto * s);
                }
            }
            catch
            {
                // El reporte no debe fallar por no poder leer el logo.
            }
        }

        private static float AnchoTexto(Graphics g, string texto, float tamano, bool negrita)
        {
            using (Font fuente = Fuente(g, tamano, negrita))
            {
                return g.MeasureString(texto, fuente).Width;
            }
        }

        private static Font Fuente(Graphics g, float tamano, bool negrita)
        {
            return new Font("Verdana", tamano * (g.DpiX / 72f), negrita ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
        }

        #endregion

        #region Vista previa de impresión

        private static void MostrarVistaPrevia(Visita577MC visita, Cliente577MC cliente, string direccionPropiedad)
        {
            using (PrintDocument documento = new PrintDocument())
            {
                documento.PrintPage += (sender, e) =>
                {
                    DibujarReporte(e.Graphics, visita, cliente, direccionPropiedad);
                };

                using (PrintPreviewDialog vista = new PrintPreviewDialog())
                {
                    vista.Document = documento;
                    vista.UseAntiAlias = true;
                    vista.ShowDialog();
                }
            }
        }

        #endregion

        private static string ObtenerNombreCliente(Cliente577MC cliente)
        {
            if (cliente == null)
            {
                return Tr("ReporteRN1.sinNombre");
            }

            string nombre = (cliente.Nombre + " " + cliente.Apellido).Trim();

            return string.IsNullOrWhiteSpace(nombre) ? Tr("ReporteRN1.sinNombre") : nombre;
        }

        private static string ObtenerCarpetaDescargas()
        {
            try
            {
                string ruta;

                if (SHGetKnownFolderPath(NavegadorDescargas, 0, IntPtr.Zero, out ruta) == 0 && !string.IsNullOrWhiteSpace(ruta))
                {
                    return ruta;
                }
            }
            catch
            {
                // Reintento con ruta por defecto.
            }

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        }

        private static readonly Guid NavegadorDescargas = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out string ppszPath);
    }
}