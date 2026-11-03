# PROJECT_MAP — Mapa de archivos

## 1. Raíz (proyecto GUI `Servicios`)

| Archivo | Rol |
|---|---|
| `Program.cs` | Punto de entrada; `Application.Run(new Login())` (línea 20); flujo de configuración inicial comentado |
| `App.config` | Cadena `ConexionPrincipal` |
| `Servicios.sln` | Solución con 5 proyectos C# + `.vdproj` inexistente |
| `GUI_577MC.csproj` | Proyecto WinExe, assembly `Servicios`, .NET 4.7.2 |
| `README.md` | Solo el título "Sistema de Gestión Inmobiliaria" (sin documentación) |
| `packages/` | Vacía (0 archivos) |

### 1.1 Formularios

| Formulario | Archivos | Responsabilidad | Puntos clave |
|---|---|---|---|
| `Login` | `Login.cs`, `.Designer.cs`, `.resx` | Autenticación, integridad, derivación | `btnLogin_Click` (39); verifica DV (52); bloquea no-admin si hay inconsistencias (60-70); deriva a `RepararInconsistencias` (72) o `CambiarContraseña` (86) |
| `MenuPrincipal` | `MenuPrincipal.cs`, `.Designer.cs`, `.resx` | Menú y navegación | `configurarAcceso()` (75) habilita ítems por permiso; abre formularios con `Show()` |
| `CambiarContraseña` | `CambiarContraseña.cs`, ... | Cambio de clave | `btnAceptar_Click` (40); nombre de archivo con encoding corrupto |
| `Configuracion_Inicial` | `Configuracion Inicial.cs`, ... | Asistente de conexión/creación de BD | `ConstruirConnectionStringDesdeFormulario` (48), `ProbarConexion_Click` (70), `btnGuardarYContinuar_Click` (108); no referencia `IIdiomaObserver` |
| `RegistrarCliente` | `RegistrarCliente.cs`, ... | ABM de clientes + serialización XML | Constructor ABM (25) y constructor modal con DNI (38); guardar (55), modificar (106), eliminar (148), serializar (250), deserializar (315) |
| `SeleccionarPropiedad` | `SeleccionarPropiedad.cs`, ... | Catálogo, búsqueda, selección y ABM de propiedades | Constructor `(modoAbm, soloOperables)`; `Seleccionar()` (194), `CargarABM` (282), `btnGuardarProp_Click` (374), `btnEliminarProp_Click` (437) |
| `ProgramarVisita` | `ProgramarVisita.cs`, ... | Programar visita, consultar disponibilidad, emitir comprobante | `BuscarOCrearCliente` (65), `AbrirAltaCliente` (87), `btnConfirmar_Click` (149), `ProgramarVisita` (188), `MostrarComprobanteRN1` (231) |
| `ActualizarEstadoVisita` | `ActualizarEstadoVisita.cs`, ... | Buscar visitas y cambiar estado/observaciones | `btnBuscar_Click` (52), `btnGuardar_Click` (105), `VisitaSeleccionada` (131), `CargarConFiltro` (148) |
| `ConsultarDisponibilidad` | `ConsultarDisponibilidad.cs`, ... | Diálogo de consulta de disponibilidad | **Nuevo/untracked**; `btnConsultar_Click` (64), `MostrarResultado` (106), `btnAceptar_Click` (141) |
| `GestionUsuario` | `GestionUsuario.cs`, ... | ABM de usuarios, activar/desactivar, desbloquear | `btnGuardar_Click` (178), `btnActDesact_Click` (301), `btnDesbloquear_Click` (324), validadores (289-299) |
| `GestionRol` | `GestionRol.cs`, ... | Crear rol, asignar/quitar patentes y familias | `cargarDatos` (36), `btnAplicar_Click` (162), `btnEliminar_Click` (302) |
| `GestionFamilia` | `GestionFamilia.cs`, ... | Crear familia, asignar patentes/familias, eliminar | `cargarDatos` (42), `btAplicar_Click` (166), `btEliminar_Click` (156) |
| `AuditoriaBitacora` | `AuditoriaBitacora.cs`, ... | Consulta/filtro de bitácora e impresión | `CargarGrillaInicial` (37), `btnFiltrar_Click` (68), `printDoc_PrintPage` (99), `btnPDF_Click` (194; abre `PrintDialog` y manda a imprimir, no genera PDF) |
| `GestionRespaldo` | `GestionRespaldo.cs`, ... | Backup y restore desde UI | `btnRealizarBackUp_Click`, `btnRealizarRestore_Click`; `Application.Restart()` tras restore |
| `RepararInconsistencias` | `RepararInconsistencias.cs`, ... | Recalcular DVH/DVV o restaurar backup | `btnRecalcular_Click`, `btnRestore_Click`; recibe flags por constructor |
| `FrmGenerarContrato` | `FrmGenerarContrato.cs`, ... | Generar contrato (CUN06) | **Untracked**; valida DNI/cliente, elige propiedad operable (`soloOperables`) y términos; valida con `BLLContrato577MC.ValidarOperabilidadPropiedad` y llama a `ValidarYGenerarContrato` |
| `FrmRegistrarTerminosCondiciones` | `FrmRegistrarTerminosCondiciones.cs`, ... | Modal de términos (CUN07) | Solo `Venta`/`Alquiler`; valida con `BLLContrato577MC.ValidarTerminos`; no persiste |
| `FrmActualizarEstadoPropiedadContrato` | `FrmActualizarEstadoPropiedadContrato.cs`, ... | Actualizar estado de contrato y propiedad (CUN08) | Grilla `Pendiente`, combo `Firmado`/`Cancelado`; al firmar emite RN2 y previsualiza |
| `FrmReimprimirContrato` | `FrmReimprimirContrato.cs`, ... | Reimprimir copia final | Grilla de contratos `Firmado`; botón/doble clic generan `ReporteRN2577MC.Emitir(contrato, false)` |

> Total: 19 formularios. Los `Reporte*.cs` no son formularios: son clases estáticas de generación de PDF.

### 1.2 Generación de reportes

| Archivo | Rol |
|---|---|
| `ReportePdf577MC.cs` | Motor compartido: A4 200 DPI, GDI+ → JPEG calidad 95 → PDF 1.4; encabezado con franja y logo, pie en 3 columnas, secciones numeradas, paneles redondeados, píldoras, chips, avatares, notas, código de barras Code 39 y vista previa fiel; Segoe UI con fallback a Verdana; formatos `es-AR` |
| `ReporteRN1577MC.cs` | Comprobante de visita RN1: hero con avatar y tiles de fecha/horario, tarjeta de propiedad con pin, timeline del estado, nota, recomendaciones y código de barras; salida `Downloads\Propiedades Calella\Comprobantes` |
| `ReporteRN2577MC.cs` | Copia final RN2: partes con avatares, propiedad con chips, términos con monto destacado, estado, código de barras y firmas en tarjeta; salida `Downloads\Propiedades Calella\Contratos` |

## 2. BE (`BE_577MC.csproj`, assembly `BE`)

| Archivo | Contenido |
|---|---|
| `Cliente577MC.cs` | DNI, Nombre, Apellido, Teléfono, Email |
| `Propiedad577MC.cs` | Id, Dirección, Tipo, Estado, Precio, SuperficieM2, Ambientes, Dormitorios, Baños, DVH |
| `Visita577MC.cs` | IdVisita, IdPropiedad, DNICliente, Fecha, HoraInicio, HoraFin, Estado, Observaciones, DVH; propiedades calculadas `EsFinalizada` y `RangoHorario` |
| `Contrato577MC.cs` | **Nuevo/untracked**: NroContrato, FechaEmision, DNIPropietario, DNICliente, CodigoPropiedad, TipoContrato, FechaInicio, FechaFin, MontoAcordado, FormaPago, DepositoGarantia, Estado, DVH y `PropiedadDireccion` (solo presentación) |
| `TerminosContrato577MC.cs` | **Nuevo/untracked**: DTO de CUN07 con los seis términos comerciales |
| `Idioma577MC.cs` | Id, Nombre |
| `InstanciaSql577MC.cs` | NombreServidor, EsLocal |
| `Enum/Criticidad577MC.cs` | Enum de criticidad (Bajo/Medio/Alto) usada en bitácora |
| `Enum/Modulos577MC.cs` | Enum de módulos (Seguridad, Usuario, Cliente, etc.) |
| `Properties/AssemblyInfo.cs` | Metadatos del ensamblado |

## 3. DAL (`DAL_577MC.csproj`, assembly `DAL`)

| Archivo | Métodos principales |
|---|---|
| `DALAcceso577MC.cs` | `executeDataTable` (19), `executeDataTableBatch` (50), `executeNonQuery` (89), `executeScalar` (116), `DetalleDelError` (144), `RegistrarErrorLog` (165) |
| `DALConexionConfig577MC.cs` | `ObtenerConnectionString()` (14) lee `ConexionPrincipal` |
| `DALCliente577MC.cs` | `ExisteCliente` (14), `ObtenerTodos` (28), `ObtenerPorDNI` (37), `TieneVisitas` (51), `ModificarCliente` (65), `EliminarCliente` (84), `InsertarCliente` (96) |
| `DALPropiedad577MC.cs` | `ObtenerTodas` (14), `ObtenerPorId`, `ExistePropiedad`, `Buscar`, `TieneVisitas`, `InsertarPropiedad`, `ModificarPropiedad`, `EliminarPropiedad`, `ActualizarEstado`, `ActualizarDVH` |
| `DALVisita577MC.cs` | `ObtenerVisitasPorPropiedadYFecha` (14), `ObtenerVisitas` (32), `ObtenerVisitaPorId` (60), `ActualizarEstadoVisita` (76), `InsertarVisita` (93) |
| `DALUsuario577MC.cs` | `InsertarUsuario` (17), `DesactivarUsuario` (39), `ActivarUsuario` (52), `ModificarUsuario` (64), `CambiarPassword` (78), `GuardarIdioma` (95), `obtenerTodos` (110), `obtenerPorDNI` (119), `obtenerPorEmail` (136), `obtenerPorUser` (153), `aumentarIntento` (174), `reiniciarIntentos` (192), `bloquearUsuario` (213), `desbloquearUsuario` (225), `ActualizarDVH` (244) |
| `DALRol577MC.cs` | `obtenerTodos` (14), `obtenerPorNombre` (23), asignaciones (35-64), `insertarRol` (65), `eliminarRol` (79), `tieneUsuariosAsignados` (91), quitar permisos (106-127), relaciones (128-140), `ObtenerDatosAcceso` (141), `ActualizarDVH` (154) |
| `DALFamilia577MC.cs` | `asignarPatenteAFamilia` (15), relaciones (30-41), `obtenerTodos` (42), `obtenerPorNombre` (49), `asignarFamiliaAFamilia` (60), `tieneDependencias` (75), `eliminarFamilia` (94), `insertarFamilia` (108), `ActualizarDVH` (122) |
| `DALPatente577MC.cs` | `obtenerTodos` (15), `ActualizarDVH` (22) |
| `DALBitacora577MC.cs` | `insertarLog` (19), `obtenerBitacora` (39) |
| `DALIdioma577MC.cs` | `obtenerTodos` (14) |
| `DALDigitoVerificador577MC.cs` | `GuardarDVV` (16), `ActualizarDVV` (30), `ExisteTabla` (42), `ObtenerFila` (55), `ObtenerDatosVerificacion` (68) |
| `DALBackUpRestore577MC.cs` | `realizarBackUp` (18), `realizarRestore` (32); instancia hardcodeada (15) |
| `DALContrato577MC.cs` | **Nuevo/untracked**: `ExisteContrato`, `ObtenerSiguienteSecuencial`, `InsertarContrato`, `ObtenerPorNro`, `ObtenerTodos`, `ObtenerPendientes`, `ObtenerContratoPendiente` (valida duplicados por propiedad), `ObtenerFirmados`, `ActualizarEstado`, `ActualizarEstadoConPropiedad` (transacción SQL), `ActualizarDVH`; la tabla `Contrato` existe en la base pero no en los scripts |

## 4. BLL (`BLL_577MC.csproj`, assembly `BLL`)

| Archivo | Métodos principales |
|---|---|
| `BLLUsuario577MC.cs` | `obtenerTodos` (21), `login` (34), `CrearUsuario` (121), `activarDesactivar` (153), `ModificarUsuario` (179), `cambiarPassword` (189), `MapearUsuario` (217), `DesbloquearUsuario` (242), `GuardarIdioma` (274), `CalcularDVHUsuario` (283), `RecalcularDVHUsuario` (307), `RepararDVH` (329), `GenerarUsuario` (335), `GenerarPassword` (340) |
| `BLLCliente577MC.cs` | `ExisteCliente` (25), `ObtenerTodos` (35), `ObtenerPorDNI` (56), `ModificarCliente` (80), `EliminarCliente` (100), `RegistrarCliente` (123) |
| `BLLPropiedad577MC.cs` | `ObtenerTodas`, `ObtenerPorId`, `TieneVisitas`, `RegistrarPropiedad`, `ModificarPropiedad`, `EliminarPropiedad`, `Buscar`; DVH centralizado en `Services.DigitoVerificador577MC` |
| `BLLContrato577MC.cs` | **Nuevo/untracked**: `ClienteExiste`, `PropiedadExiste`, `ValidarOperabilidadPropiedad` (estado operable y sin contrato `Pendiente`), `ObtenerContratosPendientes`, `ObtenerContratosFirmados`, `ValidarTerminos`, `ValidarYGenerarContrato`, `ActualizarEstadoContrato`, `FirmarContrato`, `CancelarContrato`, mapeos y bitácora |
| `BLLVisita577MC.cs` | `ObtenerVisitasPorPropiedadYFecha` (28), `ObtenerVisitas` (34), `ConsultarDisponibilidad` (52), `ProgramarVisita` (62), `ActualizarEstadoVisita` (83) |
| `BLLRol577MC.cs` | `obtenerTodos` (19), `ObtenerRolesConJerarquia` (37), `crearRol` (73), `AsignarPatente` (141), `AsignarFamilia` (158), `EliminarRol` (184), `DesasignarPatente` (201), `DesasignarFamilia` (206) |
| `BLLFamilia577MC.cs` | `ObtenerTodos` (18), `CrearFamilia` (33), `AsignarPatente` (99), `AsignarFamilia` (136), `EliminarFamilia` (185) |
| `BLLPatente577MC.cs` | `obtenerTodos` (15) |
| `BLLBitacora577MC.cs` | `registrarEvento` (16), `obtenerUltimos3Dias` (22), `obtenerBitacora` (30) |
| `BLLIdioma577MC.cs` | `obtenerTodos` (16) |

## 5. Services (`Services_577MC.csproj`, assembly `Services`)

| Archivo | Rol |
|---|---|
| `ServiceSessionManager577MC.cs` | Singleton de sesión (`namespace Services_577MC`) |
| `IdiomaManager.cs` | Carga JSON, notifica observadores, traduce |
| `DigitoVerificador577MC.cs` | DVH/DVV, verificación, reparación, restore delegado |
| `BackUpRestore577MC.cs` | Fachada de backup/restore |
| `ServiceSeguridad577MC.cs` | `Hashear` (SHA-256) |
| `Instalador/ServicioConfiguracionConexion.cs` | Detecta instancias, construye/prueba/guarda cadena, ejecuta script de creación |
| `Modelos/UsuarioModelo577MC.cs` | Usuario de sesión (DNI, datos, rol, idioma, DVH) |
| `Modelos/BitacoraEventosModelo577MC.cs` | Evento de bitácora |
| `Modelos/CompositeRol/Componente577MC.cs` | Abstracto del composite |
| `Modelos/CompositeRol/RolModelo577MC.cs` | Nodo rol |
| `Modelos/CompositeRol/FamiliaModelo577MC.cs` | Nodo familia |
| `Modelos/CompositeRol/PermisoModelo577MC.cs` | Hoja patente |
| `Modelos/Idioma/IIdiomaObserver.cs` | `actualizarIdioma()` |

## 6. Scripts SQL

| Script | Propósito | Nota |
|---|---|---|
| `scriptTerceraEntrega.sql` | Dump completo: crea BD, esquema, seeds, FKs, DVV | 765 líneas; **DDL de `Visita` inválido**; no incluye `Contrato` |
| `scriptTablaPropiedad.sql` | Crea/carga tabla `Propiedad` | Delta sobre BD existente |
| `scriptTablaVisita.sql` | Crea/carga tabla `Visita` | Delta; puede tener el mismo problema de DDL que el dump |
| `scriptActualizarEstadoVisita.sql` | Agrega columna `Observaciones` a `Visita` | Idempotente |
| `scriptSeguridadAgenteInmobiliario.sql` | Crea patente `Gestion Clientes`, familia y rol `Agente Inmobiliario` | Delta de seguridad |
| `scriptSeguridadSeleccionarPropiedad.sql` | Agrega patente `Seleccionar Propiedad` y la asigna | Recalcula DVV/DVH de `Patente` |
| `scriptSeguridadAgendarVisita.sql` | Agrega patente `Agendar Visita` | Ídem |
| `scriptSeguridadActualizarEstadoVisita.sql` | Agrega patente `Actualizar Estado Visita` (Id 13) | Ídem |
| `scriptOptimizacionesRendimiento.sql` | Índices para login/consultas | Advierte índices únicos duplicados en Email |
| `scriptVerificarTablaCliente.sql` | Consultas de verificación de estructura | Solo diagnóstico |

## 7. Recursos

| Ruta | Uso |
|---|---|
| `Lenguajes/es.json`, `en.json` | Traducciones (incluye `Reporte.*`, `ReporteRN1.*`, `ReporteRN2.*` y textos de menú) |
| `Recursos/logo-propiedades-calella.jpg` | Logo de los PDF (1020×323 px); se copia al output y se resuelve de forma portable |
| `*.resx` | Recursos de cada formulario |

## 8. Archivos que no deben modificarse sin autorización

- CUN06/CUN07/CUN08 y reportes (implementados y verificados): `BE/Contrato577MC.cs`, `BE/TerminosContrato577MC.cs`, `DAL/DALContrato577MC.cs`, `BLL/BLLContrato577MC.cs`, `FrmGenerarContrato.*`, `FrmRegistrarTerminosCondiciones.*`, `FrmActualizarEstadoPropiedadContrato.*`, `FrmReimprimirContrato.*`, `ReportePdf577MC.cs`, `ReporteRN1577MC.cs`, `ReporteRN2577MC.cs`.
- `ConsultarDisponibilidad.cs`, `ConsultarDisponibilidad.Designer.cs` (trabajo en curso).
- `Scripts/scriptTerceraEntrega.sql` (fuente de verdad del esquema; corregirlo requiere decisión).
- `App.config` (cadena de conexión local).
