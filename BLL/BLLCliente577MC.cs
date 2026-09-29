using BE;
using BE.Enum;
using DAL;
using Services;
using System;
using System.Collections.Generic;
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