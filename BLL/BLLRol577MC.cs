using BE;
using BE.Enum;
using DAL;
using Services.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLRol577MC
    {
        DALRol577MC _dal = new DALRol577MC();
        BLLBitacora577MC bllBitacora = new BLLBitacora577MC();
        public List<RolModelo577MC> obtenerTodos()
        {
            List<RolModelo577MC> lista = new List<RolModelo577MC>();

            DataTable dt = _dal.obtenerTodos();

            foreach(DataRow r in dt.Rows)
            {
                lista.Add(new RolModelo577MC
                {
                    Id = Convert.ToInt32(r["Id"]),
                    Nombre = r["Nombre"].ToString()
                });
            }

            return lista;
        }

        public List<RolModelo577MC> ObtenerRolesConJerarquia()
        {
            DataTable[] dt = _dal.ObtenerDatosAcceso();

            List<RolModelo577MC> roles = obtenerTodos(dt[0]);
            var dictRoles = roles.ToDictionary(r => r.Id);

            var dictFamilias = MapearFamiliasBase(dt[3]);
            var dictPatentes = MapearPatentesBase(dt[4]);

            EnsamblarPatentesEnFamilias(dictFamilias, dictPatentes, dt[5]);
            EnsamblarFamiliasEnFamilias(dictFamilias, dt[6]);

            EnsamblarPatentesEnRoles(dictRoles, dictPatentes, dt[1]);
            EnsamblarFamiliasEnRoles(dictRoles, dictFamilias, dt[2]);

            return roles;
        }

        private List<RolModelo577MC> obtenerTodos(DataTable dt)
        {
            List<RolModelo577MC> lista = new List<RolModelo577MC>();

            foreach (DataRow r in dt.Rows)
            {
                lista.Add(new RolModelo577MC
                {
                    Id = Convert.ToInt32(r["Id"]),
                    Nombre = r["Nombre"].ToString()
                });
            }

            return lista;
        }


        public void crearRol(string nombre, List<Componente577MC> componentes)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            DataTable dt = _dal.obtenerPorNombre(nombre);

            if(dt.Rows.Count > 0)
            {
                throw new Exception(string.Format(idioma.Translate("ExcNombreYaExiste"), idioma.Translate("TablaRol"), nombre));
            }

            RolModelo577MC rol = new RolModelo577MC { Nombre =  nombre };

            foreach (Componente577MC comp in componentes)
            {
                var permisosActuales = rol.ObtenerPermisos();

                if (comp is PermisoModelo577MC patente)
                {
                    if (permisosActuales.Any(p => p.Id == patente.Id))
                    {
                        throw new Exception(string.Format(idioma.Translate("ExcConflictoPatenteIndirecta"), patente.Nombre));
                    }
                }

                else if (comp is FamiliaModelo577MC familia)
                {
                    var permisosHija = familia.obtenerPermisos();

                    foreach (var p in permisosHija)
                    {
                        if (permisosActuales.Any(pa => pa.Id == p.Id))
                        {
                            throw new Exception(string.Format(idioma.Translate("ExcConflictoFamiliaPermisos"), familia.Nombre));
                        }
                    }
                }

                rol.Permisos.Add(comp);
            }

            int nuevoRolId = _dal.insertarRol(nombre);

            long dvhInicial = Services.DigitoVerificador577MC.CalcularDVH(nuevoRolId.ToString() + nombre);
            _dal.ActualizarDVH(nuevoRolId, dvhInicial);
            Services.DigitoVerificador577MC.ActualizarDVVRol();

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;


            foreach (Componente577MC comp in componentes)
            {
                if (comp is PermisoModelo577MC patente)
                {
                    _dal.asignarPatenteARol(patente.Id, nuevoRolId);
                    bllBitacora.registrarEvento(dniAutor, $"Asignó la patente {patente.Nombre} a el rol {nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);
                }
                else if (comp is FamiliaModelo577MC familiaHija)
                {
                    _dal.asignarFamiliaARol(familiaHija.Id, nuevoRolId);
                    bllBitacora.registrarEvento(dniAutor, $"Asignó la familia {familiaHija.Nombre} a el rol {nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);
                }
            }

            bllBitacora.registrarEvento(dniAutor, $"Creo un nuevo rol", Criticidad577MC.Alto, Modulos577MC.Perfil);

        }

        public void AsignarPatente(RolModelo577MC rol, PermisoModelo577MC patente)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            var permisosAplanados = rol.ObtenerPermisos();

            if (permisosAplanados.Any(p => p.Id == patente.Id))
            {
                throw new Exception(string.Format(idioma.Translate("ExcRolYaContienePermiso"), rol.Nombre, patente.Nombre));
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;
            bllBitacora.registrarEvento(dniAutor, $"Asigno la patente {patente.Nombre} a el rol {rol.Nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);

            _dal.asignarPatenteARol(patente.Id, rol.Id);
        }

        public void AsignarFamilia(RolModelo577MC rol, FamiliaModelo577MC familia)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            if (rol.Permisos.Any(c => c.Id == familia.Id && c is FamiliaModelo577MC))
            {
                throw new Exception(string.Format(idioma.Translate("ExcRolYaTieneFamilia"), rol.Nombre, familia.Nombre));
            }

            var permisosDelRol = rol.ObtenerPermisos();
            var permisosDeLaFamilia = familia.obtenerPermisos();

            foreach (var permisoAportado in permisosDeLaFamilia)
            {
                if (permisosDelRol.Any(p => p.Id == permisoAportado.Id))
                {
                    throw new Exception(string.Format(idioma.Translate("ExcFamiliaPermisoYaPoseido"), permisoAportado.Nombre));
                }
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;
            bllBitacora.registrarEvento(dniAutor, $"Asigno la familia {familia.Nombre} a el rol {rol.Nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);

            _dal.asignarFamiliaARol(familia.Id, rol.Id);
        }

        public void EliminarRol(int idRol)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            if (_dal.tieneUsuariosAsignados(idRol))
            {
                throw new Exception(idioma.Translate("ExcRolConUsuariosAsignados"));
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;
            bllBitacora.registrarEvento(dniAutor, $"Elimino un rol.", Criticidad577MC.Alto, Modulos577MC.Perfil);

            _dal.eliminarRol(idRol);
            Services.DigitoVerificador577MC.ActualizarDVVRol();

        }

        public void DesasignarPatente(int idRol, int idPatente)
        {
            _dal.quitarPatenteDeRol(idRol, idPatente);
        }

        public void DesasignarFamilia(int idRol, int idFamilia)
        {
            _dal.quitarFamiliaDeRol(idRol, idFamilia);
        }

        

        

        

        #region Métodos de Ensamblaje

        private Dictionary<int, FamiliaModelo577MC> MapearFamiliasBase(DataTable dt)
        {
            var diccionario = new Dictionary<int, FamiliaModelo577MC>();
            foreach (DataRow row in dt.Rows)
            {
                var familia = new FamiliaModelo577MC
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                };

                diccionario.Add(familia.Id, familia);
            }

            return diccionario;
        }

        private Dictionary<int, PermisoModelo577MC> MapearPatentesBase(DataTable dt)
        {
            var diccionario = new Dictionary<int, PermisoModelo577MC>();

            foreach (DataRow row in dt.Rows)
            {
                var patente = new PermisoModelo577MC
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString()
                };

                diccionario.Add(patente.Id, patente);
            }

            return diccionario;
        }

        private void EnsamblarPatentesEnFamilias(Dictionary<int, FamiliaModelo577MC> familias, Dictionary<int, PermisoModelo577MC> patentes, DataTable dtRelaciones)
        {
            foreach (DataRow row in dtRelaciones.Rows)
            {
                int idFamilia = Convert.ToInt32(row["IdFamilia"]);
                int idPatente = Convert.ToInt32(row["IdPatente"]);

                if (familias.ContainsKey(idFamilia) && patentes.ContainsKey(idPatente))
                {
                    familias[idFamilia].agregarHijos(patentes[idPatente]);
                }
            }
        }

        private void EnsamblarFamiliasEnFamilias(Dictionary<int, FamiliaModelo577MC> familias, DataTable dtRelaciones)
        {
            foreach (DataRow row in dtRelaciones.Rows)
            {
                int idPadre = Convert.ToInt32(row["IdFamiliaPadre"]);
                int idHija = Convert.ToInt32(row["IdFamiliaHija"]);

                if (familias.ContainsKey(idPadre) && familias.ContainsKey(idHija))
                {
                    familias[idPadre].agregarHijos(familias[idHija]);
                }
            }
        }

        private void EnsamblarPatentesEnRoles(Dictionary<int, RolModelo577MC> dictRoles, Dictionary<int, PermisoModelo577MC> dictPatentes, DataTable dtRelaciones)
        {
            foreach (DataRow row in dtRelaciones.Rows)
            {
                int idRol = Convert.ToInt32(row["IdRol"]);
                int idPatente = Convert.ToInt32(row["IdPatente"]);

                // si existen tanto el rol como la patente en nuestros diccionarios
                if (dictRoles.ContainsKey(idRol) && dictPatentes.ContainsKey(idPatente))
                {
                    dictRoles[idRol].Permisos.Add(dictPatentes[idPatente]);
                }
            }
        }

        private void EnsamblarFamiliasEnRoles(Dictionary<int, RolModelo577MC> dictRoles, Dictionary<int, FamiliaModelo577MC> dictFamilias, DataTable dtRelaciones)
        {
            foreach (DataRow row in dtRelaciones.Rows)
            {
                int idRol = Convert.ToInt32(row["IdRol"]);
                int idFamilia = Convert.ToInt32(row["IdFamilia"]);

                // si existen tanto el rol como la familia en nuestros diccionarios
                if (dictRoles.ContainsKey(idRol) && dictFamilias.ContainsKey(idFamilia))
                {
                    dictRoles[idRol].Permisos.Add(dictFamilias[idFamilia]);
                }
            }
        }

        #endregion
    }
}
