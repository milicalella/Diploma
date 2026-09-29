﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modelos
{
    public abstract class Componente577MC
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        
        public virtual void agregarHijos(Componente577MC c)
        {
            throw new NotImplementedException();
        }
        public virtual void eliminarHijo(Componente577MC c)
        {
            throw new NotImplementedException();
        }
        public virtual List<Componente577MC> obtenerPermisos()
        {
            throw new NotImplementedException();
        }
    }
}
