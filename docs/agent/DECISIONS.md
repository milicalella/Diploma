# DECISIONS — Decisiones arquitectónicas observadas

> Registro de decisiones **ya tomadas en el código** (no propuestas). Cada entrada indica evidencia y consecuencias. Las decisiones pendientes están al final.

---

## ADR-001 — WinForms sobre .NET Framework 4.7.2

**Contexto:** aplicación de escritorio de gestión inmobiliaria, probablemente académica.

**Decisión:** usar Windows Forms clásico con .NET Framework 4.7.2.

**Evidencia:** `GUI_577MC.csproj:8-11`, `TargetFrameworkVersion` en los 5 proyectos.

**Consecuencias:**
- Dependencia de Windows y Visual Studio para diseñar formularios.
- Acceso directo a `System.Data.SqlClient` y `System.Web.Extensions`.
- No hay migración a .NET moderno sin reemplazar WinForms y el serializador JSON.

---

## ADR-002 — Capas Form → BLL → DAL → SQL, con DAL que devuelve `DataTable`

**Contexto:** necesidad de separar UI, reglas y datos sin infraestructura adicional.

**Decisión:** cada entidad tiene una clase DAL que ejecuta SQL y devuelve `DataTable`/`DataRow`; la BLL mapea a entidades `BE`.

**Evidencia:** `DALCliente577MC.ObtenerTodos()` devuelve `DataTable`; `BLLCliente577MC.ObtenerTodos()` mapea (`BLLCliente577MC.cs:35-53`).

**Consecuencias:**
- Bajo acoplamiento a un ORM; control total del SQL.
- Mapeo manual repetitivo y propenso a errores si cambian columnas.
- No hay repositorios ni Unit of Work.

**Alternativas descartadas (implícitamente):** Entity Framework, Dapper, NHibernate. No hay evidencia de que se hayan evaluado.

---

## ADR-003 — Sin inyección de dependencias

**Contexto:** aplicación pequeña, un solo desarrollador/equipo.

**Decisión:** cada clase instancia sus dependencias con `new` en un campo.

**Evidencia:** `BLLCliente577MC.cs:16-17` (`DALCliente577MC dal = new ...; BLLBitacora577MC bit = new ...`), `ProgramarVisita.cs:14-15`.

**Consecuencias:**
- Simplicidad y menos archivos.
- Difícil de testear y de sustituir implementaciones.
- Acoplamiento fuerte a clases concretas.

---

## ADR-004 — Permisos con patrón Composite (Rol → Familia → Patente)

**Contexto:** seguridad basada en roles con jerarquía y permisos agrupables.

**Decisión:** modelar roles, familias y patentes como un árbol Composite; el rol se hidrata con todos sus permisos al login.

**Evidencia:**
- `Services/Modelos/CompositeRol/Componente577MC.cs`, `RolModelo577MC`, `FamiliaModelo577MC`, `PermisoModelo577MC`.
- `BLLRol577MC.ObtenerRolesConJerarquia()` (`:37`).
- `ServiceSessionManager577MC.TienePermiso` recorre `Rol.ObtenerPermisos()` (`:49-64`).

**Consecuencias:**
- Permisos reutilizables por familia.
- La UI solo consulta nombres de patente; no hay constantes centralizadas (strings dispersos).
- No se valida autorización fuera del menú (ver KI-05).

---

## ADR-005 — Sesión como singleton

**Contexto:** una sola sesión por proceso de escritorio.

**Decisión:** `ServiceSessionManager577MC` singleton con `getIntancia()`, que guarda `usuarioActivo` y el `IdiomaManager`.

**Evidencia:** `Services/ServiceSessionManager577MC.cs:12-31`.

**Consecuencias:**
- Acceso global desde cualquier capa.
- Estado mutable global; difícil de testear.
- Sin lock (KI-23) y con riesgo de NRE (KI-18).

---

## ADR-006 — Internacionalización con Observer + JSON

**Contexto:** soporte español/inglés sin reiniciar.

**Decisión:** `IdiomaManager` carga un diccionario desde `Lenguajes/{cod}.json` y notifica a los formularios suscritos (`IIdiomaObserver`).

**Evidencia:** `Services/IdiomaManager.cs:12-60`, `Services/Modelos/Idioma/IIdiomaObserver.cs`, `MenuPrincipal.CargarSubItemsIdioma` (`:36`).

**Consecuencias:**
- Agregar un idioma es agregar un JSON.
- Las claves deben existir en ambos archivos (401 cada uno).
- Fuga de observadores (KI-10).
- Textos corruptos por encoding (KI-13).

---

## ADR-007 — Integridad con DVH/DVV

**Contexto:** detectar alteraciones directas en la base.

**Decisión:** cada tabla de negocio tiene `DVH` por fila; `DigitoVerificador` guarda el DVV por tabla y un DVH de control. Al login se verifica; si falla, se repara o se restringe el acceso.

**Evidencia:**
- `Services/DigitoVerificador577MC.cs:24-44,158-194`.
- `DAL/DALDigitoVerificador577MC.cs`.
- `Login.cs:52-76`.

**Consecuencias:**
- Toda escritura debe recalcular DVH/DVV.
- Hay un bug de orden en alta de usuario (KI-04).
- El login de rol 1 deriva a reparación; los demás quedan bloqueados.

---

## ADR-008 — SQL inline parametrizado

**Contexto:** sin ORM ni stored procedures.

**Decisión:** escribir SQL en el DAL y pasar parámetros con `Dictionary<string, object>` + `AddWithValue`.

**Evidencia:** todos los `DAL/*.cs`.

**Consecuencias:**
- Previene inyección SQL en los valores.
- `AddWithValue` puede inferir tipos incorrectos (por ejemplo `varchar` vs `nvarchar`, `decimal` vs `float`).
- No hay plan de consultas reutilizable ni stored procedures.
- Facilita migrar a otro motor, pero ata a sintaxis SQL Server.

---

## ADR-009 — Auditoría desde la BLL

**Contexto:** registrar acciones de negocio.

**Decisión:** las clases BLL llaman a `BLLBitacora577MC.registrarEvento(...)` después de la operación.

**Evidencia:** `BLLCliente577MC.RegistrarCliente` (`:135`), `BLLVisita577MC.ProgramarVisita` (`:80`), `BLLUsuario577MC` (login, alta, bloqueo).

**Consecuencias:**
- La auditoría está acoplada a la lógica de negocio.
- Si la operación principal falla, no se registra (correcto); si la bitácora falla, puede abortar la operación.
- `Modulo` se pasa como enum y se persiste como int en un varchar (KI-09).

---

## ADR-010 — Backup/restore delegado a SQL Server

**Contexto:** respaldar la base completa sin lógica propia.

**Decisión:** ejecutar `BACKUP DATABASE` y `RESTORE DATABASE` desde la app; el restore se hace contra `master`.

**Evidencia:** `DAL/DALBackUpRestore577MC.cs:18-50`.

**Consecuencias:**
- Simple y completo.
- Requiere permisos elevados y rutas accesibles por el servicio SQL.
- Cadena hardcodeada (KI-06) y sin validaciones.

---

## ADR-011 — Sufijo `577MC` en todos los tipos propios

**Contexto:** distinguir el código del equipo en un repositorio compartido.

**Decisión:** todo tipo de negocio lleva el sufijo `577MC`.

**Evidencia:** `Cliente577MC`, `BLLCliente577MC`, `DALCliente577MC`, `ServiceSessionManager577MC`, etc.

**Consecuencias:**
- Consistencia nominal.
- Nombres largos.
- No aplica a formularios ni a `Program`.

---

## ADR-012 — Feature de contratos

**Contexto:** la auditoría inicial encontró `BE/Contrato577MC`, `DAL/DALContrato577MC` y `FrmGenerarContrato` sin BLL ni esquema en scripts.

**Decisión posterior (2026-10-28):** completar la feature con `BLLContrato577MC`, CUN06/CUN07/CUN08, reportes RN1/RN2 y reimpresión, manteniendo la arquitectura `Form → BLL → DAL → DALAcceso577MC → SQL`.

**Evidencia:** solución compilada, pruebas BLL/GUI 34/34 y 29/29, firma con transacción SQL, PDF generados y base restaurada con integridad `True`.

**Consecuencias:**
- La solución compila y la feature es funcional contra la base actual.
- Persiste la deuda de agregar el DDL de `Contrato` a los scripts (KI-02).
- La bitácora de la firma queda fuera de la transacción (recomendación abierta).

---

## ADR-013 — Base de conocimiento en `docs/agent/`

**Contexto:** necesidad de que futuros agentes trabajen sin redescubrir la arquitectura.

**Decisión (de esta auditoría):** crear `AGENT.md` + `docs/agent/*` con hechos verificados, convenciones, riesgos y estado.

**Evidencia:** este conjunto de archivos.

**Consecuencias:**
- Documentación versionable junto al código.
- Debe actualizarse cuando cambie la arquitectura o se resuelvan issues.

---

## ADR-014 — Auditoría sin cambios de código

**Contexto:** primera intervención sobre un repositorio con trabajo en curso.

**Decisión:** solo lectura y documentación; no modificar fuentes, formularios, SQL, configuración ni base.

**Evidencia:** `git status` antes/después; solo se agregaron `AGENT.md` y `docs/agent/`.

**Consecuencias:**
- Base segura para la próxima iteración.
- Los bloqueos quedan documentados y requieren autorización.

---

## ADR-015 — Operabilidad de la propiedad al generar contratos (RF2)

**Contexto:** `C-20261028-0004` quedó `Pendiente` sobre la propiedad 11, que ya estaba `Vendida` por `C-20261028-0002`. La firma se rechaza correctamente (`FirmarContrato` solo acepta `Disponible`/`Reservada`, con revalidación en la transacción del DAL), pero CUN06 permite generar contratos sobre propiedades no operables. El RF2 no contempla ese caso y permitirlo duplicaría contratos firmados sobre una misma propiedad.

**Decisión (2026-10-30):** mantener la regla de firma existente y agregar operabilidad en origen. No se relaja la validación para "poder firmar" contratos heredados.

1. `SeleccionarPropiedad` recibe el flag `soloOperables` (por defecto `false`); en modo selección para contratos muestra solo `Disponible`/`Reservada`. El ABM y el selector de visitas conservan el catálogo completo.
2. `FrmGenerarContrato` abre el selector con ese flag y revalida al seleccionar y antes de generar.
3. `BLLContrato577MC.ValidarOperabilidadPropiedad` relee la propiedad y rechaza la generación si no es operable, informando código, dirección y estado.
4. `FirmarContrato` mejora el mensaje de rechazo con `NroContrato`, propiedad, dirección y estado, y acepta `Disponible`/`Reservada` mediante `EsEstadoOperable`.
5. Sin cambios de esquema ni de DVH/DVV.
6. **Agregado 2026-11-02:** también se rechaza la generación si la propiedad ya tiene otro contrato `Pendiente` (`DALContrato577MC.ObtenerContratoPendiente`), para no dejar dos borradores activos sobre la misma propiedad.

**Evidencia:** lectura de `SeleccionarPropiedad.cs:74`, `BLLContrato577MC.cs:121-126`, `DALContrato577MC.cs:162-167` y de la base al 2026-10-30 (5 contratos, propiedad 11 `Vendida`, `C-20261028-0004` `Pendiente`).

**Consecuencias:**
- `C-20261028-0004` queda `Pendiente` y no se puede firmar mientras la propiedad 11 siga `Vendida`; es una decisión del usuario, no un defecto a corregir por datos.
- El gap del RF2 sobre **Matrícula Dominial** queda separado: requiere cambio de esquema y de DVH, y no forma parte de esta decisión.
- **Estado: implementado el 2026-11-02** (KI-25 resuelto). Build Debug sin errores; solo `CS2002`.

---

# Decisiones pendientes (no tomadas)

| ID | Pregunta | Opciones | Impacto |
|---|---|---|---|
| PD-01 | ~~¿Se completa la feature de contratos?~~ | **Resuelto:** implementada CUN06/CUN07/CUN08 + reportes | — |
| PD-02 | Esquema de `Contrato` | **Resuelto en la base**; falta agregar el DDL a los scripts | instalación desde cero |
| PD-03 | ¿Se corrige `scriptTerceraEntrega.sql`? | a) sí, corregir DDL; b) generar un dump nuevo desde una BD sana | instalación desde cero |
| PD-04 | Instancia oficial de backup/restore | a) `.\SQLEXPRESS` de `App.config`; b) mantener `LAPTOP-8BNKG482` | portabilidad |
| PD-05 | Autorización en formularios | a) validar en cada form; b) validar en BLL; c) dejar solo menú | seguridad |
| PD-06 | Normalización de encoding | a) commit dedicado UTF-8; b) dejar como está | textos y claves |
| PD-07 | Tipo de `BitacoraEventos.Modulo` | a) `int` + tabla; b) `varchar` con nombres | integridad de auditoría |
| PD-08 | Transacciones | a) agregar helper; b) mantener operaciones sueltas | consistencia |
| PD-09 | Hash de contraseñas | a) migrar a PBKDF2/bcrypt; b) mantener SHA-256 | seguridad |
| PD-10 | Proyecto de despliegue | a) eliminar referencia; b) recrear `.vdproj` | instalador |
| PD-11 | ~~¿Se filtra la generación de contratos por estado de propiedad?~~ | **Resuelto (ADR-015):** flag `soloOperables` + validación en BLL, implementado el 2026-11-02 | integridad de contratos |
| PD-12 | Matrícula Dominial en `Propiedad` | a) agregar columna + DVH; b) dejar el RF2 sin ese campo | esquema y DVH |
