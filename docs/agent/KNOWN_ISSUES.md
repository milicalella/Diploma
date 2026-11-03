# KNOWN_ISSUES — Defectos y riesgos verificados

> Estados: **[VERIFICADO]** leído en código/script o reproducido con build; **[INFERIDO]** deducido del código sin ejecutar; **[HIPÓTESIS]** requiere confirmación humana; **[RECOMENDACIÓN]** propuesta no aplicada. Salvo indicación contraria, los hechos descriptivos de este documento fueron verificados por lectura directa.

## Resumen por severidad

| ID | Severidad | Título | Estado |
|---|---|---|---|
| KI-01 | Bloqueante | La solución no compila: falta `BLLContrato577MC` | **RESUELTO** (2026-10-28) |
| KI-02 | Bloqueante | La tabla `Contrato` no existe en los scripts | **ABIERTO** (existe en la base, falta script) |
| KI-03 | Crítico | El script de creación de BD tiene DDL inválido en `Visita` | VERIFICADO |
| KI-04 | Alto | DVV de usuario se actualiza antes del INSERT | VERIFICADO |
| KI-05 | Alto | Autorización solo en el menú, no en formularios/BLL | VERIFICADO |
| KI-06 | Alto | Restore/backup usa instancia y base hardcodeadas | VERIFICADO |
| KI-07 | Alto | Hash SHA-256 sin sal y contraseña default predecible | VERIFICADO |
| KI-08 | Alto | El log DAL registra valores sensibles | VERIFICADO |
| KI-09 | Medio | `BitacoraEventos.Modulo` es varchar pero el código usa int | VERIFICADO |
| KI-10 | Medio | Fuga de observadores de idioma | VERIFICADO |
| KI-11 | Medio | Sin transacciones en operaciones multi-tabla | PARCIAL: firma de contrato resuelta con transacción SQL |
| KI-12 | Medio | Condición de carrera en disponibilidad de visitas | VERIFICADO (código) |
| KI-13 | Medio | Textos y nombres con encoding corrupto | VERIFICADO |
| KI-14 | Medio | Ruta absoluta del logo en el PDF | **RESUELTO** (2026-10-28) |
| KI-15 | Medio | Índices únicos duplicados de Email en `Usuario` | VERIFICADO |
| KI-16 | Medio | Proyecto de despliegue inexistente en la solución | VERIFICADO |
| KI-17 | Bajo | Namespace `Services_577MC` dentro del proyecto `Services` | VERIFICADO |
| KI-18 | Bajo | `TienePermiso` sin sesión lanza `NullReferenceException` | VERIFICADO (código) |
| KI-19 | Bajo | `ConsultarDisponibilidad` y `FrmGenerarContrato` untracked | VERIFICADO |
| KI-20 | Bajo | `FrmGenerarContrato` no genera número de contrato ni valida DNI | **RESUELTO** (2026-10-28) |
| KI-21 | Bajo | `Configuracion_Inicial` no está activo en `Program.cs` | VERIFICADO |
| KI-22 | Bajo | `scriptTerceraEntrega.sql` se copia a la salida y se ejecuta desde la app | VERIFICADO |
| KI-23 | Bajo | `ServiceSessionManager` sin lock | VERIFICADO (código) |
| KI-24 | Bajo | `GestionRespaldo` reinicia la app tras restore | VERIFICADO |
| KI-25 | Medio | Se pueden generar contratos sobre propiedades no operables | **RESUELTO** (2026-11-02) |

---

## KI-01 — La solución no compila (RESUELTO)

**Estado:** resuelto el 2026-10-28 con la implementación de `BLL/BLLContrato577MC.cs` (CUN06) y su inclusión en `BLL_577MC.csproj`.

**Evidencia actual:** `Servicios.sln` compila en Debug y genera `bin\Debug\Servicios.exe`. Solo quedan warnings `CS2002` preexistentes por `AssemblyAttributes.cs` duplicados.

---

## KI-02 — La tabla `Contrato` no existe en los scripts (ABIERTO)

**Evidencia**

- `DAL/DALContrato577MC.cs` consulta/inserta en `Contrato`.
- La tabla **sí existe en la base** `is--servicios` y fue verificada contra el DAL/BLL (columnas, FKs y DVH).
- `Scripts/scriptTerceraEntrega.sql` no crea `Contrato`; si se recrea la base desde cero, la feature falla con error 208.

**Impacto:** la aplicación funciona contra la base actual, pero no se puede instalar desde cero solo con los scripts.

**Recomendación:** agregar un script idempotente con el DDL verificado de `Contrato` (PK `NroContrato`, FKs a `Cliente`/`Propiedad`, `Estado`, `DVH`). No inventar columnas.

---

## KI-03 — DDL inválido de `Visita`

**Evidencia** (`Scripts/scriptTerceraEntrega.sql:282-293`):

```sql
CREATE TABLE [dbo].[Visita](
    [IdVisita] [int] IDENTITY(1,1) NOT NULL,
    ...
    [Estado] [varchar](15) NOT NULL
    [Observaciones] [varchar](500) NULL,,
    [DVH] [bigint] NULL,
```

- Falta la coma después de `NOT NULL` en `Estado`.
- `NULL,,` tiene una coma doble.
- El script se ejecuta desde `Configuracion_Inicial` / `ServicioConfiguracionConexion.EjecutarScriptCreacion` (`Services/Instalador/ServicioConfiguracionConexion.cs:128`), aunque ese flujo está comentado en `Program.cs`.

**Impacto:** el script falla al crear la base; no se puede inicializar el sistema desde cero.

**Recomendación:** corregir a `[Estado] [varchar](15) NOT NULL,` y `[Observaciones] [varchar](500) NULL,`. Validar el resto del script en una instancia limpia. Cambio de script requiere autorización.

---

## KI-04 — DVV actualizado antes del INSERT de usuario

**Evidencia** (`BLL/BLLUsuario577MC.cs:144-146`):

```csharp
Services.DigitoVerificador577MC.ActualizarDVVUsuario();

dal.InsertarUsuario(dni, nombre, apellido, email, rol.Id, user, passwordHash, dvh);
```

El DVV se calcula con los usuarios existentes **antes** de insertar el nuevo. El DVH de la fila nueva no se incluye en el DVV.

**Impacto:** tras crear un usuario, `VerificarIntegridad()` detecta inconsistencia en `Usuario`; el próximo login puede quedar bloqueado (salvo rol 1) y derivar a reparación.

**Recomendación:** mover `ActualizarDVVUsuario()` después de `InsertarUsuario` y verificar el resultado. Aplicar el mismo patrón en altas/bajas de Rol, Familia y Patente (revisar `BLLRol577MC`/`BLLFamilia577MC`).

---

## KI-05 — Autorización solo en el menú

**Evidencia**

- `MenuPrincipal.configurarAcceso()` (`MenuPrincipal.cs:75-95`) es el único lugar que evalúa `TienePermiso`.
- Los formularios no reciben ni validan permisos.
- `ProgramarVisita` abre `RegistrarCliente` y `SeleccionarPropiedad` de forma modal (`ProgramarVisita.cs:87-105`, `128-140`) sin chequear `Gestion Clientes` ni `Seleccionar Propiedad`.
- `FrmGenerarContrato` abre ambos formularios también sin chequeo.

**Impacto:** un usuario sin permiso puede ejecutar acciones si llega al formulario por otra vía (otro formulario, código, automatización).

**Recomendación:** validar en el punto de entrada de cada formulario o en la BLL. No confiar solo en `Enabled = false`.

---

## KI-06 — Instancia y base hardcodeadas en backup/restore

**Evidencia** (`DAL/DALBackUpRestore577MC.cs:13-15`):

```csharp
private string _nombreDB = "is--servicios";
private readonly string _stringConnectionOriginal =
    "Data Source=LAPTOP-8BNKG482\\SQLEXPRESS;Initial Catalog=is--servicios;Integrated Security=True";
```

- No usa `ConexionPrincipal` de `App.config`.
- El restore conecta a `master` con esa cadena (líneas 32-50).
- El path del backup lo elige el usuario en su máquina, pero SQL Server lo interpreta en **su** servidor.

**Impacto:** backup/restore falla en cualquier máquina que no sea `LAPTOP-8BNKG482`; con SQL remoto, la ruta local no existe para el servicio.

**Recomendación:** leer la cadena de configuración y derivar servidor/base; documentar que la ruta debe ser accesible por la cuenta del servicio SQL.

---

## KI-07 — Hash sin sal y contraseña default predecible

**Evidencia**

- `Services/ServiceSeguridad577MC.cs:12`: SHA-256 de la cadena, sin sal ni iteraciones.
- `BLLUsuario577MC.GenerarPassword(apellido, dni)` (línea 340) y `GenerarUsuario(nombre, dni)` (línea 335).
- La contraseña default se detecta comparando el hash (`BLLUsuario577MC.login`, líneas 105-112).

**Impacto:** hashes vulnerables a rainbow tables; credenciales iniciales adivinables con datos públicos (nombre/apellido/DNI).

**Recomendación:** no cambiar el algoritmo sin plan de migración (rompe hashes existentes). Si se hace, usar PBKDF2/bcrypt con sal y rehash al login. Forzar cambio de la default ya existe, pero no valida complejidad.

---

## KI-08 — El log DAL registra valores sensibles

**Evidencia** (`DAL/DALAcceso577MC.cs:183-191`):

```csharp
foreach (var p in parametros)
{
    object valor = p.Value ?? "<NULL>";
    sb.AppendLine($"  {p.Key} = [{valor.GetType().Name}] {valor}");
}
```

Se escriben DNI, emails, hashes de contraseña y cualquier parámetro en `%LOCALAPPDATA%\577MC\Logs\ErroresDAL.log`.

**Impacto:** exposición de datos personales y hashes en disco.

**Recomendación:** enmascarar parámetros sensibles (`@pass`, `@password`, `@dvh` no son sensibles, pero `@pass` sí) o loguear solo nombres y tipos. No cambiar el formato sin revisar soporte.

---

## KI-09 — `BitacoraEventos.Modulo` varchar vs int

**Evidencia**

- DDL: `[Modulo] [varchar](50) NOT NULL` (`scriptTerceraEntrega.sql:92`).
- `DALBitacora577MC.insertarLog(..., int modulo, ...)` (línea 19) pasa un `int`.
- `DALBitacora577MC.obtenerBitacora(..., int? moduloId)` compara `B.Modulo = @modulo` (línea 56).
- Seeds: `N'1'`, `N'2'`, etc. (por ejemplo línea 304).

**Impacto:** funciona por conversión implícita mientras todos los valores sean numéricos. Si se inserta un texto (o se consulta con un valor no numérico), falla o no filtra.

**Recomendación:** decidir si `Modulo` debe ser `int` (FK a una tabla de módulos) o `varchar` con nombres. Si se mantiene varchar, el DAL debe enviar `string` y el modelo alinear el tipo.

---

## KI-10 — Observadores de idioma no desuscriptos

**Evidencia**

- `IdiomaManager` guarda referencias fuertes en `_observers` (`Services/IdiomaManager.cs:14-25`).
- Solo `SeleccionarPropiedad` (`:46`) y `FrmGenerarContrato` (`:28`) se desuscriben en `FormClosed`.
- El resto (Login, MenuPrincipal, RegistrarCliente, ProgramarVisita, etc.) no lo hace.

**Impacto:** cada formulario abierto/cerrado queda referenciado; al cambiar idioma se notifica a formularios disposed (puede lanzar `ObjectDisposedException`).

**Recomendación:** agregar `FormClosed += (s,e) => ...Desuscribir(this);` en todos los formularios con `IIdiomaObserver`, o usar referencias débiles. Aplicar de a uno, compilando.

---

## KI-11 — Sin transacciones

**Evidencia**

- No hay `SqlTransaction` en `DAL/`.
- Operaciones como `BLLRol577MC.crearRol` insertan el rol y luego asignan patentes/familias en llamadas separadas.
- `DigitoVerificador577MC.RepararUsuario` actualiza fila por fila y luego el DVV.
- `DALBackUpRestore` ejecuta `ALTER DATABASE` + `RESTORE` en un solo comando, pero no es una transacción de negocio.

**Impacto:** fallas parciales dejan datos inconsistentes (rol sin permisos, DVV desincronizado).

**Recomendación:** agregar un método transaccional en `DALAcceso577MC` (por ejemplo `executeInTransaction(Action<SqlConnection, SqlTransaction>)`) solo si el usuario aprueba un cambio de infraestructura. Mientras tanto, ordenar las operaciones y reparar ante error.

---

## KI-12 — Carrera en disponibilidad de visitas

**Evidencia**

- `BLLVisita577MC.ProgramarVisita` consulta superposición (`ObtenerVisitasPorPropiedadYFecha` + `HaySuperposicion`) y luego inserta (`BLLVisita577MC.cs:62-81`).
- No hay constraint de exclusión ni transacción `SERIALIZABLE`.

**Impacto:** dos usuarios pueden agendar la misma propiedad/horario si confirman a la vez.

**Recomendación:** validar en el servidor con una transacción y bloqueo, o un constraint/trigger. No implementar sin decisión de esquema.

---

## KI-13 — Encoding corrupto

**Evidencia**

- Literales con `A3`, `A±`, `A?`, `A�` en lugar de `ó`, `ñ`, `í`.
- Ejemplos: `BLLCliente577MC.cs` (`modificA3`, `eliminA3`, `registrA3`), `FrmGenerarContrato.cs` (`A�Desea`, `AA�n`, `vA�lida`, `depA3sito`), `CambiarContraseña.cs` (`CambiarContraseA�a`), `Scripts/scriptOptimizacionesRendimiento.sql` (`A-ndices`).
- Los JSON `es.json`/`en.json` también contienen caracteres corruptos en los valores (`Contrase�a`).

**Impacto:** textos incorrectos en la UI y en la bitácora; claves de traducción que pueden no coincidir si se reescribe el archivo con otra codificación.

**Recomendación:** normalizar a UTF-8 con BOM o sin BOM de forma consistente, corregir literales y claves, y verificar que las claves (459 al 2026-10-30) sigan coincidiendo en ambos JSON. Hacerlo en un commit dedicado.

---

## KI-14 — Ruta absoluta del logo (RESUELTO)

**Estado:** resuelto el 2026-10-28.

**Solución aplicada**

- Logo nuevo: `Recursos\logo-propiedades-calella.jpg` (1020×323 px).
- `GUI_577MC.csproj` lo incluye como `Content` con `CopyToOutputDirectory = Always`.
- `ReportePdf577MC.ObtenerRutaLogo()` lo resuelve desde `AppDomain.CurrentDomain.BaseDirectory\Recursos` y usa `..\..\Recursos` como fallback.
- El logo se dibuja en el encabezado rediseñado (franja azul + doble regla), escalado a un máximo de 150×48 pt.
- Si el archivo no existe o falla la lectura, `DibujarLogo` no rompe el reporte: se genera sin logo.

**Evidencia:** RN1 y RN2 rediseñados, generados y revisados visualmente con el logo en el encabezado.

---

## KI-15 — Índices únicos duplicados de Email

**Evidencia**

- `scriptTerceraEntrega.sql:676-696` agrega **tres veces** `UNIQUE NONCLUSTERED (Email)` sobre `Usuario`.
- `scriptOptimizacionesRendimiento.sql` lo documenta y provee una consulta de diagnóstico, sin eliminar los duplicados.

**Impacto:** sobrecarga de escritura y confusión; no cambia la semántica (los tres son redundantes).

**Recomendación:** conservar uno y eliminar los otros dos en un script idempotente. Verificar nombres reales en la base antes de hacer `DROP INDEX`.

---

## KI-16 — Proyecto de despliegue inexistente

**Evidencia**

- `Servicios.sln` referencia `..\Sistema_IngSoftware\Sistema_IngSoftware.vdproj`.
- El archivo no existe en el repositorio ni en el directorio padre.

**Impacto:** no se puede generar el instalador; la solución muestra un proyecto faltante.

**Recomendación:** eliminar la referencia o agregar el `.vdproj`. Requiere decisión del usuario.

---

## KI-17 — Namespace `Services_577MC`

**Evidencia**

- `Services/ServiceSessionManager577MC.cs:10`: `namespace Services_577MC`.
- El resto de `Services/` usa `namespace Services`.
- BLL importa ambos.

**Impacto:** confusión y acoplamiento; un `using Services` no trae `ServiceSessionManager577MC`.

**Recomendación:** unificar a `Services` en un cambio mecánico, actualizando todos los `using`. No hacerlo junto con cambios funcionales.

---

## KI-18 — `TienePermiso` sin sesión

**Evidencia** (`Services/ServiceSessionManager577MC.cs:49-64`):

```csharp
List<Componente577MC> todosLosPermisos = usuarioActivo.Rol.ObtenerPermisos();
```

No valida `usuarioActivo`/`Rol` nulos.

**Impacto:** `NullReferenceException` si se llama antes del login o después del logout.

**Recomendación:** devolver `false` si no hay sesión. Cambio pequeño y seguro.

---

## KI-19 — Archivos untracked

**Evidencia** (`git status`):

- Nuevos: CUN06/CUN07 (`BE/Contrato577MC.cs`, `BE/TerminosContrato577MC.cs`, `DAL/DALContrato577MC.cs`, `BLL/BLLContrato577MC.cs`, `FrmGenerarContrato.*`, `FrmRegistrarTerminosCondiciones.*`), CUN08 (`FrmActualizarEstadoPropiedadContrato.*`), reportes (`ReportePdf577MC.cs`, `ReporteRN2577MC.cs`, `FrmReimprimirContrato.*`), `Recursos/logo-propiedades-calella.jpg`, `ConsultarDisponibilidad.cs/.Designer.cs`, `AGENT.md` y `docs/`.
- Eliminados: `FrmConsultarDisponibilidad.cs/.Designer.cs`, logo viejo `Recursos/6295da40-...jpg`.
- Modificados: `.csproj`, `BLLCliente577MC.cs`, `BLLPropiedad577MC.cs`, `DALPropiedad577MC.cs`, `Services/DigitoVerificador577MC.cs`, `MenuPrincipal.*`, `FrmActualizarEstadoPropiedadContrato.cs`, `ReporteRN1577MC.cs`, `Configuracion Inicial.Designer.cs`, `ProgramarVisita.cs`, JSON.

**Impacto:** trabajo en curso sin commit; un agente podría sobrescribirlo.

**Recomendación:** no tocar esos archivos sin confirmación. Antes de cualquier cambio, `git diff` y `git status`.

---

## KI-20 — `FrmGenerarContrato` incompleto (RESUELTO)

**Estado:** resuelto el 2026-10-28.

**Evidencia actual**

- `BLLContrato577MC.ValidarYGenerarContrato` valida DNI/propiedad/términos, asigna `NroContrato` con formato `C-YYYYMMDD-NNNN` y reintenta ante duplicados.
- CUN07 centraliza la validación de términos (`ValidarTerminos`) y CUN06 los muestra en solo lectura.
- Los DNI se validan por existencia en `Cliente`; el formato sigue a cargo de los formularios.
- La generación fue probada end-to-end contra la base real y el contrato quedó con DVH/DVV y bitácora consistentes.

**Pendiente menor:** validar manualmente el escenario “DNI inexistente → alta → continuar”.

---

## KI-21 — Configuración inicial inactiva

**Evidencia** (`Program.cs:20-41`): `Application.Run(new Login())` y todo el bloque de `ServicioConfiguracionConexion`/`Configuracion_Inicial` comentado.

**Impacto:** si la cadena de `App.config` es incorrecta o la base no existe, el login falla sin asistente.

**Recomendación:** decidir si se reactiva el asistente. Si se reactiva, corregir antes el script (KI-03).

---

## KI-22 — Script copiado a la salida

**Evidencia** (`GUI_577MC.csproj:151-153`): `Scripts\scriptTerceraEntrega.sql` con `CopyToOutputDirectory=PreserveNewest`.

**Impacto:** el instalador/app puede ejecutar un script con DDL inválido. Además, el script queda visible/modificable en la carpeta de instalación.

**Recomendación:** mantenerlo solo si el flujo de instalación lo usa; corregir el DDL y considerar embeberlo como recurso.

---

## KI-23 — Singleton sin lock

**Evidencia** (`Services/ServiceSessionManager577MC.cs:23-31`): `getIntancia()` no usa `lock` ni `Lazy<T>`.

**Impacto:** en una app de un solo hilo de UI es bajo; si se agregan tareas en background, puede crear dos instancias.

**Recomendación:** no es prioritario; si se toca, usar `Lazy<T>`.

---

## KI-24 — Reinicio tras restore

**Evidencia** (`GestionRespaldo.cs`, `btnRealizarRestore_Click`): `Application.Restart()` después del restore.

**Impacto:** si el restore deja la base en estado inconsistente, la app puede reiniciar y fallar en bucle; se pierde el contexto.

**Recomendación:** mostrar resultado y pedir cierre manual, o validar la base antes de reiniciar.

---

## KI-25 — Contratos generables sobre propiedades no operables

**Estado:** resuelto el 2026-11-02 (ADR-015 implementado).

**Evidencia del defecto (histórica):**
- `SeleccionarPropiedad` en modo selección cargaba todas las propiedades con `ObtenerTodas()`.
- `BLLContrato577MC.ValidarContrato` no validaba el estado de la propiedad antes del INSERT.
- La firma sí lo validaba: solo acepta `Disponible` o `Reservada`, y el DAL lo revalida en la transacción.

**Impacto:** se podía generar un contrato `Pendiente` sobre una propiedad `Vendida`/`Alquilada`; al intentar firmarlo, la BLL lo rechazaba y el contrato quedaba inutilizable. Caso real: `C-20261028-0004` (Venta, `Pendiente`) sobre la propiedad 11, que está `Vendida` por `C-20261028-0002`.

**Solución implementada:**
- `SeleccionarPropiedad(bool modoAbm = false, bool soloOperables = false)`: con `soloOperables = true` solo muestra y permite elegir `Disponible`/`Reservada`; el ABM y las visitas conservan el catálogo completo.
- `FrmGenerarContrato` abre el selector con `soloOperables: true` y revalida al seleccionar y antes de generar.
- `BLLContrato577MC.ValidarOperabilidadPropiedad(int)` relee la propiedad, rechaza estados no operables y bloquea propiedades con otro contrato `Pendiente` (`DALContrato577MC.ObtenerContratoPendiente`); se invoca desde `ValidarContrato`.
- `FirmarContrato` acepta `Disponible`/`Reservada` (`EsEstadoOperable`) y el mensaje incluye contrato, propiedad, dirección y estado.
- Sin cambios de esquema ni de DVH/DVV.

**Datos:** `C-20261028-0004` queda `Pendiente` por decisión del usuario; no bloquea otras propiedades ni la reimpresión.
