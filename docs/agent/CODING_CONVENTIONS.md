# CODING_CONVENTIONS — Perfil de código real

> Este documento describe **lo que el código hace**, no lo que debería hacer. Las recomendaciones están marcadas como tales.

## 1. Nombres

| Elemento | Convención observada | Evidencia |
|---|---|---|
| Tipos propios | PascalCase + sufijo `577MC` | `BLLCliente577MC`, `DALVisita577MC`, `Visita577MC` |
| Namespaces | `BE`, `BE.Enum`, `DAL`, `BLL`, `Services`, `Services.Modelos.*`, `Services_577MC`, `Servicios` | archivos |
| Clases BLL/DAL | `BLL`/`DAL` + entidad + `577MC` | `BLLPropiedad577MC`, `DALPropiedad577MC` |
| Métodos públicos | PascalCase en la mayoría (`ObtenerTodos`, `ProgramarVisita`) | `BLLCliente577MC.cs:35`, `BLLVisita577MC.cs:62` |
| Métodos públicos | **camelCase en varios** (`obtenerTodos`, `crearRol`, `asignarPatenteAFamilia`, `insertarFamilia`) | `BLLRol577MC.cs:19`, `DALFamilia577MC.cs:15` |
| Campos privados | `_camelCase` o sin guion | `_clienteBLL`, `_propiedad`, `dal`, `bit` |
| Controles WinForms | Prefijo húngaro (`btn`, `txt`, `lbl`, `dgv`, `cbo`, `dtp`, `gb`, `rb`) | `*.Designer.cs` |
| Parámetros SQL | `@camelCase` | `@nroContrato`, `@dniCliente` |
| Claves de traducción | `Formulario.clave` o `ClaveSuelta` | `Login.formTitle`, `ExcUsuarioBloqueado` |

> **Inconsistencia verificada:** los métodos de DAL/BLL alternan PascalCase y camelCase. No hay analizador ni `.editorconfig` que lo imponga. Al agregar código, seguir el estilo del archivo que se está tocando, no una regla global.

## 2. Estructura de archivos

- Un tipo principal por archivo, con el mismo nombre.
- Formularios: `X.cs` (lógica), `X.Designer.cs` (generado) y `X.resx`.
- `using` completos al inicio, muchos sin usar (generados por plantilla).
- Namespace con llaves (`namespace BLL { ... }`), no file-scoped.
- No hay `nullable` annotations (C# 7.3 / .NET Framework).
- Se usa `var` con frecuencia, pero también tipos explícitos.

## 3. Manejo de errores

### 3.1 DAL

- `DALAcceso577MC` captura `SqlException`, arma un detalle legible y registra en un log de texto.
- `DetalleDelError` traduce números de error comunes (`DALAcceso577MC.cs:144-163`).
- `RegistrarErrorLog` escribe con lock estático (`líneas 165+`).
- Los métodos públicos de DAL **no** relanzan una excepción de negocio: dejan pasar la original o devuelven el resultado.

### 3.2 BLL

- Valida y lanza `new Exception(mensajeTraducido)`.
- Ejemplo: `BLLCliente577MC.RegistrarCliente` lanza si el DNI ya existe (`BLLCliente577MC.cs:123-127`).
- Helper de traducción local en algunas clases:
  ```csharp
  private static string Tr(string key, params object[] args)
  {
      var idioma = Services_577MC.ServiceSessionManager577MC.getIntancia().Idioma;
      return string.Format(idioma != null ? idioma.Translate(key) : key, args);
  }
  ```
  (`BLLCliente577MC.cs:19-23`)

### 3.3 UI

- Patrón dominante:
  ```csharp
  try { ... }
  catch (Exception ex)
  {
      MessageBox.Show(ex.Message, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
  ```
- Se muestran mensajes de validación con `MessageBoxIcon.Warning` y confirmaciones con `YesNo` + `Question`.
- No hay un manejador global de excepciones; una excepción no capturada cierra la app.

### 3.4 Recomendación (no aplicada)

- No cambiar a un sistema de logging o excepciones tipadas sin pedido: impacta toda la UI.
- Si se agrega una validación, lanzar `Exception` con clave traducida y capturarla en el formulario, para mantener consistencia.

## 4. Acceso a datos

- SQL embebido en el DAL, en strings `@"..."` o `"..."`.
- Parámetros siempre por `Dictionary<string, object>` y `AddWithValue`.
- No hay stored procedures ni vistas.
- No hay transacciones explícitas.
- `SELECT *` aparece en varios métodos (por ejemplo `DALCliente577MC.ObtenerTodos`).
- Los nombres de tabla/columna se escriben sin corchetes en la mayoría de los casos; backup/restore sí los usa por el guion del catálogo.
- Ver `docs/agent/DAL_PATTERNS.md`.

## 5. DVH / DVV

- El DVH se calcula concatenando los campos de la fila en el orden definido por el BLL:
  - Cliente: `DNI + Nombre + Apellido + Telefono + Email` (`BLLCliente577MC.cs:86`).
  - Propiedad y Visita: ver `BLLPropiedad577MC` y `BLLVisita577MC`.
- `DigitoVerificador577MC.CalcularDVH(string)` usa una suma ponderada sobre los caracteres.
- El DVV es la suma de los DVH de la tabla.
- **Regla implícita:** toda operación de alta/modificación/baja debe recalcular el DVH de la fila y el DVV de la tabla. No siempre se cumple (ver `KNOWN_ISSUES.md`).
- Los seeds SQL traen DVH/DVV precalculados; si se cambian datos a mano hay que recalcular.

## 6. Internacionalización

- Las cadenas visibles se obtienen con `ServiceSessionManager577MC.getIntancia().Idioma.Translate("clave")`.
- Cada formulario implementa `IIdiomaObserver` y define `actualizarIdioma()`.
- Las claves se agregan a `Lenguajes/es.json` y `en.json` (459 cada uno al 2026-10-28; ambos conjuntos deben quedar idénticos).
- Los reportes PDF obtienen sus textos con claves bajo `Reporte.*`, `ReporteRN1.*` y `ReporteRN2.*`; la marca visible es siempre **Propiedades Calella** (nunca el sufijo `577MC`).
- Algunos textos están hardcodeados en español y no pasan por `Translate` (por ejemplo mensajes de `FrmGenerarContrato` y varios `MessageBox` de respaldo).
- **Encoding corrupto:** hay literales con secuencias tipo `A3`, `A±`, `A?` en lugar de `ó`, `ñ`, `í`. Ejemplos: `BLLCliente577MC.cs` ("modificA3", "eliminA3", "registrA3"), `FrmGenerarContrato.cs` ("A�Desea", "AA�n", "vA�lida", "depA3sito"). [VERIFICADO]
- Los nombres de archivo `CambiarContraseña.cs` aparecen como `CambiarContrase�a.cs` en el sistema de archivos, pero el `.csproj` los referencia con el carácter correcto o con la misma corrupción; compila igual porque el nombre físico coincide con el del proyecto.

## 7. Comentarios

- Comentarios en español, frecuentemente explicando decisiones o TODOs informales.
- Ejemplos: `DALBackUpRestore577MC.cs:16` ("si no apuntamos a la master falla"), `IdiomaManager.cs` ("Serializador sin usar Paquetes").
- No hay documentación XML salvo en `Program.cs`.

## 8. Formato

- Indentación de 4 espacios; llaves en línea nueva (estilo Allman).
- Líneas largas sin límite estricto.
- Uso de `#region` en formularios grandes (`ProgramarVisita.cs`).
- EventArgs con nombres `sender, e`; métodos de evento `control_Evento`.
- No hay `.editorconfig`, ni StyleCop, ni Roslyn analyzers.

## 9. Recursos y rutas

- Los JSON de idioma se copian a `bin\Debug\Lenguajes` mediante el `.csproj` (ver `<Content>`/`CopyToOutputDirectory`).
- El logo del PDF se resuelve de forma portable desde `Recursos\logo-propiedades-calella.jpg` (copiado al output) con fallback a `..\..\Recursos`; ya no hay ruta absoluta.
- El backup/restore usa una instancia hardcodeada (`DALBackUpRestore577MC.cs:15`).
- **Recomendación:** mover rutas a `App.config` o a `Recursos` con `Application.StartupPath`; no hacerlo sin pedido.

## 10. Reglas prácticas para nuevos cambios

1. Copiar el patrón del archivo vecino (DAL que devuelve `DataTable`, BLL que mapea, form con `IIdiomaObserver`).
2. Agregar la clase al `.csproj` correspondiente (los proyectos usan lista explícita de `Compile Include`; **no** es SDK-style).
3. Agregar claves a ambos JSON.
4. Si se agrega una tabla, incluir script SQL idempotente y actualizar DVH/DVV.
5. Registrar en bitácora las operaciones de negocio.
6. Compilar los 5 proyectos; la UI es el único que hoy falla.
7. No reformatear archivos completos: genera diffs enormes y dificulta la revisión.
