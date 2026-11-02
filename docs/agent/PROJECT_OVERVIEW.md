# PROJECT_OVERVIEW — Visión general

## 1. Dominio

Sistema de gestión inmobiliaria ("Servicios") con estas capacidades:

| Capacidad | Evidencia |
|---|---|
| Alta, modificación, baja y listado de clientes | `RegistrarCliente.cs`, `BLL/BLLCliente577MC.cs`, `DAL/DALCliente577MC.cs` |
| Alta, modificación, baja, búsqueda y selección de propiedades | `SeleccionarPropiedad.cs`, `BLL/BLLPropiedad577MC.cs`, `DAL/DALPropiedad577MC.cs` |
| Agenda de visitas con control de superposición horaria | `ProgramarVisita.cs`, `BLL/BLLVisita577MC.cs`, `DAL/DALVisita577MC.cs` |
| Actualización de estado de visitas | `ActualizarEstadoVisita.cs`, `BLLVisita577MC.ActualizarEstadoVisita` |
| Consulta de disponibilidad | `ConsultarDisponibilidad.cs` (nuevo, untracked) |
| Generación de contrato (CUN06) | `FrmGenerarContrato.cs`, `BLL/BLLContrato577MC.cs`, `DAL/DALContrato577MC.cs` |
| Términos y condiciones del contrato (CUN07) | `FrmRegistrarTerminosCondiciones.cs`, `BE/TerminosContrato577MC.cs`, `BLLContrato577MC.ValidarTerminos` |
| Actualización de estado de contrato/propiedad (CUN08) | `FrmActualizarEstadoPropiedadContrato.cs`, `BLLContrato577MC.ActualizarEstadoContrato` |
| Reimpresión de copia final | `FrmReimprimirContrato.cs`, `BLLContrato577MC.ObtenerContratosFirmados` |
| Seguridad: usuarios, roles, familias, patentes | `GestionUsuario.cs`, `GestionRol.cs`, `GestionFamilia.cs`, `BLL/BLLUsuario577MC.cs`, `BLL/BLLRol577MC.cs`, `BLL/BLLFamilia577MC.cs` |
| Auditoría de eventos | `AuditoriaBitacora.cs`, `BLL/BLLBitacora577MC.cs`, `DAL/DALBitacora577MC.cs` |
| Integridad con DVH/DVV | `Services/DigitoVerificador577MC.cs`, `DAL/DALDigitoVerificador577MC.cs` |
| Respaldo y restauración de BD | `GestionRespaldo.cs`, `Services/BackUpRestore577MC.cs`, `DAL/DALBackUpRestore577MC.cs` |
| Internacionalización español/inglés | `Lenguajes/es.json`, `Lenguajes/en.json`, `Services/IdiomaManager.cs` |
| Comprobante de visita en PDF (RN1) | `ReporteRN1577MC.cs`, `ReportePdf577MC.cs` |
| Copia final del contrato en PDF (RN2) | `ReporteRN2577MC.cs`, `ReportePdf577MC.cs` |

## 2. Stack técnico

| Elemento | Valor | Fuente |
|---|---|---|
| Lenguaje | C# | `.cs` |
| Framework | .NET Framework 4.7.2 | `GUI_577MC.csproj:11`, `BE_577MC.csproj:12`, etc. |
| UI | Windows Forms | `GUI_577MC.csproj:8` (`WinExe`) |
| Acceso a datos | ADO.NET (`System.Data.SqlClient`) | `DAL/DALAcceso577MC.cs` |
| Base de datos | SQL Server | `App.config:6-9`, scripts |
| Serialización JSON | `System.Web.Script.Serialization` | `Services/IdiomaManager.cs:7` |
| PDF | GDI+ / `System.Drawing` → JPEG → PDF 1.4 (sin librería externa) | `ReportePdf577MC.cs` |
| Paquetes NuGet | Ninguno (carpeta `packages` vacía) | `packages/` (0 archivos) |
| Pruebas | Ninguna | no hay proyecto de test |
| CI/CD | Ninguno | no hay pipelines |

## 3. Solución y proyectos

`Servicios.sln` contiene:

| Proyecto | Ensamblado | Tipo | Salida |
|---|---|---|---|
| `GUI_577MC.csproj` | `Servicios` | WinExe | `Servicios.exe` |
| `BE/BE_577MC.csproj` | `BE` | Library | `BE.dll` |
| `DAL/DAL_577MC.csproj` | `DAL` | Library | `DAL.dll` |
| `BLL/BLL_577MC.csproj` | `BLL` | Library | `BLL.dll` |
| `Services/Services_577MC.csproj` | `Services` | Library | `Services.dll` |
| `Sistema_IngSoftware.vdproj` | — | Deployment | **No existe** en `..\Sistema_IngSoftware\` |

> El proyecto de despliegue está referenciado en `Servicios.sln` pero no existe en disco. [VERIFICADO]

## 4. Configuración

`App.config`:

```xml
<add name="ConexionPrincipal"
     connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=is--servicios;Integrated Security=True"
     providerName="System.Data.SqlClient" />
```

- La cadena se lee en `DAL/DALConexionConfig577MC.cs:12-14` por nombre `ConexionPrincipal`.
- `DAL/DALBackUpRestore577MC.cs:15` **no** usa esa cadena: hardcodea `LAPTOP-8BNKG482\SQLEXPRESS`. [VERIFICADO]
- Nombre de catálogo con guiones: `is--servicios` (requiere escapado con corchetes en SQL, ya aplicado en backup/restore).

## 5. Cómo compilar y ejecutar

```powershell
# Compilar
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" `
  Servicios.sln /t:Build /p:Configuration=Debug /v:minimal

# Ejecutar (solo si compila)
.\bin\Debug\Servicios.exe
```

Estado 2026-10-28: **compila correctamente** y genera `bin\Debug\Servicios.exe`. Solo quedan warnings `CS2002` preexistentes por `AssemblyAttributes.cs` duplicados. [VERIFICADO]

Los reportes se guardan en `Downloads\Propiedades Calella\Comprobantes` (RN1) y `Downloads\Propiedades Calella\Contratos` (RN2).

## 6. Base de datos

- Script principal: `Scripts/scriptTerceraEntrega.sql` (765 líneas). Crea `is--servicios` y carga esquema + seeds.
- Tablas: `BitacoraEventos`, `DigitoVerificador`, `Familia`, `Familia_Familia`, `Familia_Patente`, `Idioma`, `Patente`, `Rol`, `Rol_Familia`, `Rol_Patente`, `Usuario`, `Cliente`, `Propiedad`, `Visita`.
- La tabla `Contrato` **existe en la base** `is--servicios` (creada manualmente) y participa de DVH/DVV y bitácora, pero **no está en los scripts** de creación.
- El DDL de `Visita` (líneas 282-293) es inválido y el script no puede ejecutarse tal cual.
- Scripts incrementales en `Scripts/` agregan/ajustan tablas, permisos y un índice.

## 7. Idioma y textos

- `Lenguajes/es.json` y `Lenguajes/en.json`: 459 claves cada uno, con el mismo conjunto de claves. [VERIFICADO]
- `IdiomaManager` carga el JSON desde `AppDomain.CurrentDomain.BaseDirectory/Lenguajes/{cod}.json` (`Services/IdiomaManager.cs:35-51`).
- Si falta una clave, `Translate` devuelve la propia clave (`IdiomaManager.cs:53-60`).
- Hay textos de código con encoding corrupto (`A3`, `A±`, `A?`) que se muestran mal en la UI. [VERIFICADO]

## 8. Usuarios y contraseñas

- Usuario generado: `nombre + dni` (`BLLUsuario577MC.GenerarUsuario`, línea 335).
- Contraseña por defecto: `apellido + dni` (`GenerarPassword`, línea 340).
- Hash: SHA-256 sin sal (`Services/ServiceSeguridad577MC.cs:12`).
- 4 intentos fallidos bloquean la cuenta (`BLLUsuario577MC.login`, líneas 80-88).
- Reset de intentos a los 30 minutos (líneas 62-70).
- Si la contraseña es la default, el login deriva a `CambiarContraseña`.

## 9. Convención de nombres

Todos los tipos propios llevan sufijo `577MC` (iniciales del equipo/curso). Ejemplos: `BLLCliente577MC`, `DALVisita577MC`, `Propiedad577MC`, `ServiceSessionManager577MC`. Ver `docs/agent/CODING_CONVENTIONS.md`.
