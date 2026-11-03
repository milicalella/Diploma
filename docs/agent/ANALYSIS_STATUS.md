# ANALYSIS_STATUS — Estado del análisis

> **Actualización 2026-10-28 (post-auditoría).** Este documento conserva la auditoría inicial. Desde entonces se implementaron y verificaron CUN06, CUN07, CUN08, los reportes RN1/RN2 y la reimpresión de copia final. La solución **compila** (`bin\Debug\Servicios.exe`), la tabla `Contrato` existe en la base y se probaron los flujos contra SQL Server. Para el estado vigente, la fuente principal es `AGENT.md`; las secciones 3 a 9 de este archivo describen el estado histórico de la auditoría.
>
> **Actualización 2026-10-30.** Build Debug repetido (genera `Servicios.exe`, solo `CS2002`) y estado de datos verificado por lectura: 5 contratos (`0001`/`0002`/`0005` `Firmado`, `0003` `Cancelado`, `0004` `Pendiente`), propiedades 9 y 14 `Alquilada`, 11 `Vendida`, 10 `Disponible`, `MaxBit = 648` y `DVV Contrato = -643719723159986574` (coincide con la suma de los `DVH`). **Pendiente acordado:** regla de operabilidad RF2 (KI-25, ADR-015) y gap de Matrícula Dominial; ver `AGENT.md`, sección 12. No se modificaron datos.
>
> **Actualización 2026-11-02.** Implementada la regla de operabilidad RF2 (KI-25/ADR-015): no se generan contratos sobre propiedades `Vendida`/`Alquilada` ni sobre una propiedad con otro contrato `Pendiente`. Build Debug correcto (solo `CS2002`). Sin cambios de esquema ni de datos.

## 1. Alcance

Auditoría **read-only inicial** del repositorio `Proyecto` (sistema Servicios / GUI_577MC), realizada el 2026-10-28 antes de las implementaciones de contratos y reportes.

**Objetivo:** construir una base de conocimiento persistente para agentes, sin modificar código, formularios, consultas, configuración ni base de datos.

**Entregables:** `AGENT.md`, `docs/agent/PROJECT_OVERVIEW.md`, `ARCHITECTURE.md`, `PROJECT_MAP.md`, `CODING_CONVENTIONS.md`, `WINFORMS_PATTERNS.md`, `DAL_PATTERNS.md`, `DEPENDENCIES.md`, `KNOWN_ISSUES.md`, `DECISIONS.md`, `ANALYSIS_STATUS.md`.

## 2. Método

1. Inventario de archivos con `Get-ChildItem` excluyendo `obj`, `bin`, `packages`.
2. Lectura de la solución, los 5 `.csproj`, `App.config`, `Program.cs`.
3. Lectura de todos los `.cs` de BE, DAL, BLL, Services y UI (incluidos `.Designer.cs` cuando aportaban al flujo).
4. Lectura de los 10 scripts SQL, en especial `scriptTerceraEntrega.sql` (765 líneas).
5. Verificación de referencias entre proyectos y de archivos incluidos en cada `.csproj`.
6. Conteo de claves de traducción en `es.json`/`en.json` (401 cada uno).
7. Compilación con MSBuild 2022 para confirmar el estado.
8. `git status` y `git log` para identificar trabajo en curso.
9. Búsqueda de patrones de riesgo: `Suscribir`/`Desuscribir`, `SqlTransaction`, rutas absolutas, `AddWithValue`, `Password`, `DVV`.

## 3. Verificaciones ejecutadas

### 3.1 Build

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" `
  Servicios.sln /t:Build /p:Configuration=Debug /v:minimal /nologo
```

Resultado de la auditoría inicial (histórico):

```
BE_577MC -> ...\BE\bin\Debug\BE.dll
DAL_577MC -> ...\DAL\bin\Debug\DAL.dll
Services_577MC -> ...\Services\bin\Debug\Services.dll
BLL_577MC -> ...\BLL\bin\Debug\BLL.dll
FrmGenerarContrato.cs(20,9): error CS0246: El nombre del tipo o del espacio de nombres
'BLLContrato577MC' no se encontró ...
```

- Código de salida: 1.
- `bin\Debug` de la raíz no tenía `Servicios.exe`.

**Estado actual (2026-10-28, post-implementación):** compila completo y genera `bin\Debug\Servicios.exe`; solo quedan warnings `CS2002` preexistentes. [VERIFICADO]

### 3.2 Git

- Rama actual con cambios sin commit.
- Último commit: `474f59a Ultimo primer entrega`.
- Archivos untracked y modificados listados en `KNOWN_ISSUES.md` (KI-19).

### 3.3 Conteo de traducciones

- `Lenguajes/es.json`: 401 claves al momento de la auditoría; **459** al 2026-10-28 tras agregar reportes y su rediseño.
- `Lenguajes/en.json`: 401 claves al momento de la auditoría; **459** al 2026-10-28 (mismo conjunto de claves que `es.json`).
- No se validó que las claves coincidan una a una (solo el total). [VERIFICADO parcial]

### 3.4 SQL

- `scriptTerceraEntrega.sql` crea 14 tablas; no crea `Contrato`.
- `Visita` tiene DDL inválido (KI-03).
- `Usuario` tiene 3 índices únicos de Email duplicados (KI-15).
- `BitacoraEventos.Modulo` es `varchar(50)` con seeds numéricos (KI-09).

## 4. Cobertura

| Área | Cobertura | Nota |
|---|---|---|
| Estructura de solución y proyectos | 100% | 5 `.csproj` + `.sln` |
| BE | 100% | 6 clases + 2 enums |
| DAL | 100% | 12 clases |
| BLL | 100% | 9 clases |
| Services | 100% | 12 archivos |
| UI | 100% de los `.cs` | 19 formularios + `Program` + 3 clases de reporte |
| SQL | 100% de los scripts | 10 archivos |
| JSON | 100% (conteo) | 459 claves en `es.json` y `en.json`; conjuntos comparados, 0 diferencias |
| Configuración | 100% | `App.config`, `.csproj` |
| Ejecución de la app | 100% | compila y se instanciaron formularios en pruebas |
| Conexión a SQL Server | 100% (pruebas) | CUN06/CUN07/CUN08 y reportes contra `is--servicios` |
| Pruebas funcionales | Parcial | harness PowerShell BLL/GUI; no hay proyecto de test |
| Rendimiento | 0% | solo lectura estática |

## 5. Límites y supuestos

- **Límites de la auditoría inicial:** no se ejecutó la aplicación (la UI no compilaba), no se conectó a SQL Server y el esquema se infería de `scriptTerceraEntrega.sql`.
- **Posteriormente (2026-10-28)** se compiló, se conectó a `is--servicios`, se validó la tabla `Contrato` y se probaron CUN06/CUN07/CUN08, RN1/RN2, reimpresión e integridad.
- **No se validó** que toda la base existente coincida con el dump (puede tener cambios manuales, scripts aplicados o columnas distintas).
- **No se probaron** login, backup/restore ni todos los ABMs; sí se probaron los flujos de contratos, reportes e integridad.
- **No se compararon** las 401 claves de traducción entre español e inglés en la auditoría inicial; sí se compararon las **459** posteriores (0 diferencias).
- **No se verificó** el contenido del `.vdproj` (no existe).
- **No se auditó** la seguridad de la instancia SQL ni permisos de usuarios de base.
- Las rutas de archivos y líneas corresponden al estado del 2026-10-28; pueden cambiar con el trabajo en curso.

## 6. Hallazgos principales (resumen)

1. ~~**Build bloqueado** por `BLLContrato577MC` inexistente.~~ **Resuelto:** compila y genera `Servicios.exe`.
2. ~~**Feature de contratos** incompleta: falta BLL, tabla y numeración.~~ **Resuelto funcionalmente:** CUN06/CUN07/CUN08 y reportes implementados; la tabla existe en la base.
3. **Script de creación de BD inválido** en `Visita`; no se puede inicializar desde cero.
4. **DVV de usuario** se actualiza antes del INSERT.
5. **Autorización** solo en el menú.
6. **Backup/restore** con instancia hardcodeada.
7. **Seguridad de contraseñas** débil (SHA-256 sin sal, default predecible).
8. **Log DAL** con valores sensibles.
9. **Encoding corrupto** en textos y nombres.
10. **Sin transacciones** y con carrera en disponibilidad de visitas.

El detalle y las recomendaciones están en `KNOWN_ISSUES.md`.

## 7. Trabajo en curso detectado

Archivos que **no deben modificarse sin autorización**:

- Implementados: CUN06/CUN07/CUN08 (`BE/Contrato577MC.cs`, `BE/TerminosContrato577MC.cs`, `DAL/DALContrato577MC.cs`, `BLL/BLLContrato577MC.cs`, `FrmGenerarContrato.*`, `FrmRegistrarTerminosCondiciones.*`, `FrmActualizarEstadoPropiedadContrato.*`), reportes (`ReportePdf577MC.cs`, `ReporteRN1577MC.cs`, `ReporteRN2577MC.cs`, `FrmReimprimirContrato.*`).
- En curso: `ConsultarDisponibilidad.cs/.Designer.cs`.
- Eliminados: `FrmConsultarDisponibilidad.cs/.Designer.cs`, logo viejo.
- Modificados: `.csproj`, `BLLCliente577MC.cs`, `BLLPropiedad577MC.cs`, `DALPropiedad577MC.cs`, `Services/DigitoVerificador577MC.cs`, `MenuPrincipal.*`, `Configuracion Inicial.Designer.cs`, `ProgramarVisita.cs`, `Lenguajes/en.json`, `Lenguajes/es.json`.

## 8. Preguntas abiertas

1. ~~¿Se implementa la feature de contratos o se excluye temporalmente del build?~~ **Resuelto:** implementada (CUN06/CUN07/CUN08).
2. ~~¿Qué esquema y reglas debe tener `Contrato`?~~ **Resuelto en la base**, pero falta el DDL en los scripts.
3. ¿Se corrige `scriptTerceraEntrega.sql` o se genera un dump nuevo desde una base sana?
4. ¿La instancia `LAPTOP-8BNKG482\SQLEXPRESS` es la oficial para backup/restore?
5. ¿La autorización debe validarse en cada formulario, en la BLL, o alcanza con el menú?
6. ¿Se normaliza el encoding en un commit dedicado?
7. ¿Se cambia el tipo de `BitacoraEventos.Modulo`?
8. ¿Se agregan transacciones y un constraint para visitas?
9. ¿Se migra el hash de contraseñas?
10. ¿Se elimina la referencia al `.vdproj` o se recrea el instalador?
11. ~~¿Se restringe la generación de contratos a propiedades operables?~~ **Resuelto (ADR-015) e implementado (2026-11-02):** flag `soloOperables` + validación en BLL.
12. ¿Se agrega `Matrícula Dominial` a `Propiedad` (cambio de esquema y DVH)?

## 9. Próximos pasos sugeridos (requieren autorización)

| Prioridad | Acción | Por qué |
|---|---|---|
| 1 | ~~Decidir contratos (PD-01/PD-02)~~ | **hecho**: CUN06/CUN07/CUN08 y reportes implementados |
| 2 | ~~Implementar la regla de operabilidad RF2 (KI-25/ADR-015)~~ | **hecho (2026-11-02)**: no se generan contratos sobre propiedades no operables ni duplicados `Pendiente` |
| 3 | Agregar DDL de `Contrato` y corregir `Visita` (KI-02/KI-03) | permite instalar desde cero |
| 4 | Corregir orden de DVV en `CrearUsuario` (KI-04) | evita bloqueos de login |
| 5 | Validar autorización en formularios/BLL (KI-05) | seguridad |
| 6 | Externalizar instancia de backup (KI-06); logo ya resuelto (KI-14) | portabilidad |
| 7 | Revisar log de parámetros (KI-08) | privacidad |
| 8 | Normalizar encoding (KI-13) | calidad de UI |
| 9 | Desuscribir observadores (KI-10) | estabilidad |
| 10 | Agregar transacciones/constraint (KI-11/12) | consistencia |
| 11 | Actualizar esta base de conocimiento tras cada cambio | mantenibilidad |

## 10. Estado de la documentación

| Documento | Estado |
|---|---|
| `AGENT.md` | Completo |
| `PROJECT_OVERVIEW.md` | Completo |
| `ARCHITECTURE.md` | Completo |
| `PROJECT_MAP.md` | Completo |
| `CODING_CONVENTIONS.md` | Completo |
| `WINFORMS_PATTERNS.md` | Completo |
| `DAL_PATTERNS.md` | Completo |
| `DEPENDENCIES.md` | Completo |
| `KNOWN_ISSUES.md` | Completo |
| `DECISIONS.md` | Completo |
| `ANALYSIS_STATUS.md` | Completo |

**Fin de la auditoría.** La auditoría inicial (2026-10-28) no modificó código de la aplicación; solo agregó documentación. Las implementaciones posteriores de CUN06/CUN07/CUN08, reportes y reimpresión sí tocaron código y están registradas en `AGENT.md` y en la sección 7 de este documento.
