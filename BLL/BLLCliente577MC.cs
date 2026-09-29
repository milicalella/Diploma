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
    public class BLLCliente577MC
    {
        DALCliente577MC dal = new DALCliente577MC();
        BLLBitacora577MC bit = new BLLBitacora577MC();

        private static string Tr(string key, params object[] args)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;
            return string.Format(idioma != null ? idioma.Translate(key) : key, args);
        }

        public bool ExisteCliente(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return false;
            }

            return dal.ExisteCliente(dni.Trim());
        }

        public List<Cliente577MC> ObtenerTodos()
        {
            List<Cliente577MC> lista = new List<Cliente577MC>();

            foreach (DataRow r in dal.ObtenerTodos().Rows)
            {
                lista.Add(new Cliente577MC
                {
                    DNI = r["DNI"].ToString(),
                    Nombre = r["Nombre"].ToString(),
                    Apellido = r["Apellido"].ToString(),
                    Telefono = r["Telefono"].ToString(),
                    Email = r["Email"].ToString()
                });
            }

            return lista;
        }

        public void ModificarCliente(Cliente577MC cliente)
        {
            if (!dal.ExisteCliente(cliente.DNI))
            {
                throw new Exception(Tr("ClienteException.msgNoExiste"));
            }

            long dvh = DigitoVerificador577MC.CalcularDVH(cliente.DNI + cliente.Nombre + cliente.Apellido + cliente.Telefono + cliente.Email);

            int afectadas = dal.ModificarCliente(cliente.DNI, cliente.Nombre, cliente.Apellido, cliente.Telefono, cliente.Email, dvh);

            if (afectadas == 0)
            {
                throw new Exception(Tr("ClienteException.msgNoActualizado"));
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Se modificó el cliente {cliente.Nombre} {cliente.Apellido} (DNI {cliente.DNI}).", Criticidad577MC.Medio, Modulos577MC.Cliente);
        }

        public void EliminarCliente(string dni)
        {
            if (!dal.ExisteCliente(dni))
            {
                throw new Exception(Tr("ClienteException.msgNoExiste"));
            }

            if (dal.TieneVisitas(dni))
            {
                throw new Exception(Tr("ClienteException.msgTieneVisitas"));
            }

            int afectadas = dal.EliminarCliente(dni);

            if (afectadas == 0)
            {
                throw new Exception(Tr("ClienteException.msgNoActualizado"));
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Se eliminó el cliente (DNI {dni}).", Criticidad577MC.Medio, Modulos577MC.Cliente);
        }

        public void RegistrarCliente(Cliente577MC cliente)
        {
            if (dal.ExisteCliente(cliente.DNI))
            {
                throw new Exception(Tr("ClienteException.msgYaExiste"));
            }

            long dvh = DigitoVerificador577MC.CalcularDVH(cliente.DNI + cliente.Nombre + cliente.Apellido + cliente.Telefono + cliente.Email);

            dal.InsertarCliente(cliente.DNI, cliente.Nombre, cliente.Apellido, cliente.Telefono, cliente.Email, dvh);

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo?.DNI ?? "SISTEMA";
            bit.registrarEvento(dniAutor, $"Se registró el cliente {cliente.Nombre} {cliente.Apellido} (DNI {cliente.DNI}).", Criticidad577MC.Medio, Modulos577MC.Cliente);
        }
    }
}