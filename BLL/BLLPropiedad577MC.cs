using BE;
using BE.Enum;
using DAL;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPropiedad577MC
    {
        private static readonly string[] TiposValidos = { "Casa", "Departamento", "Local", "Terreno" };
        private static readonly string[] EstadosValidos = { "Disponible", "Vendida", "Alquilada", "Reservada" };

        private static string Tr(string key, params object[] args)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;
            return string.Format(idioma != null ? idioma.Translate(key) : key, args);
        }

        DALPropiedad577MC dal = new DALPropiedad577MC();
        BLLBitacora577MC bit = new BLLBitacora577MC();

        public List<Propiedad577MC> ObtenerTodas()
        {
            DataTable dt = dal.ObtenerTodas();
            return Mapear(dt);
        }

        public bool TieneVisitas(int idPropiedad)
        {
            return dal.TieneVisitas(idPropiedad);
        }

        public int RegistrarPropiedad(Propiedad577MC propiedad)
        {
            ValidarDatosPropiedad(propiedad);

            int id = dal.InsertarPropiedad(propiedad.Direccion.Trim(), propiedad.Tipo.Trim(), propiedad.Estado.Trim(),
                propiedad.Precio, propiedad.SuperficieM2, propiedad.Ambientes, propiedad.Dormitorios, propiedad.Banios);

            long dvh = CalcularDVHPropiedad(id, propiedad);
            dal.ActualizarDVH(id, dvh);

            RegistrarBitacora($"Se registró la propiedad {id} - {propiedad.Direccion}.");

            return id;
        }

        public void ModificarPropiedad(Propiedad577MC propiedad)
        {
            ValidarDatosPropiedad(propiedad);

            long dvh = CalcularDVHPropiedad(propiedad.Id, propiedad);

            int afectadas = dal.ModificarPropiedad(propiedad.Id, propiedad.Direccion.Trim(), propiedad.Tipo.Trim(), propiedad.Estado.Trim(),
                propiedad.Precio, propiedad.SuperficieM2, propiedad.Ambientes, propiedad.Dormitorios, propiedad.Banios, dvh);

            if (afectadas == 0)
            {
                throw new Exception(Tr("PropiedadException.msgNoExiste"));
            }

            RegistrarBitacora($"Se modificó la propiedad {propiedad.Id} - {propiedad.Direccion}.");
        }

        public void EliminarPropiedad(int idPropiedad)
        {
            if (dal.TieneVisitas(idPropiedad))
            {
                throw new Exception(Tr("PropiedadException.msgConVisitas"));
            }

            int afectadas = dal.EliminarPropiedad(idPropiedad);

            if (afectadas == 0)
            {
                throw new Exception(Tr("PropiedadException.msgNoExiste"));
            }

            RegistrarBitacora($"Se eliminó la propiedad {idPropiedad}.");
        }

        private void RegistrarBitacora(string evento)
        {
            var usuario = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo;
            if (usuario == null || string.IsNullOrEmpty(usuario.DNI))
            {
                return;
            }

            bit.registrarEvento(usuario.DNI, evento, Criticidad577MC.Medio, Modulos577MC.Propiedades);
        }

        public List<Propiedad577MC> Buscar(string direccion, string tipo, string estado, decimal? precioMin, decimal? precioMax)
        {
            ValidarCriterios(direccion, tipo, estado, precioMin, precioMax);

            DataTable dt = dal.Buscar(direccion, tipo, estado, precioMin, precioMax);
            List<Propiedad577MC> propiedades = Mapear(dt);

            return propiedades;
        }

        private void ValidarDatosPropiedad(Propiedad577MC propiedad)
        {
            if (propiedad == null)
            {
                throw new Exception(Tr("PropiedadException.msgDatosIncompletos"));
            }

            if (string.IsNullOrWhiteSpace(propiedad.Direccion) || propiedad.Direccion.Trim().Length < 3)
            {
                throw new Exception(Tr("PropiedadException.msgDireccionInvalida"));
            }

            if (string.IsNullOrWhiteSpace(propiedad.Tipo) || !SistemaValido(TiposValidos, propiedad.Tipo))
            {
                throw new Exception(Tr("PropiedadException.msgTipoInvalido", propiedad.Tipo, string.Join(", ", TiposValidos)));
            }

            if (string.IsNullOrWhiteSpace(propiedad.Estado) || !SistemaValido(EstadosValidos, propiedad.Estado))
            {
                throw new Exception(Tr("PropiedadException.msgEstadoInvalido", propiedad.Estado, string.Join(", ", EstadosValidos)));
            }

            if (propiedad.Precio < 0)
            {
                throw new Exception(Tr("PropiedadException.msgPrecioInvalido"));
            }

            if (propiedad.SuperficieM2 <= 0)
            {
                throw new Exception(Tr("PropiedadException.msgSuperficieInvalida"));
            }

            if (propiedad.Ambientes < 0 || propiedad.Dormitorios < 0 || propiedad.Banios < 0)
            {
                throw new Exception(Tr("PropiedadException.msgNumerosInvalido"));
            }
        }

        private long CalcularDVHPropiedad(int id, Propiedad577MC propiedad)
        {
            string cadena = id.ToString() +
                            propiedad.Direccion.Trim() +
                            propiedad.Tipo.Trim() +
                            propiedad.Estado.Trim() +
                            propiedad.Precio.ToString("0.00", CultureInfo.InvariantCulture) +
                            propiedad.SuperficieM2.ToString("0.00", CultureInfo.InvariantCulture) +
                            propiedad.Ambientes +
                            propiedad.Dormitorios +
                            propiedad.Banios;

            return DigitoVerificador577MC.CalcularDVH(cadena);
        }

        private void ValidarCriterios(string direccion, string tipo, string estado, decimal? precioMin, decimal? precioMax)
        {
            if (precioMin.HasValue && precioMin.Value < 0)
            {
                throw new Exception(Tr("PropiedadException.msgPrecioMinimoNegativo"));
            }

            if (precioMax.HasValue && precioMax.Value < 0)
            {
                throw new Exception(Tr("PropiedadException.msgPrecioMaximoNegativo"));
            }

            if (precioMin.HasValue && precioMax.HasValue && precioMin.Value > precioMax.Value)
            {
                throw new Exception(Tr("PropiedadException.msgPrecioRango"));
            }

            if (!string.IsNullOrWhiteSpace(tipo) && !SistemaValido(TiposValidos, tipo))
            {
                throw new Exception(Tr("PropiedadException.msgTipoInvalido", tipo, string.Join(", ", TiposValidos)));
            }

            if (!string.IsNullOrWhiteSpace(estado) && !SistemaValido(EstadosValidos, estado))
            {
                throw new Exception(Tr("PropiedadException.msgEstadoInvalido", estado, string.Join(", ", EstadosValidos)));
            }
        }

        private bool SistemaValido(string[] valores, string valor)
        {
            return valores.Any(v => string.Equals(v, valor, StringComparison.OrdinalIgnoreCase));
        }

        private List<Propiedad577MC> Mapear(DataTable dt)
        {
            List<Propiedad577MC> lista = new List<Propiedad577MC>();

            foreach (DataRow r in dt.Rows)
            {
                lista.Add(new Propiedad577MC
                {
                    Id = Convert.ToInt32(r["Id"]),
                    Direccion = r["Direccion"].ToString(),
                    Tipo = r["Tipo"].ToString(),
                    Estado = r["Estado"].ToString(),
                    Precio = Convert.ToDecimal(r["Precio"]),
                    SuperficieM2 = Convert.ToDecimal(r["SuperficieM2"]),
                    Ambientes = Convert.ToInt32(r["Ambientes"]),
                    Dormitorios = Convert.ToInt32(r["Dormitorios"]),
                    Banios = Convert.ToInt32(r["Banios"]),
                    DVH = r["DVH"] == DBNull.Value ? 0 : Convert.ToInt64(r["DVH"])
                });
            }

            return lista;
        }
    }
}