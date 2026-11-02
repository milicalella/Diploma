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
using Services_577MC;

namespace Servicios
{
    /// <summary>
    /// Motor común de los reportes PDF: renderiza una página A4 con GDI+,
    /// la comprime como JPEG y arma el archivo PDF sin librerías externas.
    /// También concentra las piezas visuales compartidas por RN1 y RN2.
    /// </summary>
    internal static class ReportePdf577MC
    {
        public const string Marca = "Propiedades Calella";

        public const int AnchoPt = 595;
        public const int AltoPt = 842;
        public const int Dpi = 200;
        public const int AnchoPx = (int)(AnchoPt * Dpi / 72f);
        public const int AltoPx = (int)(AltoPt * Dpi / 72f);

        private const string ArchivoLogo = "logo-propiedades-calella.jpg";

        public static readonly Color Azul = Color.FromArgb(43, 76, 126);
        public static readonly Color AzulProfundo = Color.FromArgb(27, 53, 87);
        public static readonly Color Oscuro = Color.FromArgb(31, 36, 48);
        public static readonly Color Gris = Color.FromArgb(107, 114, 128);
        public static readonly Color GrisClaro = Color.FromArgb(156, 163, 175);
        public static readonly Color Linea = Color.FromArgb(214, 221, 230);
        public static readonly Color Papel = Color.FromArgb(247, 249, 252);
        public static readonly Color Verde = Color.FromArgb(32, 132, 88);
        public static readonly Color VerdeSuave = Color.FromArgb(228, 244, 236);
        public static readonly Color Naranja = Color.FromArgb(196, 116, 20);
        public static readonly Color NaranjaSuave = Color.FromArgb(253, 239, 222);
        public static readonly Color Rojo = Color.FromArgb(176, 54, 46);
        public static readonly Color RojoSuave = Color.FromArgb(251, 234, 232);
        public static readonly Color AzulSuave = Color.FromArgb(233, 239, 247);

        private static readonly string Familia = ResolverFamilia("Segoe UI", "Verdana");

        private static readonly Encoding Enc = Encoding.GetEncoding(1252);
        private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-AR");
        private static readonly Guid NavegadorDescargas = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        private static readonly StringFormat SfTipografico = CrearFormatoTipografico();

        public static string Tr(string key)
        {
            return ServiceSessionManager577MC.getIntancia().Idioma.Translate(key);
        }

        #region Generación de archivos

        public static string GuardarPdf(string subcarpeta, string nombreArchivo, Action<Graphics, float> dibujar)
        {
            string carpeta = Path.Combine(ObtenerCarpetaDescargas(), Marca, subcarpeta);

            Directory.CreateDirectory(carpeta);

            string ruta = Path.Combine(carpeta, nombreArchivo);

            File.WriteAllBytes(ruta, ConstruirPdf(dibujar));

            return ruta;
        }

        public static void MostrarVistaPrevia(string titulo, Action<Graphics, float> dibujar)
        {
            // Se renderiza exactamente la misma página que va al PDF y se
            // escala a la hoja, para que la vista previa sea fiel al archivo.
            using (Bitmap pagina = RenderizarPagina(dibujar))
            using (PrintDocument documento = new PrintDocument())
            {
                documento.DocumentName = titulo;

                try
                {
                    documento.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                    documento.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                catch
                {
                    // Si el driver no acepta el tamaño, se usa el predeterminado.
                }

                documento.OriginAtMargins = true;

                documento.PrintPage += (sender, e) =>
                {
                    e.Graphics.PageUnit = GraphicsUnit.Display;
                    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    Rectangle bounds = e.PageBounds;
                    e.Graphics.DrawImage(pagina, new RectangleF(bounds.Left, bounds.Top, bounds.Width, bounds.Height));
                    e.HasMorePages = false;
                };

                using (PrintPreviewDialog vista = new PrintPreviewDialog())
                {
                    vista.Document = documento;
                    vista.UseAntiAlias = true;
                    vista.Text = titulo;
                    vista.Width = 1000;
                    vista.Height = 780;
                    vista.ShowDialog();
                }
            }
        }

        private static byte[] ConstruirPdf(Action<Graphics, float> dibujar)
        {
            using (Bitmap pagina = RenderizarPagina(dibujar))
            {
                byte[] jpegPagina = GuardarJpeg(pagina);
                return EnsamblarPdfConImagen(jpegPagina, pagina.Width, pagina.Height);
            }
        }

        private static Bitmap RenderizarPagina(Action<Graphics, float> dibujar)
        {
            Bitmap bmp = new Bitmap(AnchoPx, AltoPx, PixelFormat.Format24bppRgb);
            bmp.SetResolution(Dpi, Dpi);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                dibujar(g, g.DpiX / 72f);
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
                    parametros.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
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

        #region Piezas visuales compartidas

        public static void DibujarEncabezado(Graphics g, float s)
        {
            using (Font marca = Fuente(g, 26, true))
            using (Font sub = Fuente(g, 9, false))
            using (Pen penAzul = new Pen(Azul, 2f * s))
            using (Pen penLinea = new Pen(Linea, 1f * s))
            using (SolidBrush azul = new SolidBrush(Azul))
            using (SolidBrush gris = new SolidBrush(Gris))
            {
                // Franja de marca en el borde superior.
                g.FillRectangle(azul, 0, 0, AnchoPt * s, 7 * s);

                g.DrawString(Marca, marca, azul, 55 * s, 26 * s, SfTipografico);
                g.DrawString(Tr("Reporte.subtitulo"), sub, gris, 57 * s, 64 * s, SfTipografico);

                DibujarLogo(g, s);

                g.DrawLine(penAzul, 55 * s, 90 * s, 545 * s, 90 * s);
                g.DrawLine(penLinea, 55 * s, 94 * s, 545 * s, 94 * s);
            }
        }

        public static void DibujarPie(Graphics g, float s, string generado)
        {
            using (Font pie = Fuente(g, 7.5f, false))
            using (Font marca = Fuente(g, 7.5f, true))
            using (Pen pen = new Pen(Azul, 1.2f * s))
            using (SolidBrush gris = new SolidBrush(Gris))
            using (SolidBrush azul = new SolidBrush(Azul))
            {
                g.DrawLine(pen, 55 * s, 784 * s, 545 * s, 784 * s);

                g.DrawString(generado, pie, gris, 55 * s, 793 * s, SfTipografico);

                string pagina = string.Format(Tr("Reporte.pagina"), 1, 1);
                DibujarTextoCentrado(g, s, pagina, (AnchoPt / 2f), 793, 7.5f, false, Gris);

                float anchoMarca = Medir(g, Marca, 7.5f, true);
                g.DrawString(Marca, marca, azul, 545 * s - anchoMarca, 793 * s, SfTipografico);
            }
        }

        public static void DibujarSeccion(Graphics g, float s, string numero, string texto, float y)
        {
            using (Font num = Fuente(g, 8.5f, true))
            using (Font titulo = Fuente(g, 8.5f, true))
            using (SolidBrush grisClaro = new SolidBrush(GrisClaro))
            using (SolidBrush azul = new SolidBrush(Azul))
            using (SolidBrush oscuro = new SolidBrush(Oscuro))
            using (Pen hair = new Pen(Linea, 0.7f * s))
            {
                float x = 55 * s;

                g.DrawString(numero, num, grisClaro, x, y * s, SfTipografico);

                float anchoNumero = Medir(g, numero, 8.5f, true);
                float xBarra = x + anchoNumero + 8 * s;

                g.FillRectangle(azul, xBarra, (y + 2) * s, 3 * s, 11 * s);

                float xTexto = xBarra + 10 * s;
                string tituloMayus = (texto ?? string.Empty).ToUpperInvariant();

                g.DrawString(tituloMayus, titulo, oscuro, xTexto, y * s, SfTipografico);

                float anchoTitulo = Medir(g, tituloMayus, 8.5f, true);
                float xLinea = xTexto + anchoTitulo + 12 * s;

                if (xLinea < 540 * s)
                {
                    g.DrawLine(hair, xLinea, (y + 7) * s, 545 * s, (y + 7) * s);
                }
            }
        }

        public static void DibujarPanel(Graphics g, float s, float x, float y, float ancho, float alto,
            float radio, Color relleno, Color borde)
        {
            RectangleF r = new RectangleF(x * s, y * s, ancho * s, alto * s);

            using (GraphicsPath camino = RectanguloRedondeado(r, radio * s))
            {
                using (SolidBrush fondo = new SolidBrush(relleno))
                {
                    g.FillPath(fondo, camino);
                }

                if (borde != Color.Empty)
                {
                    using (Pen pen = new Pen(borde, 0.8f * s))
                    {
                        g.DrawPath(pen, camino);
                    }
                }
            }
        }

        public static void DibujarBarraAcento(Graphics g, float s, float x, float y, float alto, Color color)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x * s, y * s, 3.5f * s, alto * s);
            }
        }

        public static void DibujarEtiqueta(Graphics g, float s, string texto, float x, float y)
        {
            using (Font etiqueta = Fuente(g, 7.2f, true))
            using (SolidBrush gris = new SolidBrush(Gris))
            {
                g.DrawString((texto ?? string.Empty).ToUpperInvariant(), etiqueta, gris, x * s, y * s, SfTipografico);
            }
        }

        public static void DibujarValor(Graphics g, float s, string texto, float x, float y)
        {
            DibujarValor(g, s, texto, x, y, 11f);
        }

        public static void DibujarValor(Graphics g, float s, string texto, float x, float y, float tamano)
        {
            using (Font valor = Fuente(g, tamano, true))
            using (SolidBrush oscuro = new SolidBrush(Oscuro))
            {
                g.DrawString(texto ?? string.Empty, valor, oscuro, x * s, y * s, SfTipografico);
            }
        }

        public static void DibujarTexto(Graphics g, float s, string texto, float x, float y,
            float tamano, bool negrita, Color color)
        {
            using (Font fuente = Fuente(g, tamano, negrita))
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.DrawString(texto ?? string.Empty, fuente, brush, x * s, y * s, SfTipografico);
            }
        }

        public static void DibujarTextoAjustado(Graphics g, float s, string texto, float x, float y,
            float anchoMax, float tamano, bool negrita, Color color)
        {
            texto = texto ?? string.Empty;

            float tam = tamano;

            while (tam > 7.5f && Medir(g, texto, tam, negrita) > anchoMax * s)
            {
                tam -= 0.5f;
            }

            using (Font fuente = Fuente(g, tam, negrita))
            using (SolidBrush brush = new SolidBrush(color))
            {
                if (Medir(g, texto, tam, negrita) > anchoMax * s)
                {
                    RectangleF area = new RectangleF(x * s, y * s, anchoMax * s, (tam * 1.8f) * s);

                    using (StringFormat formato = new StringFormat())
                    {
                        formato.Trimming = StringTrimming.EllipsisCharacter;
                        formato.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip;

                        g.DrawString(texto, fuente, brush, area, formato);
                    }
                }
                else
                {
                    g.DrawString(texto, fuente, brush, x * s, y * s, SfTipografico);
                }
            }
        }

        public static void DibujarTextoCentrado(Graphics g, float s, string texto, float centro, float y,
            float tamano, bool negrita, Color color)
        {
            texto = texto ?? string.Empty;

            using (Font fuente = Fuente(g, tamano, negrita))
            using (SolidBrush brush = new SolidBrush(color))
            {
                float ancho = g.MeasureString(texto, fuente, PointF.Empty, SfTipografico).Width;
                g.DrawString(texto, fuente, brush, centro * s - ancho / 2f, y * s, SfTipografico);
            }
        }

        public static float DibujarPildora(Graphics g, float s, string texto, float x, float y,
            Color relleno, Color colorTexto, Color borde)
        {
            float ancho = AnchoPildora(g, texto);
            float alto = 21f;

            using (GraphicsPath camino = RectanguloRedondeado(
                new RectangleF(x * s, y * s, ancho * s, alto * s), (alto / 2f) * s))
            using (Font fuente = Fuente(g, 8.2f, true))
            using (SolidBrush fondo = new SolidBrush(relleno))
            using (SolidBrush brushTexto = new SolidBrush(colorTexto))
            {
                g.FillPath(fondo, camino);

                if (borde != Color.Empty)
                {
                    using (Pen pen = new Pen(borde, 1f * s))
                    {
                        g.DrawPath(pen, camino);
                    }
                }

                float anchoTexto = g.MeasureString(texto ?? string.Empty, fuente, PointF.Empty, SfTipografico).Width;
                g.DrawString(texto ?? string.Empty, fuente, brushTexto,
                    x * s + (ancho * s - anchoTexto) / 2f, (y + 5) * s, SfTipografico);
            }

            return ancho;
        }

        public static float AnchoPildora(Graphics g, string texto)
        {
            return Medir(g, texto, 8.2f, true) / (g.DpiX / 72f) + 22f;
        }

        public static float DibujarChip(Graphics g, float s, string texto, float x, float y,
            Color relleno, Color colorTexto)
        {
            float ancho = Medir(g, texto, 8f, true) / (g.DpiX / 72f) + 18f;
            float alto = 20f;

            using (GraphicsPath camino = RectanguloRedondeado(
                new RectangleF(x * s, y * s, ancho * s, alto * s), (alto / 2f) * s))
            using (Font fuente = Fuente(g, 8, true))
            using (SolidBrush fondo = new SolidBrush(relleno))
            using (SolidBrush brushTexto = new SolidBrush(colorTexto))
            {
                g.FillPath(fondo, camino);
                g.DrawString(texto ?? string.Empty, fuente, brushTexto, (x + 9) * s, (y + 5) * s, SfTipografico);
            }

            return ancho;
        }

        public static float AnchoChip(Graphics g, string texto)
        {
            return Medir(g, texto, 8f, true) / (g.DpiX / 72f) + 18f;
        }

        public static void DibujarAvatar(Graphics g, float s, string iniciales, float x, float y, float diametro)
        {
            using (SolidBrush fondo = new SolidBrush(AzulSuave))
            using (Pen borde = new Pen(Color.FromArgb(70, Azul), 1f * s))
            using (Font fuente = Fuente(g, 12, true))
            using (SolidBrush texto = new SolidBrush(Azul))
            {
                RectangleF circulo = new RectangleF(x * s, y * s, diametro * s, diametro * s);

                g.FillEllipse(fondo, circulo);
                g.DrawEllipse(borde, circulo);

                string ini = string.IsNullOrWhiteSpace(iniciales) ? "?" : iniciales.ToUpperInvariant();
                float ancho = g.MeasureString(ini, fuente, PointF.Empty, SfTipografico).Width;
                float alto = g.MeasureString(ini, fuente, PointF.Empty, SfTipografico).Height;

                g.DrawString(ini, fuente, texto,
                    circulo.X + (circulo.Width - ancho) / 2f,
                    circulo.Y + (circulo.Height - alto) / 2f, SfTipografico);
            }
        }

        public static void DibujarCheck(Graphics g, float s, float x, float y, float diametro)
        {
            using (SolidBrush fondo = new SolidBrush(AzulSuave))
            using (Pen check = new Pen(Azul, 1.4f * s))
            {
                check.StartCap = LineCap.Round;
                check.EndCap = LineCap.Round;

                g.FillEllipse(fondo, x * s, y * s, diametro * s, diametro * s);

                float cx = (x + diametro / 2f) * s;
                float cy = (y + diametro / 2f) * s;
                float u = diametro * s / 10f;

                g.DrawLines(check, new[]
                {
                    new PointF(cx - 2.4f * u, cy + 0.1f * u),
                    new PointF(cx - 0.6f * u, cy + 1.8f * u),
                    new PointF(cx + 2.6f * u, cy - 1.6f * u)
                });
            }
        }

        public static void DibujarNota(Graphics g, float s, string texto, float x, float y, float ancho, float alto)
        {
            DibujarPanel(g, s, x, y, ancho, alto, 8, AzulSuave, Color.Empty);
            DibujarBarraAcento(g, s, x + 1, y + 1, alto - 2, Azul);

            using (Font nota = Fuente(g, 8.6f, false))
            using (SolidBrush oscuro = new SolidBrush(Oscuro))
            {
                RectangleF area = new RectangleF((x + 17) * s, (y + 9) * s, (ancho - 30) * s, (alto - 16) * s);

                SizeF medida = g.MeasureString(texto ?? string.Empty, nota, new SizeF(area.Width, 10000f));
                float yTexto = area.Y + Math.Max(0f, (area.Height - medida.Height) / 2f);

                g.DrawString(texto ?? string.Empty, nota, oscuro,
                    new RectangleF(area.X, yTexto, area.Width, medida.Height + 2 * s));
            }
        }

        public static void DibujarIconoPin(Graphics g, float s, float centroX, float y, Color color)
        {
            using (Pen pen = new Pen(color, 1.6f * s))
            using (SolidBrush brush = new SolidBrush(color))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                float cx = centroX * s;
                float cy = (y + 6) * s;
                float r = 5.2f * s;

                g.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
                g.FillEllipse(brush, cx - 1.6f * s, cy - 1.6f * s, 3.2f * s, 3.2f * s);

                g.DrawLine(pen, cx - 3.4f * s, cy + 3.6f * s, cx, (y + 16) * s);
                g.DrawLine(pen, cx + 3.4f * s, cy + 3.6f * s, cx, (y + 16) * s);
            }
        }

        public static float DibujarCodigoBarras(Graphics g, float s, string datos, float x, float y,
            float alto, float modulo, Color color)
        {
            string limpio = LimpiarParaCodigo39(datos);
            string secuencia = "*" + limpio + "*";

            float cursor = x;

            using (SolidBrush brush = new SolidBrush(color))
            {
                foreach (char caracter in secuencia)
                {
                    string patron;

                    if (!Patrones39.TryGetValue(caracter, out patron))
                    {
                        continue;
                    }

                    for (int i = 0; i < patron.Length; i++)
                    {
                        float ancho = (patron[i] == '1') ? modulo * 2.6f : modulo;

                        if (i % 2 == 0)
                        {
                            g.FillRectangle(brush, cursor * s, y * s, ancho * s, alto * s);
                        }

                        cursor += ancho;
                    }

                    // Separación entre caracteres.
                    cursor += modulo;
                }
            }

            return cursor - x;
        }

        public static void DibujarBloqueCodigoBarras(Graphics g, float s, string datos, float centroX,
            float y, float alto, float modulo, string etiqueta)
        {
            {
                if (!string.IsNullOrWhiteSpace(etiqueta))
                {
                    DibujarTextoCentrado(g, s, etiqueta.ToUpperInvariant(), centroX, y - 13, 6.8f, true, Gris);
                }

                float ancho = CalcularAncho39(datos, modulo);
                float inicio = centroX - ancho / 2f;

                DibujarCodigoBarras(g, s, datos, inicio, y, alto, modulo, Oscuro);

                DibujarTextoCentrado(g, s, datos, centroX, y + alto + 5, 7.5f, true, Oscuro);
            }
        }

        public static float CalcularAncho39(string datos, float modulo)
        {
            string secuencia = "*" + LimpiarParaCodigo39(datos) + "*";
            float ancho = 0;

            foreach (char caracter in secuencia)
            {
                string patron;

                if (!Patrones39.TryGetValue(caracter, out patron))
                {
                    continue;
                }

                for (int i = 0; i < patron.Length; i++)
                {
                    ancho += (patron[i] == '1') ? modulo * 2.6f : modulo;
                }

                ancho += modulo;
            }

            return ancho;
        }

        public static float AnchoTexto(Graphics g, string texto, float tamano, bool negrita)
        {
            return Medir(g, texto, tamano, negrita);
        }

        public static float Medir(Graphics g, string texto, float tamano, bool negrita)
        {
            using (Font fuente = Fuente(g, tamano, negrita))
            {
                return g.MeasureString(texto ?? string.Empty, fuente, PointF.Empty, SfTipografico).Width;
            }
        }

        public static Font Fuente(Graphics g, float tamano, bool negrita)
        {
            return new Font(Familia, tamano * (g.DpiX / 72f),
                negrita ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
        }

        private static void DibujarLogo(Graphics g, float s)
        {
            string ruta = ObtenerRutaLogo();

            if (!File.Exists(ruta))
            {
                return;
            }

            try
            {
                using (Image logo = Image.FromFile(ruta))
                {
                    float escala = Math.Min(150f / logo.Width, 48f / logo.Height);
                    float ancho = logo.Width * escala;
                    float alto = logo.Height * escala;
                    float y = 24f + (48f - alto) / 2f;

                    g.DrawImage(logo, 545 * s - ancho * s, y * s, ancho * s, alto * s);
                }
            }
            catch
            {
                // El reporte no debe fallar por no poder leer el logo.
            }
        }

        private static string ObtenerRutaLogo()
        {
            string enSalida = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recursos", ArchivoLogo);

            if (File.Exists(enSalida))
            {
                return enSalida;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Recursos", ArchivoLogo);
        }

        private static GraphicsPath RectanguloRedondeado(RectangleF r, float radio)
        {
            float limite = Math.Min(r.Width, r.Height) / 2f;
            float d = Math.Max(0f, Math.Min(radio, limite)) * 2f;

            GraphicsPath camino = new GraphicsPath();

            if (d <= 0.01f)
            {
                camino.AddRectangle(r);
                camino.CloseFigure();
                return camino;
            }

            camino.AddArc(r.X, r.Y, d, d, 180, 90);
            camino.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            camino.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            camino.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            camino.CloseFigure();

            return camino;
        }

        private static StringFormat CrearFormatoTipografico()
        {
            StringFormat formato = StringFormat.GenericTypographic.Clone() as StringFormat;

            if (formato == null)
            {
                formato = new StringFormat();
            }

            formato.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            return formato;
        }

        private static string ResolverFamilia(string preferida, string alternativa)
        {
            try
            {
                using (FontFamily familia = new FontFamily(preferida))
                {
                    if (string.Equals(familia.Name, preferida, StringComparison.OrdinalIgnoreCase))
                    {
                        return preferida;
                    }
                }
            }
            catch
            {
                // Se usa la alternativa.
            }

            return alternativa;
        }

        private static string LimpiarParaCodigo39(string datos)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in (datos ?? string.Empty).ToUpperInvariant())
            {
                if (Patrones39.ContainsKey(c) && c != '*')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        private static readonly Dictionary<char, string> Patrones39 = new Dictionary<char, string>
        {
            { '0', "000110100" }, { '1', "100100001" }, { '2', "001100001" }, { '3', "101100000" },
            { '4', "000110001" }, { '5', "100110000" }, { '6', "001110000" }, { '7', "000100101" },
            { '8', "100100100" }, { '9', "001100100" },
            { 'A', "100001001" }, { 'B', "001001001" }, { 'C', "101001000" }, { 'D', "000011001" },
            { 'E', "100011000" }, { 'F', "001011000" }, { 'G', "000001101" }, { 'H', "100001100" },
            { 'I', "001001100" }, { 'J', "000011100" }, { 'K', "100000011" }, { 'L', "001000011" },
            { 'M', "101000010" }, { 'N', "000010011" }, { 'O', "100010010" }, { 'P', "001010010" },
            { 'Q', "000000111" }, { 'R', "100000110" }, { 'S', "001000110" }, { 'T', "000010110" },
            { 'U', "110000001" }, { 'V', "011000001" }, { 'W', "111000000" }, { 'X', "010010001" },
            { 'Y', "110010000" }, { 'Z', "011010000" },
            { '-', "010000101" }, { '.', "110000100" }, { ' ', "011000100" },
            { '$', "010101000" }, { '/', "010100010" }, { '+', "010001010" }, { '%', "000101010" },
            { '*', "010010100" }
        };

        #endregion

        #region Formatos

        public static string FormatearFecha(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy", Cultura);
        }

        public static string FormatearMoneda(decimal valor)
        {
            return valor.ToString("C2", Cultura);
        }

        public static string FormatearNumero(decimal valor)
        {
            return valor.ToString("N2", Cultura);
        }

        #endregion

        #region Carpeta de salida

        public static string ObtenerCarpetaDescargas()
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

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out string ppszPath);

        #endregion
    }
}
