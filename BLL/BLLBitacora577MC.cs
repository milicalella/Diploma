using DAL;
using BE.Enum;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBitacora577MC
    {
        DALBitacora577MC dal = new DALBitacora577MC();
        public void registrarEvento(string dni, string evento, Criticidad577MC criticidad, Modulos577MC modulo)
        { 
            dal.insertarLog(dni, evento, (int)criticidad, (int)modulo, DateTime.Now);
        }


        public DataTable obtenerUltimos3Dias()
        {
            DateTime desde = DateTime.Now.AddDays(-3);
            DateTime hasta = DateTime.Now;

            return dal.obtenerBitacora(desde, hasta);
        }

        public DataTable obtenerBitacora(DateTime desde, DateTime hasta, int? moduloFiltro = null)
        {
            return dal.obtenerBitacora(desde, hasta, moduloFiltro);
        }
    }
}
