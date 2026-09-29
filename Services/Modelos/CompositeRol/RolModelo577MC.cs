﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public class RolModelo577MC
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Componente577MC> Permisos { get; set; } = new List<Componente577MC>();

        public override string ToString()
        {
            return this.Nombre;
        }
        public List<Componente577MC> ObtenerPermisos()
        {
            List<Componente577MC> permisos = new List<Componente577MC >();

            foreach (Componente577MC hijo in this.Permisos)
            {
                permisos.AddRange(hijo.obtenerPermisos());
            }

            return permisos.GroupBy(p => p.Id).Select(grupo => grupo.First()).ToList(); // esto permite que no se le asignen permisos duplicados
        }
    }
}
