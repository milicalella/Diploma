# AGENT.md — Guía de trabajo para agentes de IA

> Base de conocimiento del sistema **Servicios / GUI_577MC** (gestión inmobiliaria, WinForms .NET Framework 4.7.2 + SQL Server).
> Última actualización: 2026-11-02 (regla de operabilidad RF2 implementada: no se generan contratos sobre propiedades no operables ni duplicados pendientes).

## 1. Qué es este repositorio

Aplicación de escritorio WinForms para administrar clientes, propiedades, visitas, contratos (CUN06, CUN07 y CUN08 implementados), reportes PDF de visita y de contrato, usuarios, roles/familias/patentes, bitácora de auditoría, respaldo/restauración e integridad de datos mediante DVH/DVV.

- Solución: `Servicios.sln` (5 proyectos C# + 1 proyecto de despliegue inexistente).
- UI: `GUI_577MC.csproj` → ensamblado `Servicios.exe`.
- Base de datos: SQL Server, catálogo `is--servicios`, cadena en `App.config`.
- Entrada: `Program.cs` → `Application.Run(new Login())`.

## 2. Estado actual (verificado 2026-10-30)

- **La solución compila en Debug** con MSBuild 2022 y genera `bin\Debug\Servicios.exe` (último build: 2026-10-30). Solo queda el warning `CS2002` por `AssemblyAttributes.cs` duplicado.
- El último cambio de código fue `FrmActualizarEstadoPropiedadContrato.cs` (2026-10-29 00:08); el build del 2026-10-30 ya lo incluye.
- **CUN06 "Generar contrato" implementado** de punta a punta (BE, DAL, BLL, Services y GUI). Detalle en la sección 7.
- **CUN07 "Registrar términos y condiciones" implementado** como modal de CUN06, con validación en BLL y sin persistencia propia. Detalle en la sección 8.
- **CUN08 "Actualizar estado de propiedad y contrato" implementado y verificado** (BE, DAL, BLL y GUI). Al firmar genera la copia final real en PDF. Detalle en la sección 9.
- **RN1 (comprobante de visita) y RN2 (copia final del contrato) implementados**, con marca **Propiedades Calella** y logo portable. Detalle en la sección 10.
- **Contratos → Reimprimir Copia Final** implementado (`FrmReimprimirContrato`): lista contratos `Firmado` y regenera el RN2. Mismo permiso que CUN08.
- `CUN07` ya **no ofrece `Comodato`**: solo `Venta` y `Alquiler`. Un contrato viejo con `Comodato` no se puede firmar (la BLL lo rechaza sin modificar datos).
- La tabla `Contrato` existe en la base `is--servicios` (creada manualmente por el usuario) y su esquema coincide con lo esperado (sección 7).
- Estado real de la base al 2026-10-30 (datos dejados por las pruebas manuales del usuario; el agente no los modificó):
  - Contratos: `C-20261028-0001` (Alquiler, `Firmado`, propiedad 14), `C-20261028-0002` (Venta, `Firmado`, propiedad 11), `C-20261028-0003` (Venta, `Cancelado`, propiedad 11), `C-20261028-0004` (Venta, `Pendiente`, propiedad 11) y `C-20261028-0005` (Alquiler, `Firmado`, propiedad 9).
  - Propiedades: 9 y 14 `Alquilada`, 11 `Vendida`, 10 `Disponible`.
  - `DVV Contrato = -643719723159986574`, verificado contra la suma de los `DVH` (coincide; el signo negativo es por el desborde controlado de `long`). `MaxBit = 648`.
  - `C-20261028-0004` queda `Pendiente` sobre la propiedad 11 `Vendida` por decisión del usuario: no se puede firmar mientras la propiedad siga en ese estado. Detalle en la sección 12.
- En CUN06 los términos ya **no se editan**: se cargan en el modal CUN07 y se muestran en solo lectura. La validación de términos es única (`BLLContrato577MC.ValidarTerminos`) y exige `DepositoGarantia > 0`.
- `Contrato` participa de DVH/DVV y de la bitácora (módulo `Contrato = 7`).
- `Scripts/scriptTerceraEntrega.sql` **no** incluye el DDL de `Contrato` y mantiene el DDL inválido de `Visita` (`KI-03`); no fue modificado.
- Los archivos de CUN06, CUN07, CUN08 y de los reportes siguen **sin commitear** (`git status`).
- El disco `C:` estaba lleno (0 bytes libres) durante la verificación; se liberaron ~0,5 GB limpiando temporales. Revisar espacio antes de compilar.
- `docs/agent/` contiene 10 documentos y quedó sincronizada con este `AGENT.md` el 2026-10-30.

## 3. Reglas duras para cualquier agente

> **Convención de evidencia:** lo que se afirma sin etiqueta está **verificado** por lectura directa de código/scripts o por una ejecución reproducible. `[INFERIDO]` = deducido del código sin ejecutar; `[HIPÓTESIS]` = requiere confirmación humana; `[RECOMENDACIÓN]` = propuesta, no implementada.

1. **No inventar** tablas, columnas, formularios, permisos ni dependencias. Si no está en el código o en los scripts, es hipótesis.
2. **Respetar la arquitectura real**: `Form → BLL → DAL → DALAcceso577MC → SQL Server`. No introducir DI, repositorios, ORM, CQRS, middleware, migraciones ni capas nuevas.
3. **No cambiar el esquema ni los datos** sin pedido explícito y sin script idempotente/reversible.
4. **No tocar trabajo no confirmado** (ver `git status`): `ConsultarDisponibilidad.*` y cambios ajenos a la tarea. `docs/agent/` es la base de conocimiento del agente (ADR-013) y **sí debe mantenerse al día**. Los archivos de CUN06 (`BE/Contrato577MC.cs`, `DAL/DALContrato577MC.cs`, `BLL/BLLContrato577MC.cs`, `FrmGenerarContrato.*`), de CUN07 (`BE/TerminosContrato577MC.cs`, `FrmRegistrarTerminosCondiciones.*`) y de CUN08 (`FrmActualizarEstadoPropiedadContrato.*`, métodos de estado en `DALContrato577MC`/`DALPropiedad577MC` y `BLLContrato577MC.ActualizarEstadoContrato`) ya están implementados y verificados; no reescribirlos sin pedido explícito.
5. **No exponer secretos** ni volcar cadenas de conexión completas en logs o documentación pública.
6. **Mantener el estilo**: sufijo `577MC`, identificadores en español, DAL que devuelve `DataTable`, BLL que mapea a entidades, formularios con `Designer.cs` + `.resx`.
7. **Toda afirmación relevante debe citar archivo/método/línea** cuando se documente.
8. Antes de un cambio funcional: compilar, reproducir, revisar impacto en DVH/DVV y bitácora, y actualizar este `AGENT.md` (y `docs/agent/` si existe).

## 4. Flujo de trabajo recomendado

1. Leer `docs/agent/ARCHITECTURE.md` y `docs/agent/PROJECT_MAP.md`.
2. Para UI: `docs/agent/WINFORMS_PATTERNS.md`.
3. Para datos: `docs/agent/DAL_PATTERNS.md`.
4. Para estado y bloqueos: `docs/agent/ANALYSIS_STATUS.md` y `docs/agent/KNOWN_ISSUES.md`.
5. Compilar con MSBuild 2022:
   ```powershell
   & "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" Servicios.sln /t:Build /p:Configuration=Debug /v:minimal
   ```
6. Verificar `git diff` antes y después. No commitear sin pedido.

## 5. Mapa rápido de documentos

| Documento | Contenido |
|---|---|
| `docs/agent/PROJECT_OVERVIEW.md` | Dominio, stack, proyectos, cómo ejecutar |
| `docs/agent/ARCHITECTURE.md` | Capas, dependencias, flujos transversales |
| `docs/agent/PROJECT_MAP.md` | Archivo por archivo, formularios y scripts SQL |
| `docs/agent/CODING_CONVENTIONS.md` | Nombres, estilo, manejo de errores, encoding |
| `docs/agent/WINFORMS_PATTERNS.md` | Patrones de formularios, navegación, i18n |
| `docs/agent/DAL_PATTERNS.md` | Acceso a datos, parámetros, DVH, transacciones |
| `docs/agent/DEPENDENCIES.md` | Referencias entre proyectos y externas |
| `docs/agent/KNOWN_ISSUES.md` | Defectos y riesgos verificados |
| `docs/agent/DECISIONS.md` | Decisiones arquitectónicas observadas (ADR) |
| `docs/agent/ANALYSIS_STATUS.md` | Cobertura, método, límites y próximos pasos |

> Nota: los 10 documentos existen y se actualizaron entre el 2026-10-28 y el 2026-10-30. La fuente de verdad sigue siendo este `AGENT.md` y el código; si hay contradicción, prevalece el código verificado.

## 6. Preguntas abiertas que requieren decisión humana

- ~~¿Se implementa CUN07 (registrar/`Cargar Acuerdos`) y en ese momento se bloquean los términos editables de CUN06?~~ **Resuelto:** CUN07 implementado como modal y los términos de CUN06 pasaron a solo lectura.
- ¿Se agrega el DDL de `Contrato` a `Scripts/scriptTerceraEntrega.sql` o se mantiene solo en la base?
- ¿Se corrige el script de creación de BD (`Visita`) o se parte de otra base?
- ¿La instancia `LAPTOP-8BNKG482\SQLEXPRESS` de respaldo/restore es la oficial?
- ¿La autorización debe validarse también dentro de cada formulario o alcanza con el menú?
- ¿Se normaliza el encoding de archivos con caracteres corruptos (`A3`, `A±`, etc.)?
- ¿Se agrega un acceso directo para abrir la carpeta de descargas o el PDF recién generado? Hoy se informa la ruta y se abre la vista previa, pero no se lanza el visor externo.
- ~~¿Se implementa la **regla de operabilidad RF2** (generar contratos solo sobre propiedades `Disponible`/`Reservada`)?~~ **Resuelto el 2026-11-02:** implementada (sección 12).
- ¿Se agrega **Matrícula Dominial** a `Propiedad`? El RF2 la pide, pero hoy no existe en el esquema ni en el selector (sección 12).

## 7. CUN06 — Generar contrato (implementado y verificado 2026-10-28)

### Flujo funcional

1. Menú **Contratos → Generar Contrato** (`MenuPrincipal.generarContratoToolStripMenuItem_Click`). Se habilita con permiso `Gestion Clientes` **o** `Seleccionar Propiedad`.
2. `FrmGenerarContrato` solicita DNI del propietario y DNI del cliente/inquilino.
3. Escenario alternativo: si un DNI no existe, se avisa, se abre el formulario existente `RegistrarCliente(dni)` y, tras el alta, se puede continuar.
4. La propiedad se elige reutilizando `SeleccionarPropiedad` (CUN02) y se muestra en solo lectura: código, dirección, tipo, estado, precio, superficie, ambientes, dormitorios y baños. **Hoy el selector muestra todas las propiedades**, incluidas `Vendida`/`Alquilada`; la regla de operabilidad que lo restringe está pendiente (sección 12).
5. Los términos (`TipoContrato`, `FechaInicio`, `FechaFin`, `MontoAcordado`, `FormaPago`, `DepositoGarantia`) se cargan en el modal **CUN07** (`btnTerminosCondiciones`, texto **“Términos y Condiciones”**). Al confirmar, CUN06 los muestra en solo lectura; si el modal se cancela, se conservan los últimos confirmados.
6. `BLLContrato577MC.ValidarYGenerarContrato` valida (incluida la validación única de términos), numera, calcula DVH, inserta, actualiza DVV y registra bitácora.
7. Mensaje de éxito exacto: **`Contrato generado con éxito (borrador emitido)`**.

### Reglas de negocio (BLL)

- Ambos DNI deben existir en `Cliente` (no en `Usuario`).
- La propiedad debe existir en `Propiedad`.
- `TipoContrato` y `FormaPago` obligatorios (validados por `BLLContrato577MC.ValidarTerminos`, reutilizada por `ValidarContrato`).
- `FechaInicio`/`FechaFin` obligatorias y `FechaFin > FechaInicio`.
- `MontoAcordado > 0`; `DepositoGarantia > 0`.
- `Estado` siempre se fuerza a `"Pendiente"` (borrador).

### Numeración

- Formato `C-YYYYMMDD-NNNN` (ejemplo real verificado: `C-20261028-0001`).
- Secuencial diario en `DALContrato577MC.ObtenerSiguienteSecuencial`, usando `ISNULL(MAX(TRY_CAST(SUBSTRING(NroContrato, 12, 4) AS int)), 0) + 1` sobre el prefijo del día.
- `BLLContrato577MC` reintenta hasta 5 veces si el INSERT choca con duplicado (`SqlException` 2627/2601).

### Esquema `Contrato` (verificado en la base)

| Columna | Tipo | Nulo |
|---|---|---|
| `NroContrato` | `varchar(20)` PK | NO |
| `FechaEmision` | `date` | NO |
| `DNIPropietario` | `varchar(50)` FK → `Cliente(DNI)` | NO |
| `DNICliente` | `varchar(50)` FK → `Cliente(DNI)` | NO |
| `CodigoPropiedad` | `int` FK → `Propiedad(Id)` | NO |
| `TipoContrato` | `varchar(50)` | NO |
| `FechaInicio` | `date` | NO |
| `FechaFin` | `date` | NO |
| `MontoAcordado` | `decimal(18,2)` | NO |
| `FormaPago` | `varchar(50)` | NO |
| `DepositoGarantia` | `decimal(18,2)` | NO |
| `Estado` | `varchar(20)` (default `'Pendiente'`) | NO |
| `DVH` | `bigint` | SÍ |

- PK: `PK_Contrato`. FKs: `FK_Contrato_Propietario`, `FK_Contrato_Cliente`, `FK_Contrato_Propiedad`.
- La tabla **no** está en `Scripts/scriptTerceraEntrega.sql`; fue creada manualmente. Si se recrea la base, hay que incluirla.

### DVH/DVV

- Cadena del DVH: `NroContrato + FechaEmision(yyyyMMdd) + DNIPropietario + DNICliente + CodigoPropiedad + TipoContrato + FechaInicio(yyyyMMdd) + FechaFin(yyyyMMdd) + MontoAcordado(0.00) + FormaPago + DepositoGarantia(0.00) + Estado`.
- `Services.DigitoVerificador577MC.CalcularDVHContrato` (sobrecarga por parámetros y por `DataRow`).
- La fila de control se guarda en `DigitoVerificador` con `NombreTabla = 'Contrato'`.
- Si la fila de control no existe, la primera verificación la crea automáticamente y devuelve consistente (`VerificarTablaConDatos`).
- `Login` y `RepararInconsistencias` incluyen `Contrato`; el mensaje de reparación lista `- Contrato`.

### Archivos modificados/creados

- Nuevos CUN06: `BE/Contrato577MC.cs`, `DAL/DALContrato577MC.cs`, `BLL/BLLContrato577MC.cs`, `FrmGenerarContrato.cs`, `FrmGenerarContrato.Designer.cs`.
- Nuevos CUN07: `BE/TerminosContrato577MC.cs`, `FrmRegistrarTerminosCondiciones.cs`, `FrmRegistrarTerminosCondiciones.Designer.cs`.
- Modificados: `BE/Enum/Modulos577MC.cs`, `BE/BE_577MC.csproj`, `BLL/BLL_577MC.csproj`, `DAL/DALDigitoVerificador577MC.cs`, `DAL/DALPropiedad577MC.cs`, `Services/DigitoVerificador577MC.cs`, `Login.cs`, `RepararInconsistencias.cs`, `MenuPrincipal.cs`, `MenuPrincipal.Designer.cs`, `GUI_577MC.csproj`.
- Codificación: los archivos nuevos/modificados se guardaron como UTF-8 con BOM para preservar acentos.

### Evidencia de verificación

- Prueba end-to-end contra la base real (harness temporal ya eliminado): clientes/propiedad existentes e inexistentes, rechazos de validación, dos altas reales (`0001` y `0002`), `VerificarContrato()` consistente, detección de manipulación de `MontoAcordado` y verificación final de las 5 tablas (`Usuario`, `Rol`, `Familia`, `Patente`, `Contrato`) en `True`.
- Los contratos de prueba y sus bitácoras se borraron; `Contrato` quedó vacía y la fila de control quedó con `DVV = 0`.
- Segunda ronda (2026-10-28, cierre de CUN06/CUN07):
  - `ValidarTerminos`: válido, nulo, tipo/forma vacíos, fechas invertidas o iguales, monto 0/negativo, depósito 0/negativo y recorte de espacios → 11/11 OK.
  - Instanciación de ambos formularios validada (Designer sin errores): controles, tamaños, colores, botón “Términos y Condiciones”, resumen en solo lectura, combos vacíos al abrir y precarga al reabrir.
  - Flujo GUI real: el modal inválido (vacíos, fechas invertidas, monto 0, texto no numérico) muestra el mensaje y **no** cierra; el caso válido cierra y devuelve `Terminos`; CUN06 bloquea la generación sin términos y, con términos, persiste el contrato con DVH correcto y limpia el formulario.
  - Contratos y eventos de bitácora de prueba eliminados; `Contrato` quedó vacía, `DVV = 0` y `DVH` de control restaurado (`5757155124328012617`).
- Instanciación de `FrmGenerarContrato` validada (Designer sin errores).
- Compilación final: `GUI_577MC -> bin\Debug\Servicios.exe`.

### Pendientes / próximos pasos

- Probar manualmente en la GUI el escenario "DNI inexistente → alta → continuar" y el recorrido completo `Generar Contrato → Términos y Condiciones → Generar Contrato`.
- Evaluar agregar el DDL de `Contrato` al script de entrega.
- [RECOMENDACIÓN] La bitácora se registra después del INSERT y fuera de transacción: si el log falla (por ejemplo, DNI de usuario inexistente), el contrato queda insertado y el usuario ve error. En uso normal siempre hay un usuario logueado, así que no se reproduce.
- No commitear sin pedido explícito.

## 8. CUN07 — Registrar términos y condiciones (implementado y verificado 2026-10-28)

### Flujo funcional

1. Se abre como **modal** (`ShowDialog`) desde `FrmGenerarContrato.btnTerminosCondiciones_Click`, pasando el último `TerminosContrato577MC` confirmado (o `null` en la primera apertura).
2. Campos del modal: `cboTipoContrato` (Venta / Alquiler), `cboFormaPago` (Contado / Financiado / Cuotas), `dtpFechaInicio`, `dtpFechaFin`, `txtMontoAcordado`, `txtDepositoGarantia`.
3. **Confirmar Términos** parsea los montos con la cultura actual (o invariante) y llama a `BLLContrato577MC.ValidarTerminos`.
   - Válido: carga `Terminos`, asigna `DialogResult = OK`, cierra y **no muestra mensaje**.
   - Inválido: muestra exactamente `Debe completar todos los términos comerciales correctamente` y **no cierra**.
4. **Cancelar**: `DialogResult.Cancel`; CUN06 conserva los últimos términos confirmados.
5. Al reabrir, el modal precarga lo último confirmado; la primera apertura arranca con combos sin selección y montos vacíos (fechas por defecto: hoy y hoy + 1 mes).

### Reglas de negocio

- La validación vive en `BLLContrato577MC.ValidarTerminos` y es reutilizada por `ValidarContrato`: un único punto de verdad.
- CUN07 **no persiste** ni usa DAL: solo valida en memoria y devuelve el DTO.
- El DTO es `BE/TerminosContrato577MC.cs` (los seis términos comerciales); no se reutiliza `Contrato577MC` parcialmente cargado.

### Controles y textos

- Título del formulario y del grupo: “Términos y Condiciones”.
- Botones: “Confirmar Términos” (Aceptar) y “Cancelar” (Cancelar).
- `FormBorderStyle.FixedDialog`, `CenterParent`, 560x400, sin maximizar/minimizar.

## 9. CUN08 — Actualizar estado de propiedad y contrato (implementado y verificado 2026-10-28)

### Flujo funcional

1. Menú **Contratos → Actualizar Estado de Propiedad y Contrato** (`MenuPrincipal.actualizarEstadoContratoToolStripMenuItem_Click`). Mismo permiso que CUN06: `Gestion Clientes` **o** `Seleccionar Propiedad`.
2. `FrmActualizarEstadoPropiedadContrato` carga automáticamente la grilla con los contratos en estado `Pendiente` (`dgvContratos`, solo lectura, selección por fila).
3. Columnas de la grilla: Nro. Contrato, Emisión, DNI Cliente, **Dirección Propiedad**, Tipo Contrato, Inicio, Fin, Monto y Estado. La dirección llega por `LEFT JOIN Propiedad` (`DALContrato577MC.ObtenerPendientes`).
4. **Modificar Estado**: carga el detalle del contrato seleccionado en campos de solo lectura y habilita el combo **Nuevo estado**.
5. El combo solo ofrece **Firmado** y **Cancelado**. **Guardar** permanece deshabilitado hasta elegir una opción.
6. Al guardar, la BLL aplica la regla. Si la decisión es **Firmado**, el formulario genera y previsualiza el RN2 real; luego recarga la grilla, limpia el detalle y deja el combo deshabilitado.
7. Mensajes de éxito:
   - Firmado: `Contrato firmado con éxito. La copia final se guardó en:\n<ruta>` (seguido de la vista previa). El PDF queda en `Downloads\Propiedades Calella\Contratos\RN2_<NroContrato>.pdf`.
   - Cancelado: `Contrato cancelado con éxito. La propiedad mantiene su disponibilidad.`
   - Si el PDF falla, el contrato **ya quedó firmado** y solo se informa el error (`No se pudo generar la copia final del contrato: ...`).

### Reglas de negocio (BLL)

- `BLLContrato577MC.ActualizarEstadoContrato(nroContrato, decision)`:
  - `decision` se recorta y debe ser `Firmado` o `Cancelado` → si no, `Debe seleccionar el nuevo estado del contrato.`
  - El contrato debe existir y seguir en `Pendiente` → si no, error explícito.
  - **Cancelado**: solo actualiza el contrato (`Estado = 'Cancelado'`, DVH recalculado, DVV de `Contrato` y bitácora). La propiedad no se toca.
  - **Firmado**: valida propiedad existente y estado en `Disponible`/`Reservada`. `Alquilada` o `Vendida` rechazan la firma sin modificar nada.
  - Tipo de contrato: `Alquiler → Alquilada`, `Venta → Vendida`. Cualquier otro tipo (por ejemplo un `Comodato` viejo) se rechaza con `El tipo de contrato '<tipo>' no permite actualizar el estado de la propiedad.`
- La firma ejecuta **contrato + propiedad en una única transacción SQL real** (`SET XACT_ABORT ON` + `BEGIN TRAN` + `THROW` + `ROLLBACK` en `DALContrato577MC.ActualizarEstadoConPropiedad`); los `UPDATE` individuales (`ActualizarEstado`) se conservan para la cancelación.
- Tras una firma exitosa se registran dos eventos de bitácora: uno del módulo `Contrato` y otro del módulo `Propiedades`.

### DVH/DVV

- Contrato: se recalcula con `Services.DigitoVerificador577MC.CalcularDVHContrato` y luego se actualiza `DigitoVerificador` para `Contrato`.
- Propiedad: **no tiene fila de control DVV**; solo se recalcula su `DVH` con `Services.DigitoVerificador577MC.CalcularDVHPropiedad` (cadena: `Id + Direccion + Tipo + Estado + Precio(0.00) + SuperficieM2(0.00) + Ambientes + Dormitorios + Banios`).
- `CalcularDVHPropiedad` está centralizado en `Services.DigitoVerificador577MC` y lo reutiliza `BLLPropiedad577MC`, para que alta, modificación y firma usen la misma cadena.
- `CalcularDVV` suma `long`; **no** usar `SUM(CAST(DVH AS bigint))` en SQL para verificar, porque la suma puede desbordar `bigint`.

### Archivos de CUN08

- Nuevos: `FrmActualizarEstadoPropiedadContrato.cs`, `FrmActualizarEstadoPropiedadContrato.Designer.cs`.
- `BE/Contrato577MC.cs`: campo de presentación `PropiedadDireccion` (no persistido).
- `DAL/DALContrato577MC.cs`: `ObtenerPendientes`, `ActualizarEstado`, `ActualizarEstadoConPropiedad`.
- `DAL/DALPropiedad577MC.cs`: `ObtenerPorId`, `ActualizarEstado`.
- `BLL/BLLContrato577MC.cs`: `ObtenerContratosPendientes`, `ActualizarEstadoContrato`, `FirmarContrato`, `CancelarContrato`, mapeos y bitácora por módulo.
- `Services/DigitoVerificador577MC.cs`: `CalcularDVHPropiedad` y `CalcularDVV`.
- `BLL/BLLPropiedad577MC.cs`: reutiliza el cálculo centralizado de DVH.
- `FrmRegistrarTerminosCondiciones.cs`: se quitó `Comodato` del combo de CUN07.
- `MenuPrincipal.cs` / `MenuPrincipal.Designer.cs` / `GUI_577MC.csproj`: ítem de menú, permiso y alta de los formularios.

### Evidencia de verificación

- **BLL/DAL contra la base real: 34/34 OK.** Propiedades y contratos de prueba creados con la propia BLL y borrados al final; no se usaron propiedades del usuario.
  - Firma `Alquiler` → contrato `Firmado`, DVH correcto, propiedad `Alquilada`, DVH de propiedad correcto, DVV/DVH de control consistentes, 3 eventos de bitácora (alta + contrato + propiedad).
  - Firma `Venta` → propiedad `Vendida`; `Reservada` también se puede firmar.
  - Cancelación → contrato `Cancelado`, DVH recalculado, propiedad intacta y fuera de la grilla de pendientes.
  - Rechazos: contrato ya no pendiente, contrato inexistente, decisión inválida/vacía/en blanco, `Comodato`, propiedad `Alquilada` (sin modificar contrato ni propiedad).
- **GUI real: 29/29 OK** (formulario instanciado y mostrado, clics simulados con `PerformClick`, `MessageBox` cerrados con temporizador + `SendKeys`).
  - Título, combo con 2 opciones, grilla con columna Dirección Propiedad, Guardar deshabilitado al inicio.
  - `Modificar Estado` carga el detalle y habilita el combo; `Guardar` se habilita al elegir.
  - Flujo `Cancelado` y flujo `Firmado` (Venta) completos, con mensaje de confirmación y recarga de la grilla.
- **Base restaurada al cierre de esa verificación (histórico, 2026-10-28)**: 1 contrato (`C-20261028-0001`), `DVV = 8073176779485644170`, `DVH` de control `318395726382682900`, `MaxBit = 509`, propiedades 9/10/11/14 en `Disponible` con `DVH = NULL`. El estado vigente es el de la sección 2.

### Pendientes

- [RECOMENDACIÓN] Evaluar si la bitácora de la firma debe entrar en la misma transacción que los `UPDATE` (hoy se registra después del `COMMIT`).
- [RECOMENDACIÓN] Probar manualmente el cierre de la vista previa después de firmar. La generación del PDF y la firma se verificaron; la automatización del diálogo modal de vista previa quedó bloqueada y debió cerrarse el proceso de prueba.

## 10. Reportes PDF — RN1 y RN2 (rediseñados y verificados 2026-10-28)

### Identidad visual compartida

- Marca visible: **Propiedades Calella** (encabezado y pie). El sufijo `577MC` queda solo en el código, nunca en el PDF.
- Encabezado: franja azul de 7 pt en el borde superior, marca en azul `#2B4C7E` (26 pt), subtítulo “Gestión Inmobiliaria”, logo a la derecha (máx. 150×48) y doble regla (2 pt azul + 1 pt gris).
- Paleta: azul `#2B4C7E`, oscuro `#1F2430`, gris `#6B7280`, gris claro `#9CA3AF`, línea `#D6DDE6`, papel `#F7F9FC`, azul suave `#E9EFF7`, verde `#208458`, naranja `#C47414` y rojo `#B0362E` con sus fondos suaves.
- Tipografía **Segoe UI** (fallback automático a Verdana si no está instalada); títulos 20 pt, valores 10,5–14 pt y etiquetas 7,2 pt en mayúsculas.
- Secciones numeradas `01`, `02`, … con barra azul de acento y hairline hasta el margen derecho.
- Tarjetas redondeadas (`Papel` con borde `Línea`), píldoras de estado, avatares con iniciales y *chips*.
- A4 vertical (595×842 pt) renderizado a 200 DPI y embebido como JPEG calidad 95 en un PDF 1.4.
- Pie en tres columnas: “Generado el dd/mm/yyyy HH:mm”, “Página 1 de 1” y “Propiedades Calella” sobre hairline azul.
- **Vista previa fiel:** `MostrarVistaPrevia` renderiza el mismo bitmap que el PDF y lo escala a la hoja, así que lo que se ve es lo que se imprime.

### RN1 — Comprobante de visita

- Clase: `ReporteRN1577MC` (`Emitir` / `GuardarPdf`).
- Título “Comprobante de Visita Pactada” y píldora con `RN1-<IdVisita:D4>`.
- Tarjeta principal: avatar con iniciales, nombre y DNI del cliente, más dos tiles grandes de **fecha** y **horario**.
- `01 Propiedad`: tarjeta con ícono de pin y dirección.
- `02 Estado`: línea de tiempo **Pactada → Confirmada → Realizada** (nodo activo según `Visita.Estado`; `Cancelada` se muestra en rojo).
- `03 Datos de la visita`: nota “Este comprobante acredita la visita pactada…” en panel con acento.
- `04 Recomendaciones`: cuatro ítems con tilde (presentar comprobante, llegar 10 minutos antes, traer DNI, avisar reprogramaciones).
- **Código de barras Code 39** real con `RN1-<IdVisita>` y la etiqueta “Código de verificación”.
- Salida: `Downloads\Propiedades Calella\Comprobantes\RN1_<IdVisita>.pdf`.
- Se emite desde `ProgramarVisita.btnReporte_Click` → `MostrarComprobanteRN1` (`ProgramarVisita.cs:233-257`); usa la última visita confirmada y resuelve el cliente con `BLLCliente577MC.ObtenerPorDNI` si no está en memoria.

### RN2 — Copia final del contrato

- Clase: `ReporteRN2577MC` (`Emitir(contrato)`, `Emitir(contrato, esFirma)` y `GuardarPdf(contrato)`).
- Título según el tipo: “Contrato de Venta” o “Contrato de Alquiler”; píldora con el número y “Fecha de emisión” alineada a la derecha.
- `01 Partes intervinientes`: dos columnas con avatar, nombre, DNI y rol (`PROPIETARIO` / `CLIENTE`), separadas por un divisor.
- `02 Propiedad`: dirección destacada con pin y *chips* de tipo, superficie, ambientes, dormitorios, baños y código.
- `03 Términos y condiciones`: grilla con tipo, fechas, forma de pago y depósito; el **monto acordado** se destaca en azul a 13,5 pt.
- Estado en píldora (`FIRMADO` verde, `CANCELADO` rojo) + **código de barras Code 39** con el NroContrato.
- Frase de cierre aprobada: “Las partes declaran haber leído y aceptado los términos y condiciones del presente contrato, y firman en señal de conformidad en la fecha de emisión indicada.”
- Firmas en tarjeta: línea, rol, nombre y DNI de propietario y cliente, más “Por Propiedades Calella”.
- Salida: `Downloads\Propiedades Calella\Contratos\RN2_<NroContrato>.pdf`.
- Los datos de nombres y propiedad se completan con `BLLCliente577MC.ObtenerPorDNI` y `BLLPropiedad577MC.ObtenerPorId`; si faltan, se muestra el DNI o `-`.

### Motor compartido, logo y código de barras

- `ReportePdf577MC` concentra: render A4, fuentes, formatos (`es-AR`, moneda `C2`, fecha `dd/MM/yyyy`), encabezado/pie/secciones, paneles redondeados, píldoras, chips, avatares, checks, notas, código de barras, armado del PDF y carpeta de descargas.
- **Code 39** implementado sin librerías externas (`DibujarCodigoBarras` / `DibujarBloqueCodigoBarras`), con tabla de patrones y ancho calculado para centrar el bloque.
- Textos largos: `DibujarTextoAjustado` reduce el tamaño y, si aún no entra, recorta con elipsis.
- Logo: `Recursos\logo-propiedades-calella.jpg` (1020×323 px). Se resuelve desde `AppDomain.CurrentDomain.BaseDirectory\Recursos` con fallback a `..\..\Recursos`; el `.csproj` lo copia al output (`CopyToOutputDirectory = Always`).
- Si el logo no existe o no se puede leer, el reporte se genera igual sin logo.

### Reimpresión

- Menú **Contratos → Reimprimir Copia Final** (`MenuPrincipal.reimprimirContratoToolStripMenuItem_Click`), mismo permiso que CUN08 (`Gestion Clientes` o `Seleccionar Propiedad`).
- `FrmReimprimirContrato` lista contratos `Firmado` (`DALContrato577MC.ObtenerFirmados` → `BLLContrato577MC.ObtenerContratosFirmados`) con Nro., emisión, DNI cliente, dirección, tipo, monto y estado.
- Botón **Generar PDF** y doble clic en la fila llaman a `ReporteRN2577MC.Emitir(contrato, false)`.
- Sin cambios de esquema: solo lecturas.

### Archivos de los reportes

- Nuevos: `ReportePdf577MC.cs`, `ReporteRN2577MC.cs`, `FrmReimprimirContrato.cs`, `FrmReimprimirContrato.Designer.cs`.
- Modificados: `ReporteRN1577MC.cs` (rediseño completo), `FrmActualizarEstadoPropiedadContrato.cs` (emite RN2 al firmar), `MenuPrincipal.cs` / `MenuPrincipal.Designer.cs` (ítem de reimpresión), `GUI_577MC.csproj` (altas y logo), `Lenguajes/es.json` / `Lenguajes/en.json` (`Reporte.*`, `ReporteRN1.*`, `ReporteRN2.*` y textos de menú; **459 claves** cada uno), `DAL/DALContrato577MC.cs` (`ObtenerFirmados`), `BLL/BLLContrato577MC.cs` (`ObtenerContratosFirmados`), `BLL/BLLPropiedad577MC.cs` (`ObtenerPorId`).
- `Recursos/logo-propiedades-calella.jpg` reemplaza al logo viejo (que ya no existe).

### Evidencia de verificación

- Compilación Debug completa: `GUI_577MC -> bin\Debug\Servicios.exe`, sin errores nuevos (solo warnings `CS2002` preexistentes).
- RN1 y RN2 generados con datos reales y extraídos a imagen para revisión visual: franja y encabezado, tarjetas, píldoras, avatar, timeline, chips, nota, recomendaciones, código de barras Code 39, cierre y firmas correctos; sin desbordes de texto.
- Ajustes posteriores al primer render: nota con centrado vertical, etiqueta “Estado” en RN2 y altura de la tarjeta de términos.
- Reimpresión (solo lectura, 2026-10-28, con 2 contratos `Firmado`): 5/5 OK. La grilla mostró 2 filas, el contrato estaba `Firmado` y se generó el PDF de 366 KB.
- Al 2026-10-30 hay 3 contratos `Firmado` (`0001`, `0002` y `0005`); la reimpresión los listaría a los tres.
- La base ya no está en el estado de la verificación anterior: las pruebas manuales del usuario firmaron `C-20261028-0001` (propiedad 14 `Alquilada`), `C-20261028-0002` (propiedad 11 `Vendida`) y `C-20261028-0005` (propiedad 9 `Alquilada`), cancelaron `0003` y dejaron `0004` `Pendiente`. **El agente no modificó datos**: las muestras de PDF se generaron solo con lecturas. Detalle vigente en la sección 12.

## 11. Convención visual

Aplicada a CUN06/CUN07/CUN08 y **obligatoria para los próximos casos de uso**:

| Elemento | Valor |
|---|---|
| Fondo del formulario | `Color.LightSteelBlue` |
| Título | Verdana 14F Bold |
| Labels | Verdana 9F Bold |
| Texto de GroupBox | Verdana 10F Bold |
| Inputs (texto, combo, fecha) | Verdana 9F Regular |
| Botones | `Color.SteelBlue`, texto `SystemColors.HighlightText`, Verdana 9F/10F Bold, `UseVisualStyleBackColor = false` |
| Campos de solo lectura | `ReadOnly = true`, `BackColor = White`, `TabStop = false` |

- Layout base de CUN06: 760x646, con group boxes de clientes, propiedad y términos, y resumen de términos en solo lectura.
- Layout de CUN08: 1020x620, con group boxes “Contratos pendientes” (grilla), “Contrato seleccionado” (detalle en solo lectura) y “Nuevo estado” (combo), más los botones “Modificar Estado” y “Guardar”.
- Layout de reimpresión: 1020x460, group box “Contratos firmados” con grilla y botón “Generar PDF”; doble clic en la fila también genera.
- Los formularios nuevos mantienen `Designer.cs` + `.resx` cuando corresponde y el sufijo `577MC` en la lógica de negocio.
- Los reportes (`ReportePdf577MC`, `ReporteRN1577MC`, `ReporteRN2577MC`) no son formularios: son clases estáticas de generación y no llevan Designer.

## 12. Estado de datos y pendientes al 2026-10-30

### Base real (solo lectura; el agente no modificó datos)

| Contrato | Tipo | Estado | Propiedad | Estado propiedad |
|---|---|---|---|---|
| `C-20261028-0001` | Alquiler | `Firmado` | 14 — Int. Amenedo 148 | `Alquilada` |
| `C-20261028-0002` | Venta | `Firmado` | 11 — Boulevard Haedo 300 | `Vendida` |
| `C-20261028-0003` | Venta | `Cancelado` | 11 — Boulevard Haedo 300 | `Vendida` |
| `C-20261028-0004` | Venta | `Pendiente` | 11 — Boulevard Haedo 300 | `Vendida` |
| `C-20261028-0005` | Alquiler | `Firmado` | 9 — Av. San Martín 450 | `Alquilada` |

- Propiedad 10 — Calle 9 de Julio 820: `Disponible` (única operable al 2026-10-30).
- `MaxBit = 648`; `DVV Contrato = -643719723159986574`; la suma de los `DVH` de los 5 contratos coincide con el DVV (verificado con aritmética de 64 bits).
- `C-20261028-0004` queda `Pendiente` por decisión del usuario. Intentar firmarlo devuelve `No se puede firmar el contrato C-20261028-0004: la propiedad 11 - 'Boulevard Haedo 300' se encuentra en estado 'Vendida'. Solo se puede firmar sobre una propiedad 'Disponible' o 'Reservada'.` No bloquea otras propiedades ni la reimpresión.

### Implementada: regla de operabilidad RF2 (2026-11-02)

**Situación detectada (2026-10-30):** `SeleccionarPropiedad` en modo selección cargaba todas las propiedades (`ObtenerTodas`) y `BLLContrato577MC.ValidarContrato` no validaba el estado antes del INSERT. Por eso se podían generar contratos `Pendiente` sobre propiedades `Vendida`/`Alquilada` que después no se podían firmar.

**Regla vigente:** solo se genera un contrato si la propiedad está `Disponible` o `Reservada` **y** no tiene otro contrato `Pendiente`.

1. `SeleccionarPropiedad(bool modoAbm = false, bool soloOperables = false)`: con `soloOperables = true` el catálogo, el filtro de estado y la validación de selección quedan limitados a `Disponible`/`Reservada`. El ABM (`modoAbm`) y el selector de visitas conservan el catálogo completo.
2. `FrmGenerarContrato` abre el selector con `soloOperables: true`, revalida al seleccionar y limpia la selección si la propiedad deja de ser operable.
3. `BLLContrato577MC.ValidarOperabilidadPropiedad(int)` relee la propiedad y rechaza la generación si no es operable o si `DALContrato577MC.ObtenerContratoPendiente(id)` devuelve un contrato pendiente. Se invoca desde `ValidarContrato` y desde `FrmGenerarContrato.btnGenerarContrato_Click` antes de registrar clientes.
4. `FirmarContrato` acepta `Disponible`/`Reservada` (`EsEstadoOperable`) y el mensaje de rechazo incluye `NroContrato`, propiedad, dirección y estado.
5. Sin cambios de esquema ni de DVH/DVV.

**Estado:** implementado y compilado el 2026-11-02 (`Servicios.exe`, solo `CS2002`).

### Gap RF2: Matrícula Dominial

- El RF2 pide mostrar `Matrícula Dominial` al seleccionar la propiedad.
- La tabla `Propiedad` solo tiene `Id, Direccion, Tipo, Estado, Precio, SuperficieM2, Ambientes, Dormitorios, Banios, DVH`; el selector muestra Id, Dirección, Tipo, Estado y Precio.
- Agregarla requiere cambio de esquema, BE/DAL/BLL, ABM y recalcular el DVH de propiedad. No forma parte de la corrección de operabilidad.

### Verificación al 2026-10-30

- Build Debug con MSBuild 2022: `GUI_577MC -> bin\Debug\Servicios.exe`, sin errores; solo `CS2002`.
- `es.json` y `en.json`: 459 claves cada uno, mismos conjuntos.
- RN1 y RN2 revisados visualmente; reimpresión 5/5 (2026-10-28).
- Todo sigue **sin commitear** (`AGENT.md` incluido, sin seguimiento en Git).
