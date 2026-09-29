using BE.Enum;
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
    public class BLLFamilia577MC
    {
        DALFamilia577MC _dal = new DALFamilia577MC();
        DALPatente577MC dalPatente = new DALPatente577MC();
        BLLBitacora577MC BLLBit = new BLLBitacora577MC();
        public List<FamiliaModelo577MC> ObtenerTodos()
        {
            DataTable dtFamilias = _dal.obtenerTodos();
            DataTable dtPatentes = dalPatente.obtenerTodos();
            DataTable dtRelFamiliaPatente = _dal.obtenerRelacionesFamiliaPatente();
            DataTable dtRelFamiliaFamilia = _dal.obtenerRelacionesFamiliaFamilia();

            var dictFamilias = MapearFamiliasBase(dtFamilias);
            var dictPatentes = MapearPatentesBase(dtPatentes);

            EnsamblarPatentesEnFamilias(dictFamilias, dictPatentes, dtRelFamiliaPatente);
            EnsamblarFamiliasEnFamilias(dictFamilias, dtRelFamiliaFamilia);

            return dictFamilias.Values.ToList();
        }
        public void CrearFamilia(string nombre, List<Componente577MC> componentes)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            DataTable dtFamilia = _dal.obtenerPorNombre(nombre);

            if (dtFamilia.Rows.Count > 0)
            {
                throw new Exception(string.Format(idioma.Translate("ExcNombreYaExiste"), idioma.Translate("TablaFamilia"), nombre));
            }

            FamiliaModelo577MC familia = new FamiliaModelo577MC { Nombre = nombre };

            foreach (Componente577MC comp in componentes) 
            {
                var permisosActuales = familia.obtenerPermisos();

                if (comp is PermisoModelo577MC patente)
                {
                    if (permisosActuales.Any(p => p.Id == patente.Id))
                    {
                        throw new Exception(string.Format(idioma.Translate("ExcConflictoPatenteIndirecta"), patente.Nombre));
                    }
                }

                else if (comp is FamiliaModelo577MC familiaHija)
                {
                    var permisosHija = familiaHija.obtenerPermisos();

                    foreach (var p in permisosHija)
                    {
                        if (permisosActuales.Any(pa => pa.Id == p.Id))
                        {
                            throw new Exception(string.Format(idioma.Translate("ExcConflictoFamiliaPermisos"), familiaHija.Nombre));
                        }
                    }
                }

                familia.agregarHijos(comp);
            }

            int nuevoFamiliaId = _dal.insertarFamilia(nombre);
            long dvhInicial = Services.DigitoVerificador577MC.CalcularDVH(nuevoFamiliaId.ToString() + nombre);
            _dal.ActualizarDVH(nuevoFamiliaId, dvhInicial);
            Services.DigitoVerificador577MC.ActualizarDVVFamilia();

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;

            foreach (Componente577MC comp in componentes)
            {
                if (comp is PermisoModelo577MC patente)
                {
                    _dal.asignarPatenteAFamilia(patente.Id, nuevoFamiliaId);
                    BLLBit.registrarEvento(dniAutor, $"Asignó la patente {patente.Nombre} a la familia {nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);
                }
                else if (comp is FamiliaModelo577MC familiaHija)
                {
                    _dal.asignarFamiliaAFamilia(nuevoFamiliaId, familiaHija.Id);
                    BLLBit.registrarEvento(dniAutor, $"Asignó la familia {familiaHija.Nombre} a la familia {nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);
                }
            }

            BLLBit.registrarEvento(dniAutor, $"Creo una nueva familia", Criticidad577MC.Alto, Modulos577MC.Perfil);

        }

        public void AsignarPatente(FamiliaModelo577MC familia, PermisoModelo577MC patente)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            var permisosAplanados = familia.obtenerPermisos();

            if (permisosAplanados.Any(p => p.Id == patente.Id))
            {
                throw new Exception(string.Format(idioma.Translate("ExcFamiliaYaContienePermiso"), familia.Nombre, patente.Nombre));
            }

            BLLRol577MC bllRol = new BLLRol577MC();
            List<RolModelo577MC> todosLosRoles = bllRol.ObtenerRolesConJerarquia();

            foreach (RolModelo577MC rol in todosLosRoles)
            {
                bool rolUsaEstaFamilia = RolUsaFamilia(rol, familia.Id);

                if (rolUsaEstaFamilia)
                {
                    // si el rol usa la familia, sacamos sus patentes aplanadas para ver si ya tiene la patente por otra vía
                    var permisosDelRol = rol.ObtenerPermisos();
                    bool rolYaTienePatente = permisosDelRol.Any(p => p.Id == patente.Id);

                    if (rolYaTienePatente)
                    {
                        throw new Exception(string.Format(idioma.Translate("ExcRolUsaFamiliaConPatente"), rol.Nombre, familia.Nombre, patente.Nombre));
                    }
                }
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;
            BLLBit.registrarEvento(dniAutor, $"Asigno la patente {patente.Nombre} a la familia {familia.Nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);

            _dal.asignarPatenteAFamilia(patente.Id, familia.Id);
        }

        public void AsignarFamilia(FamiliaModelo577MC familiaPadre, FamiliaModelo577MC familiaHija)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            if (familiaPadre.Id == familiaHija.Id)
            {
                throw new Exception(idioma.Translate("ExcFamiliaAsignadaAsiMisma"));
            }

            var permisosPadre = familiaPadre.obtenerPermisos();
            var permisosHija = familiaHija.obtenerPermisos();

            foreach (var permiso in permisosHija)
            {
                //si el padre ya tiene un permiso que la hija intenta aportar, hay redundancia
                if (permisosPadre.Any(p => p.Id == permiso.Id))
                {
                    throw new Exception(string.Format(idioma.Translate("ExcFamiliaHijaPermisoDuplicado"), familiaHija.Nombre, permiso.Nombre, familiaPadre.Nombre));
                }
            }

            BLLRol577MC bllRol = new BLLRol577MC();
            List<RolModelo577MC> todosLosRoles = bllRol.ObtenerRolesConJerarquia();

            foreach (RolModelo577MC rol in todosLosRoles)
            {
                bool rolUsaEstaFamilia = RolUsaFamilia(rol, familiaPadre.Id);

                if (rolUsaEstaFamilia)
                {
                    var permisosDelRol = rol.ObtenerPermisos();

                    // evaluamos si las patentes aplanadas de la familia hija generarían choque
                    foreach (var patenteAportada in permisosHija)
                    {
                        if (permisosDelRol.Any(p => p.Id == patenteAportada.Id))
                        {
                            throw new Exception(string.Format(idioma.Translate("ExcRolDuplicariaPatente"), rol.Nombre, familiaPadre.Nombre, familiaHija.Nombre, patenteAportada.Nombre));
                        }
                    }
                }
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;
            BLLBit.registrarEvento(dniAutor, $"Asigno la familia {familiaHija.Nombre} a la familia {familiaPadre.Nombre}.", Criticidad577MC.Alto, Modulos577MC.Perfil);

            _dal.asignarFamiliaAFamilia(familiaPadre.Id, familiaHija.Id);
        }

        public void EliminarFamilia(int idFamilia)
        {
            var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;

            if (_dal.tieneDependencias(idFamilia)) 
            {
                throw new Exception(idioma.Translate("ExcFamiliaConDependencias"));
            }

            string dniAutor = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo.DNI;
            BLLBit.registrarEvento(dniAutor, $"Elimino una familia.", Criticidad577MC.Alto, Modulos577MC.Perfil);

            _dal.eliminarFamilia(idFamilia);
            Services.DigitoVerificador577MC.ActualizarDVVFamilia();
        }

       

        

        

        #region Verificacion hacia arriba
        private bool RolUsaFamilia(RolModelo577MC rol, int idFamiliaBuscada)
        {
            return BuscarFamiliaEnNodos(rol.Permisos, idFamiliaBuscada);
        }

        private bool BuscarFamiliaEnNodos(IEnumerable<Componente577MC> nodos, int idFamiliaBuscada)
        {
            foreach (var nodo in nodos)
            {
                if (nodo is FamiliaModelo577MC familia)
                {
                    // si es la familia que estamos buscando
                    if (familia.Id == idFamiliaBuscada)
                    {
                        return true;
                    }

                    // si no es, buscamos recursivamente adentro de sus hijos
                    if (BuscarFamiliaEnNodos(familia.obtenerPermisos(), idFamiliaBuscada))
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        #endregion

        #region Mapear y Ensamblar
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

        #endregion
    }
}
