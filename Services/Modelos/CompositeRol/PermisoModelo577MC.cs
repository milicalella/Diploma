﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public class PermisoModelo577MC : Componente577MC
    {
        public override List<Componente577MC> obtenerPermisos()
        {
            return new List<Componente577MC> { this }; // se devuelve a si mismo
        }

        public override string ToString()
        {
            return $"Patente {this.Nombre}";
        }
    }
}
