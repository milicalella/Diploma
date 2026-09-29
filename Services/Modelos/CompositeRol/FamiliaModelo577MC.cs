﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public class FamiliaModelo577MC : Componente577MC
    {
        private List<Componente577MC> hijos = new List<Componente577MC>();

        public override void agregarHijos(Componente577MC c)
        {
            hijos.Add(c);
        }

        public override void eliminarHijo(Componente577MC c)
        {
            hijos.Remove(c);
        }

        public override List<Componente577MC> obtenerPermisos()
        {
            List<Componente577MC> permisos = new List<Componente577MC>();

            foreach (Componente577MC hijo in hijos)
            {
                permisos.AddRange(hijo.obtenerPermisos());
            }

            return permisos;
        }

        public override string ToString()
        {
            return $"Familia {this.Nombre}";
        }
    }
}
