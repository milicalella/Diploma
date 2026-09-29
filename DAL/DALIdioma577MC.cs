using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALIdioma577MC
    {
        DALAcceso577MC acceso = new DALAcceso577MC();

        public DataTable obtenerTodos()
        {
            string query = "SELECT Id, Nombre FROM Idioma";
            return acceso.executeDataTable(query);
        }
    }
}
