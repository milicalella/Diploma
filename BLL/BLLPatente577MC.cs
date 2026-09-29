using DAL;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPatente577MC
    {
        DALPatente577MC dal = new DALPatente577MC();
        public List<PermisoModelo577MC> obtenerTodos()
        {
            DataTable dt = dal.obtenerTodos();

            List<PermisoModelo577MC> lista = new List<PermisoModelo577MC>();

            foreach (DataRow row in dt.Rows)
            {
                var patente = new PermisoModelo577MC
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                };

                lista.Add(patente);
            }

            return lista;
        }

        

        
    }
}
