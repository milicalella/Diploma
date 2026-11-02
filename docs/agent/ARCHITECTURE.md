# ARCHITECTURE — Arquitectura real

## 1. Resumen

Arquitectura en capas al estilo clásico de aplicaciones de escritorio:

```
┌──────────────────────────────────────────────┐
│ GUI (WinForms, namespace Servicios)          │
│ Login, MenuPrincipal, ABMs, selectores       │
└───────────────┬──────────────────────────────┘
                │ instancia directa (new)
┌───────────────▼──────────────────────────────┐
│ BLL (namespace BLL)                          │
│ validaciones, mapeo DataTable→entidad,       │
│ DVH, bitácora, reglas de negocio             │
└───────┬───────────────────────┬──────────────┘
        │                       │
┌───────▼──────────────┐  ┌─────▼──────────────────────┐
│ DAL (namespace DAL)  │  │ Services (namespace        │
│ SQL inline + ADO.NET │  │ Services / Services_577MC) │
└───────┬──────────────┘  │ sesión, i18n, DV, backup   │
        │                 └─────┬──────────────────────┘
┌───────▼───────────────────────▼──────────────┐
│ DALAcceso577MC                               │
│ executeDataTable / Batch / NonQuery / Scalar │
└───────────────┬──────────────────────────────┘
                │ SqlConnection + SqlCommand
┌───────────────▼──────────────────────────────┐
│ SQL Server — catálogo is--servicios          │
└──────────────────────────────────────────────┘
```

## 2. Regla de dependencias observada

| Proyecto | Referencia a | Fuente |
|---|---|---|
| GUI (`Servicios`) | BE, BLL, Services | `GUI_577MC.csproj:225-235` |
| BLL | BE, DAL, Services | `BLL/BLL_577MC.csproj:56-67` |
| DAL | BE | `DAL/DAL_577MC.csproj:65-68` |
| Services | BE, DAL | `Services/Services_577MC.csproj:62-69` |
| BE | ninguna | `BE/BE_577MC.csproj` |

La dirección nominal es `GUI → BLL → DAL → SQL`. Sin embargo, **Services también referencia DAL**, por lo que hay dependencias laterales:

- `BLL → Services → DAL`
- `GUI → Services → DAL`
- `BLL` consume `Services_577MC.ServiceSessionManager577MC`, `Services.DigitoVerificador577MC`, `Services.ServiceSeguridad577MC`.

No hay inyección de dependencias: cada clase instancia sus colaboradores con `new` a nivel de campo (por ejemplo `BLLCliente577MC.cs:16-17`, `ProgramarVisita.cs:14-15`).

## 3. Capas y responsabilidades

### 3.1 GUI

- Formularios WinForms con `*.Designer.cs` generado y `*.resx`.
- Namespace `Servicios` (no `GUI`).
- La lógica de presentación vive en el code-behind: validaciones de campos, confirmaciones, navegación, formateo de mensajes.
- Acceso a sesión/idioma vía singleton `ServiceSessionManager577MC.getIntancia()`.
- Ver `docs/agent/WINFORMS_PATTERNS.md`.

### 3.2 BLL

- Una clase por entidad o agregado: `BLLCliente577MC`, `BLLPropiedad577MC`, `BLLVisita577MC`, `BLLUsuario577MC`, `BLLRol577MC`, `BLLFamilia577MC`, `BLLPatente577MC`, `BLLBitacora577MC`, `BLLIdioma577MC`.
- Valida reglas (existencia, duplicados, dependencias, superposición de visitas).
- Convierte `DataTable` en objetos `BE`.
- Calcula y persiste DVH; registra bitácora.
- Lanza `Exception` con mensajes traducidos; la UI captura y muestra.

### 3.3 DAL

- Una clase por tabla/agregado: `DALCliente577MC`, `DALPropiedad577MC`, `DALVisita577MC`, `DALUsuario577MC`, `DALRol577MC`, `DALFamilia577MC`, `DALPatente577MC`, `DALBitacora577MC`, `DALIdioma577MC`, `DALDigitoVerificador577MC`, `DALContrato577MC` (CUN06/CUN08), `DALBackUpRestore577MC`.
- SQL embebido como string, parámetros con `Dictionary<string, object>` y `AddWithValue`.
- Devuelve `DataTable`, `DataRow`, `bool`, `int` o `void`; **no** devuelve entidades.
- No hay repositorios genéricos ni Unit of Work.

### 3.4 Infraestructura de datos

- `DALAcceso577MC` centraliza la ejecución y el logging de errores SQL.
- Abre y cierra una `SqlConnection` por llamada (`using`), sin pooling explícito (lo maneja ADO.NET).
- `DALConexionConfig577MC` lee `ConexionPrincipal` de `ConfigurationManager`.
- `executeDataTableBatch` ejecuta varias sentencias separadas y devuelve varios `DataTable`; se usa para armar permisos en una sola llamada.

### 3.5 Services

| Clase | Rol |
|---|---|
| `ServiceSessionManager577MC` | Singleton de sesión; expone `usuarioActivo`, `Login`, `Logout`, `estaLogueado`, `TienePermiso`, `Idioma` |
| `IdiomaManager` | Observer de idioma; carga JSON y notifica a `IIdiomaObserver` |
| `DigitoVerificador577MC` | Cálculo/verificación/reparación de DVH y DVV; consulta DAL directamente |
| `BackUpRestore577MC` | Fachada de backup/restore |
| `ServiceSeguridad577MC` | Hash SHA-256 de contraseñas |
| `Instalador/ServicioConfiguracionConexion` | Prueba y guarda cadena de conexión (flujo comentado en `Program.cs`) |
| `Modelos/CompositeRol/*` | Composite de permisos: `Componente577MC`, `RolModelo577MC`, `FamiliaModelo577MC`, `PermisoModelo577MC` |
| `Modelos/UsuarioModelo577MC` | Usuario de sesión |
| `Modelos/BitacoraEventosModelo577MC` | Modelo de evento |
| `Modelos/Idioma/IIdiomaObserver` | Contrato `actualizarIdioma()` |

## 4. Excepciones a la arquitectura por capas

Estas son las desviaciones reales; un agente debe conocerlas antes de "corregir" nada:

1. **GUI → Services → DAL** para integridad y respaldo:
   - `Login.cs:52` llama `DigitoVerificador577MC.VerificarIntegridad()`.
   - `RepararInconsistencias.cs` llama `DigitoVerificador577MC.RepararUsuario/Rol/Familia/Patente` y `RealizarRestore`.
   - `GestionRespaldo.cs:16` instancia `BackUpRestore577MC`.
2. **BLL → Services** para sesión, hash, traducciones y DVH.
3. **Services → DAL** (capa de servicios acoplada a datos).
4. **Formularios que abren otros formularios** directamente (`ProgramarVisita` → `RegistrarCliente`, `SeleccionarPropiedad`, `ConsultarDisponibilidad`), sin pasar por un coordinador.
5. **Lógica de integridad en el login**: el formulario decide si bloquea o deriva a reparación (`Login.cs:52-76`), no un servicio de aplicación.

## 5. Flujos transversales

### 5.1 Sesión

- Singleton con constructor privado: `ServiceSessionManager577MC.cs:12-31`.
- `getIntancia()` crea la instancia de forma perezosa, **sin lock**.
- `usuarioActivo` es público con setter privado; se asigna en `Login(UsuarioModelo577MC)`.
- `TienePermiso` recorre `usuarioActivo.Rol.ObtenerPermisos()` comparando nombres sin distinguir mayúsculas (`ServiceSessionManager577MC.cs:49-64`).
- **Riesgo:** si se llama `TienePermiso` sin sesión, `usuarioActivo` es `null` → `NullReferenceException`.
- El rol se hidrata con jerarquía en `BLLUsuario577MC.login` (líneas 92-101) usando `BLLRol577MC.ObtenerRolesConJerarquia`.

### 5.2 Permisos (Composite)

- `RolModelo577MC`, `FamiliaModelo577MC` y `PermisoModelo577MC` heredan de `Componente577MC`.
- Un rol contiene patentes y familias; una familia puede contener patentes y otras familias.
- `BLLRol577MC.ObtenerRolesConJerarquia()` arma el árbol; `DALRol577MC.ObtenerDatosAcceso()` trae rol-patente, rol-familia y familia-patente en un batch.
- Los nombres de patente usados por la UI están en `MenuPrincipal.configurarAcceso()` (`MenuPrincipal.cs`): `Cambiar Clave`, `Cerrar Sesion`, `Gestion Usuario`, `Iniciar Sesion`, `Auditoria Eventos`, `Gestion Roles`, `Gestion Familia`, `Cambiar Idioma`, `Gestion Respaldo`, `Gestion Clientes`, `Seleccionar Propiedad`, `Agendar Visita`, `Actualizar Estado Visita`.
- Los tres ítems del submenú **Contratos** (generar, actualizar estado y reimprimir copia final) se habilitan con `Gestion Clientes` **o** `Seleccionar Propiedad`.
- **La UI solo deshabilita ítems del menú.** No hay verificación dentro de los formularios ni en BLL/DAL. Ver `KNOWN_ISSUES.md`.

### 5.3 Integridad DVH/DVV

- `DigitoVerificador577MC.CalcularDVH(string)` y `CalcularDVV(IEnumerable<long>)` (`Services/DigitoVerificador577MC.cs:24-44`).
- `VerificarIntegridad()` devuelve `ResultadoVerificacion` con flags por tabla (`Usuario`, `Rol`, `Familia`, `Patente`) (`líneas 150-184`).
- `RepararUsuario/Rol/Familia/Patente` recalculan DVH de cada fila, actualizan DVV y registran el evento (`líneas 191-194`).
- `ActualizarDVVUsuario/Rol/Familia/Patente` recalculan solo el DVV (`líneas 210-213`).
- La tabla `DigitoVerificador` guarda una fila por tabla con DVV y un DVH de control.
- **Defecto conocido:** `BLLUsuario577MC.CrearUsuario` llama `ActualizarDVVUsuario()` **antes** de insertar el usuario (`BLLUsuario577MC.cs:144-146`), dejando el DVV desactualizado.

### 5.4 Internacionalización

- `IdiomaManager` mantiene una lista de observadores y un diccionario de traducciones (`Services/IdiomaManager.cs:12-15`).
- `CargarIdioma(cod)` lee `Lenguajes/{cod}.json` desde el directorio de la aplicación y notifica (`líneas 35-51`).
- `Translate(key)` devuelve la clave si no hay diccionario o no existe la entrada (`líneas 53-60`).
- Los formularios implementan `IIdiomaObserver` y se suscriben en el constructor.
- **Defecto conocido:** la mayoría no se desuscribe al cerrarse. Se desuscriben `SeleccionarPropiedad`, `FrmGenerarContrato`, `FrmActualizarEstadoPropiedadContrato` y `FrmReimprimirContrato`.

### 5.5 Auditoría

- `BLLBitacora577MC.registrarEvento(dni, evento, criticidad, modulo)` inserta en `BitacoraEventos`.
- `Criticidad577MC` y `Modulos577MC` son enums en `BE/Enum/`.
- `DALBitacora577MC.insertarLog` recibe `int criticidad, int modulo`; la columna `Modulo` es `varchar(50)` y los seeds son strings numéricos. SQL Server convierte implícitamente. [VERIFICADO en script]

### 5.6 Respaldo / restauración

- `GestionRespaldo` → `BackUpRestore577MC` → `DALBackUpRestore577MC`.
- Backup: `BACKUP DATABASE [is--servicios] TO DISK = @ruta WITH FORMAT`.
- Restore: conecta a `master`, `ALTER DATABASE ... SINGLE_USER`, `RESTORE ... WITH REPLACE`, `MULTI_USER`.
- La cadena usada para restore está hardcodeada y **no** es la de `App.config`.
- El restore no valida que el archivo exista ni que sea un backup válido antes de ejecutar.

## 6. Namespaces

| Namespace | Ubicación | Observación |
|---|---|---|
| `BE`, `BE.Enum` | `BE/` | entidades y enums |
| `DAL` | `DAL/` | acceso a datos |
| `BLL` | `BLL/` | reglas |
| `Services` | `Services/` | i18n, hash, DV, backup, instalador |
| `Services.Modelos`, `Services.Modelos.Idioma`, `Services.Modelos.CompositeRol` | `Services/Modelos/` | modelos |
| `Services_577MC` | `Services/ServiceSessionManager577MC.cs` | **inconsistencia**: el proyecto `Services` contiene dos namespaces raíz |
| `Servicios` | `*.cs` de UI | coincide con el nombre del ensamblado, no con el proyecto |

`BLL` usa `using Services;` para DVH/hash y referencia `Services_577MC.ServiceSessionManager577MC` con el nombre completo (por ejemplo `BLLCliente577MC.cs:4` y `BLLCliente577MC.cs:21`). Cualquier refactor de namespaces impacta en todos los proyectos.

## 7. Construcción y despliegue

- `Servicios.sln` incluye un `.vdproj` de Visual Studio Installer que no existe → la solución queda marcada como incompleta.
- No hay publicación configurada, ni scripts de despliegue, ni appsettings por entorno.
- `Program.cs` tiene comentado el flujo de `Configuracion_Inicial` que verificaba la cadena al arrancar; hoy arranca directo en `Login`.
- `bin/Debug` no contiene `Servicios.exe` porque la UI no compila.

## 8. Consecuencias para un agente

- Para agregar una funcionalidad: crear `BE/X`, `DAL/DALX`, `BLL/BLLX`, formulario, agregar al `.csproj` correspondiente, agregar claves a ambos JSON y, si aplica, script SQL.
- No agregar capas ni patrones nuevos.
- Replicar el manejo de errores existente (`try/catch` en UI + `MessageBox`), aunque no sea ideal.
- Si se toca seguridad, actualizar DVH/DVV y bitácora en el mismo flujo.
- Si se toca la UI, mantener el patrón `IIdiomaObserver` y las claves de traducción.
