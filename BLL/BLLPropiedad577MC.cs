using BE;
using BE.Enum;
using DAL;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPropiedad577MC
    {
        private static readonly string[] TiposValidos = { "Casa", "Departamento", "Local", "Terreno" };
        private static readonly string[] EstadosValidos = { "Disponible", "Vendida", "Alquilada", "Reservada" };

        DALPropiedad577MC dal = new DALPropiedad577MC();
        BLLBitacora577MC bit = new BLLBitacora577MC();

        public List<Propiedad577MC> ObtenerTodas()
        {
            DataTable dt = dal.ObtenerTodas();
            return Mapear(dt);
        }

        public List<Propiedad577MC> Buscar(string direccion, string tipo, string estado, decimal? precioMin, decimal? precioMax)
        {
            ValidarCriterios(direccion, tipo, estado, precioMin, precioMax);

            DataTable dt = dal.Buscar(direccion, tipo, estado, precioMin, precioMax);
            List<Propiedad577MC> propiedades = Mapear(dt);

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Consultó el catálogo de propiedades y obtuvo {propiedades.Count} resultado(s).", Criticidad577MC.Medio, Modulos577MC.Propiedades);

            return propiedades;
        }

        private void ValidarCriterios(string direccion, string tipo, string estado, decimal? precioMin, decimal? precioMax)
        {
            if (precioMin.HasValue && precioMin.Value < 0)
            {
                throw new Exception("El precio mínimo no puede ser negativo.");
            }

            if (precioMax.HasValue && precioMax.Value < 0)
            {
                throw new Exception("El precio máximo no puede ser negativo.");
            }

            if (precioMin.HasValue && precioMax.HasValue && precioMin.Value > precioMax.Value)
            {
                throw new Exception("El precio mínimo no puede ser mayor que el precio máximo.");
            }

            if (!string.IsNullOrWhiteSpace(direccion) && direccion.Trim().Length < 3)
            {
                throw new Exception("La dirección debe tener al menos 3 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(tipo) && !SistemaValido(TiposValidos, tipo))
            {
                throw new Exception($"El tipo '{tipo}' no es válido. Tipos permitidos: {string.Join(", ", TiposValidos)}.");
            }

            if (!string.IsNullOrWhiteSpace(estado) && !SistemaValido(EstadosValidos, estado))
            {
                throw new Exception($"El estado '{estado}' no es válido. Estados permitidos: {string.Join(", ", EstadosValidos)}.");
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