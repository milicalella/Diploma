# DEPENDENCIES — Dependencias y entorno

## 1. Referencias entre proyectos

| Proyecto | Referencias | Líneas |
|---|---|---|
| `GUI_577MC` (`Servicios`) | `BE_577MC`, `BLL_577MC`, `Services_577MC` | `GUI_577MC.csproj:225-235` |
| `BLL_577MC` | `BE_577MC`, `DAL_577MC`, `Services_577MC` | `BLL_577MC.csproj:56-67` |
| `DAL_577MC` | `BE_577MC` | `DAL_577MC.csproj:65-68` |
| `Services_577MC` | `BE_577MC`, `DAL_577MC` | `Services_577MC.csproj:62-69` |
| `BE_577MC` | — | — |

### 1.1 Grafo

```
GUI ──► BLL ──► DAL ──► BE
 │       │        ▲
 │       └──► Services ──► DAL
 └──────────► Services
```

- No hay dependencia `DAL → BLL` ni `BE → *`.
- `Services → DAL` es la dependencia lateral que rompe el esquema estrictamente por capas.
- `BLL → Services` se usa para sesión, hash, DVH e i18n.

## 2. Ensamblados del framework

### GUI

`System`, `System.Core`, `System.Xml.Linq`, `System.Data.DataSetExtensions`, `Microsoft.CSharp`, `System.Data`, `System.Deployment`, `System.Drawing`, `System.Net.Http`, `System.Windows.Forms`, `System.Xml` (`GUI_577MC.csproj:36-49`).

### DAL

`System`, `System.Configuration`, `System.Core`, `System.Xml.Linq`, `System.Data.DataSetExtensions`, `Microsoft.CSharp`, `System.Data` (con `System.Data.SqlClient`), `System.Net.Http`, `System.Xml` (`DAL_577MC.csproj:34-45`).

### Services

`System`, `System.Configuration`, `System.Core`, `System.Web.Extensions` (para `JavaScriptSerializer`), `System.Xml.Linq`, `System.Data.DataSetExtensions`, `Microsoft.CSharp`, `System.Data`, `System.Net.Http`, `System.Xml` (`Services_577MC.csproj:34-44`).

### BE / BLL

Mismos ensamblados base; BLL no tiene referencias externas adicionales.

## 3. Paquetes NuGet

**Ninguno.** La carpeta `packages/` existe pero está vacía (0 archivos). No hay `packages.config` ni `PackageReference`. El JSON se serializa con `System.Web.Extensions`, incluido en .NET Framework.

> Consecuencia: no se puede agregar una librería externa sin incorporar el paquete y actualizar el `.csproj` a mano (los proyectos no son SDK-style).

## 4. Dependencias de ejecución

| Dependencia | Requerida para | Verificación |
|---|---|---|
| Windows | WinForms | plataforma |
| .NET Framework 4.7.2 | ejecutar `Servicios.exe` | `TargetFrameworkVersion` |
| SQL Server (instancia `.\SQLEXPRESS` por defecto) | toda la app | `App.config` |
| Base `is--servicios` creada y poblada | login y ABMs | scripts |
| Permiso de escritura en `%LOCALAPPDATA%\577MC\Logs` | logging de errores DAL | `DALAcceso577MC.cs:169-176` |
| Permiso de escritura en `Downloads` | RN1 y RN2 (`Downloads\Propiedades Calella\...`) | `ReportePdf577MC.ObtenerCarpetaDescargas` |
| Logo `Recursos\logo-propiedades-calella.jpg` copiado al output | PDF con logo | `GUI_577MC.csproj` + `ReportePdf577MC.ObtenerRutaLogo` |
| Fuente **Segoe UI** instalada | tipografía de RN1/RN2 | `ReportePdf577MC.ResolverFamilia` usa Verdana como fallback si no está |
| Driver/impresora predeterminada | vista previa con `PrintPreviewDialog` | `ReportePdf577MC.MostrarVistaPrevia` (los errores se informan sin romper la app) |
| Permiso de backup/restore en SQL Server | `GestionRespaldo` | `BACKUP/RESTORE DATABASE` |

## 5. Archivos copiados a la salida

| Archivo | Modo | Fuente |
|---|---|---|
| `Lenguajes\en.json` | `Always` | `GUI_577MC.csproj:202-204` |
| `Lenguajes\es.json` | `Always` | `GUI_577MC.csproj:205-207` |
| `Scripts\scriptTerceraEntrega.sql` | `PreserveNewest` | `GUI_577MC.csproj` |
| `Recursos\logo-propiedades-calella.jpg` | `Always` | `GUI_577MC.csproj` |

El script se copia porque `Configuracion_Inicial` / `ServicioConfiguracionConexion.EjecutarScriptCreacion` lo usa para crear la base.

## 6. Dependencias del instalador

- `Servicios.sln` referencia `..\Sistema_IngSoftware\Sistema_IngSoftware.vdproj`.
- El archivo **no existe** en el repositorio ni en el directorio padre.
- Visual Studio Installer Projects no está incluido en VS 2022 por defecto; aun así, la solución carga con el proyecto faltante.
- No hay `.iss` (Inno Setup), WiX, MSIX ni script de publicación alternativo.

## 7. Variables de entorno y configuración

- No hay variables de entorno.
- No hay transformaciones por configuración (`App.Debug.config`, etc.).
- La única configuración es `App.config` → `ConexionPrincipal`.
- `Configuracion_Inicial` puede reescribir la cadena en el `.config` de la aplicación mediante `ServicioConfiguracionConexion.GuardarConnectionString` (`Services/Instalador/ServicioConfiguracionConexion.cs:97`).
- El flujo de configuración inicial está **comentado** en `Program.cs` (líneas 23-41), por lo que en la práctica se usa siempre la cadena de `App.config`.

## 8. Riesgos de dependencias

| Riesgo | Detalle |
|---|---|
| Instancia hardcodeada | `DALBackUpRestore577MC` ignora `App.config` y usa `LAPTOP-8BNKG482\SQLEXPRESS` |
| Catálogo hardcodeado | backup/restore usan `is--servicios` |
| ~~Ruta de logo absoluta~~ | **resuelto**: logo portable copiado al output |
| Proyecto de despliegue ausente | no se puede generar el instalador desde la solución |
| Sin NuGet | cualquier librería nueva requiere edición manual del `.csproj` |
| `System.Web.Extensions` | dependencia de framework, no de paquete; no disponible en .NET Core |
| SQL Server Express local | no hay soporte para instancia remota sin cambiar configuración |

## 9. Compatibilidad

- El código usa C# compatible con .NET Framework 4.7.2 (interpolación, `out var`, expresiones `=>`, `?.`).
- No usa APIs de .NET Core/.NET 5+.
- `System.Web.Script.Serialization` impide una migración directa a .NET moderno sin reemplazo (`System.Text.Json`).
- WinForms Designer requiere Visual Studio en Windows.
