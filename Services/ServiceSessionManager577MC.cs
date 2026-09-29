﻿using BE;
using Services;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_577MC
{
    public sealed class ServiceSessionManager577MC
    {
        private ServiceSessionManager577MC() 
        {
            Idioma = new IdiomaManager();
        }

        private static ServiceSessionManager577MC _instancia;

        public UsuarioModelo577MC usuarioActivo { get; private set; }

        public static ServiceSessionManager577MC getIntancia()
        {
            if( _instancia == null)
            {
                _instancia = new ServiceSessionManager577MC();
            }

            return _instancia;
        }

        public void Login(UsuarioModelo577MC usuario)
        {
            usuarioActivo = usuario;
        }

        public void Logout()
        {
            usuarioActivo = null;
        }

        public bool estaLogueado()
        {
            return usuarioActivo != null;
        }


        public bool TienePermiso(string nombrePermiso)
        {
            List<Componente577MC> todosLosPermisos = usuarioActivo.Rol.ObtenerPermisos();

            //recorremos la lista buscando coincidencia por el nombre de la patente
            foreach (Componente577MC componente in todosLosPermisos)
            {
                if (string.Equals(componente.Nombre, nombrePermiso, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        public IdiomaManager Idioma { get; private set; }

    }
}
