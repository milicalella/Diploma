﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace Services.Modelos
{
    public class UsuarioModelo577MC
    {

        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public RolModelo577MC Rol { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Intentos { get; set; }
        public bool Bloqueo { get; set; }
        public bool Activo { get; set; }
        public DateTime? UltimoIntentoFallido { get; set; }
        public int IdIdioma { get; set; }
        public long DVH { get; set; }




        public UsuarioModelo577MC()
        {
            
        }

        //ctor para new
        public UsuarioModelo577MC(string dNI, string nombre, string apellido, string email, RolModelo577MC rol)
        {
            DNI = dNI;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Rol = rol;
        }
    }
}
