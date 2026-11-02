# DAL_PATTERNS — Acceso a datos

## 1. Puerta única

Todo el acceso pasa por `DALAcceso577MC` (`DAL/DALAcceso577MC.cs`). No hay `SqlConnection` directas salvo en `DALBackUpRestore577MC`, que necesita conectarse a `master` para el restore.

### 1.1 Métodos

| Método | Firma | Uso |
|---|---|---|
| `executeDataTable` | `DataTable executeDataTable(string query, Dictionary<string, object> parametros = null)` | SELECT de una tabla |
| `executeDataTableBatch` | `DataTable[] executeDataTableBatch(string query, Dictionary<string, object> parametros = null)` | varias sentencias SELECT en una sola conexión (permisos) |
| `executeNonQuery` | `int executeNonQuery(string consulta, Dictionary<string, object> parametros = null)` | INSERT/UPDATE/DELETE; devuelve filas afectadas |
| `executeScalar` | `object executeScalar(string consulta, Dictionary<string, object> parametros = null)` | un único valor |

### 1.2 Comportamiento

- La cadena se resuelve en cada llamada: `_stringConnection => DALConexionConfig577MC.ObtenerConnectionString()` (línea 15).
- Cada método crea y destruye su `SqlConnection` con `using`.
- Los parámetros se agregan con `AddWithValue(p.Key, p.Value ?? DBNull.Value)` (líneas 28-31, 58-63, 95-101, 122-128).
- Los `SqlException` se capturan, se loguean y se relanzan como `Exception` genérica con un mensaje amigable (líneas 40-44, 78-82, 107-111, 135-139).
- `DetalleDelError` traduce los errores 2627/2601 (duplicado), 515 (NULL), 8152/2628 (longitud), 207 (columna), 208 (tabla); el resto devuelve `ex.Message` (líneas 144-163).
- `RegistrarErrorLog` escribe en `%LOCALAPPDATA%\577MC\Logs\ErroresDAL.log` con lock estático (líneas 165-211).
- **Riesgo:** el log incluye **todos los parámetros con sus valores** (líneas 183-191), incluidos DNI, emails y hashes de contraseña. No loguear datos sensibles sin revisar esto.

### 1.3 Consecuencia del relanzado

El `throw new Exception(..., ex)` pierde el tipo `SqlException` para los llamadores. La UI solo ve un mensaje. Si se necesita distinguir errores, hay que inspeccionar `InnerException`.

## 2. Patrón de una clase DAL

```csharp
public class DALCliente577MC
{
    DALAcceso577MC acceso = new DALAcceso577MC();

    public DataTable ObtenerTodos()
    {
        string query = "SELECT DNI, Nombre, Apellido, Telefono, Email FROM Cliente";
        return acceso.executeDataTable(query);
    }

    public int InsertarCliente(string dni, string nombre, string apellido,
                               string telefono, string email, long dvh)
    {
        string query = @"INSERT INTO Cliente (DNI, Nombre, Apellido, Telefono, Email, DVH)
                         VALUES (@dni, @nombre, @apellido, @telefono, @email, @dvh)";
        var parametros = new Dictionary<string, object>
        {
            { "@dni", dni }, { "@nombre", nombre }, { "@apellido", apellido },
            { "@telefono", telefono }, { "@email", email }, { "@dvh", dvh }
        };
        return acceso.executeNonQuery(query, parametros);
    }
}
```

Características:

- Campo `acceso` (o `_dal`) instanciado por clase.
- SQL como string literal; sin `StringBuilder` ni query builders.
- Un `Dictionary<string, object>` por llamada.
- Sin `try/catch` en la clase DAL: deja que `DALAcceso` traduzca y relance.
- Devuelve `DataTable`/`DataRow`/`int`/`bool`; el mapeo a `BE` ocurre en BLL.

## 3. SQL inline

- No hay stored procedures, funciones, vistas ni migraciones.
- No hay `SqlTransaction` en ningún método. Las operaciones multi-tabla (crear rol + asignar permisos, reparar DVH) se ejecutan en llamadas separadas y pueden quedar a medias.
- `executeDataTableBatch` permite varias sentencias en un solo `SqlDataAdapter.Fill`, pero **no** es transaccional para escrituras.
- Se usa `SELECT *` en varios métodos (`DALCliente577MC.ObtenerTodos`, `DALUsuario577MC.obtenerTodos`, etc.). Al agregar columnas, el mapeo por nombre en BLL sigue funcionando, pero se transfiere de más.
- Los nombres de tabla no se califican con `dbo.` en el código, salvo scripts.
- `DALPropiedad577MC.Buscar` construye la cláusula `WHERE` dinámicamente con parámetros (`@direccion`, `@tipo`, etc.), no concatena valores.
- `DALBitacora577MC.obtenerBitacora` arma filtros dinámicos por fecha y módulo.

## 4. Mapeo DataTable → BE

El BLL recorre `DataTable.Rows` y crea entidades:

```csharp
foreach (DataRow r in dal.ObtenerTodos().Rows)
{
    lista.Add(new Cliente577MC
    {
        DNI = r["DNI"].ToString(),
        Nombre = r["Nombre"].ToString(),
        ...
    });
}
```

(`BLLCliente577MC.cs:35-53`)

- Se accede por nombre de columna, no por índice.
- `Convert.ToInt32`, `Convert.ToDecimal`, `Convert.ToDateTime` se usan en los mapeos.
- Si una columna falta o es NULL, se produce excepción o `InvalidCastException`; no hay validación defensiva.

## 5. DVH en el DAL

- Cada tabla de negocio (`Cliente`, `Propiedad`, `Visita`, `Usuario`, `Rol`, `Familia`, `Patente`) tiene columna `DVH bigint`.
- El DAL recibe el `long dvh` ya calculado y lo persiste en el `INSERT`/`UPDATE`.
- `ActualizarDVH(id/dni, long dvh)` existe en las clases correspondientes para reparaciones.
- `DALDigitoVerificador577MC` maneja la tabla de control `DigitoVerificador`:
  - `GuardarDVV(tabla, dvv, dvh)`
  - `ActualizarDVV(tabla, dvv, dvh)`
  - `ExisteTabla(tabla)`
  - `ObtenerFila(tabla)`
  - `ObtenerDatosVerificacion()` → batch con las tablas a verificar.

## 6. Backup / restore

`DALBackUpRestore577MC` es la excepción a la regla:

- Usa `DALAcceso577MC` para el backup (`BACKUP DATABASE ... TO DISK = @ruta`).
- Para el restore abre una `SqlConnection` propia contra `master` (`líneas 32-50`), porque no se puede restaurar la base activa.
- Cadena hardcodeada: `Data Source=LAPTOP-8BNKG482\SQLEXPRESS;Initial Catalog=is--servicios;Integrated Security=True` (línea 15).
- El nombre de la base está hardcodeado: `_nombreDB = "is--servicios"` (línea 13).
- No valida ruta, archivo ni compatibilidad. `WITH REPLACE` sobrescribe sin backup previo.

## 7. Tablas y columnas usadas por el código

| Tabla | Columnas referenciadas |
|---|---|
| `Cliente` | `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `DVH` |
| `Propiedad` | `Id`, `Direccion`, `Tipo`, `Estado`, `Precio`, `SuperficieM2`, `Ambientes`, `Dormitorios`, `Banios`, `DVH` |
| `Visita` | `IdVisita`, `IdPropiedad`, `DNICliente`, `Fecha`, `HoraInicio`, `HoraFin`, `Estado`, `Observaciones`, `DVH` |
| `Usuario` | `DNI`, `Nombre`, `Apellido`, `Username`, `Email`, `PasswordHash`, `IdRol`, `IdIdioma`, `Activo`, `Bloqueo`, `Intentos`, `UltimoIntentoFallido`, `DVH` |
| `Rol` | `Id`, `Nombre`, `DVH` |
| `Familia` | `Id`, `Nombre`, `DVH` |
| `Patente` | `Id`, `Nombre`, `DVH` |
| `Rol_Patente` | `IdRol`, `IdPatente` |
| `Rol_Familia` | `IdRol`, `IdFamilia` |
| `Familia_Patente` | `IdFamilia`, `IdPatente` |
| `Familia_Familia` | `IdFamiliaPadre`, `IdFamiliaHija` |
| `BitacoraEventos` | `Id`, `DNI`, `Evento`, `Criticidad`, `Modulo`, `FechaHora` |
| `DigitoVerificador` | `NombreTabla`, `DVV`, `DVH` |
| `Idioma` | `Id`, `Nombre` |
| `Contrato` | **Existe en la base, no en scripts**: `NroContrato`, `FechaEmision`, `DNIPropietario`, `DNICliente`, `CodigoPropiedad`, `TipoContrato`, `FechaInicio`, `FechaFin`, `MontoAcordado`, `FormaPago`, `DepositoGarantia`, `Estado`, `DVH` |

> El esquema de `Contrato` fue verificado contra la base `is--servicios` (PK, FKs y DVH). Falta agregarlo a `scriptTerceraEntrega.sql` para instalaciones desde cero. La firma usa una transacción real en `DALContrato577MC.ActualizarEstadoConPropiedad`.

## 8. Cómo agregar una operación DAL (receta)

1. Crear/abrir `DAL/X577MC.cs`.
2. Agregar `DALAcceso577MC acceso = new DALAcceso577MC();`.
3. Escribir SQL con parámetros `@nombre`; nunca concatenar valores.
4. Crear `Dictionary<string, object>` con todos los parámetros.
5. Devolver `DataTable`/`int`/`bool` según corresponda.
6. Agregar el archivo al `DAL_577MC.csproj` (`<Compile Include="...">`).
7. Si hay DVH, recibirlo como parámetro y persistirlo.
8. Si la operación es multi-tabla, evaluar transacción: hoy no hay patrón; si se agrega, hacerlo en `DALAcceso577MC` con un método nuevo, no en cada DAL.
