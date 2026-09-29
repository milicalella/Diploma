using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLIdioma577MC
    {
        DALIdioma577MC dal = new DALIdioma577MC();

        public List<Idioma577MC> obtenerTodos()
        {
            DataTable dt = dal.obtenerTodos();
            List<Idioma577MC> lista = new List<Idioma577MC>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Idioma577MC
                {
                    Id = System.Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                });
            }

            return lista;
        }
    }
}
