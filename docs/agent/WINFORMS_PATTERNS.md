# WINFORMS_PATTERNS — Patrones de UI

## 1. Estructura de un formulario

Todo formulario sigue el patrón WinForms clásico:

- `X.cs`: lógica y manejadores de eventos.
- `X.Designer.cs`: `InitializeComponent()`, declaración de controles, layout.
- `X.resx`: recursos del diseñador.

No hay user controls, MVP, MVVM ni binding a ViewModels. El code-behind concentra validación, navegación y llamadas a BLL.

## 2. Constructor típico

```csharp
public ProgramarVisita()
{
    InitializeComponent();
    ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
    actualizarIdioma();
}
```

Variantes:

- **Suscripción + desuscripción** (correcta):
  ```csharp
  ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
  FormClosed += (s, e) => ServiceSessionManager577MC.getIntancia().Idioma.Desuscribir(this);
  ```
  Presente en `SeleccionarPropiedad`, `FrmGenerarContrato`, `FrmActualizarEstadoPropiedadContrato` y `FrmReimprimirContrato`. Los dos formularios nuevos de contratos se desuscriben en `FormClosed`.
- **Solo suscripción** (mayoría): `Login`, `MenuPrincipal`, `RegistrarCliente`, `ProgramarVisita`, `ActualizarEstadoVisita`, `GestionUsuario`, `GestionRol`, `GestionFamilia`, `AuditoriaBitacora`, `RepararInconsistencias`, `CambiarContraseña`, `ConsultarDisponibilidad`. Genera referencias a formularios cerrados y notificaciones innecesarias.
- **Sin observer**: `Configuracion_Inicial`.

## 3. Navegación

### 3.1 Login → menú / cambio de clave / reparación

`Login.btnLogin_Click` (`Login.cs:39`):

1. `_userService.login(username, password)` → `bool usaPasswordDefault`.
2. Carga idioma según `usuarioActivo.IdIdioma` (1 = `es`, otro = `en`).
3. `DigitoVerificador577MC.VerificarIntegridad()`.
4. Si hay inconsistencias y el rol no es 1 → mensaje, `Logout()` y return.
5. Si es rol 1 → oculta Login y abre `RepararInconsistencias` (modalidad no modal).
6. Si la clave es default → `CambiarContraseña`; si no → `MenuPrincipal`.
7. `FormClosed` de la pantalla abierta vuelve a mostrar `Login` (`RestaurarIdiomaLogin`).

No hay cierre de la aplicación al cerrar el menú; `MenuPrincipal.OnFormClosing` (`MenuPrincipal.cs:126`) maneja el cierre.

### 3.2 Menú → formularios

- `MenuPrincipal.configurarAcceso()` (`líneas 75-95`) habilita ítems según `TienePermiso("...")`.
- Los handlers hacen `new Form(); form.Show();` (no modal), salvo selectores.
- Algunos handlers vuelven a chequear el permiso y muestran "acceso denegado" (`MenuPrincipal.cs:211-218`, `225-232`, `239-246`, `253-260`, `267-274`, y los tres de contratos).
- Submenú **Contratos**: `Generar Contrato`, `Actualizar Estado de Propiedad y Contrato` y `Reimprimir Copia Final` (este último abre `FrmReimprimirContrato`).
- **No hay verificación de permisos dentro de los formularios.** Si se abre por otra vía (por ejemplo, desde `ProgramarVisita`), no se valida.

### 3.3 Selectores modales

Patrón:

```csharp
using (SeleccionarPropiedad form = new SeleccionarPropiedad())
{
    if (form.ShowDialog() == DialogResult.OK && form.PropiedadSeleccionada != null)
    {
        _propiedad = form.PropiedadSeleccionada;
        ...
    }
}
```

- `ProgramarVisita.btnSeleccionarPropiedad_Click` (`ProgramarVisita.cs:128`).
- `FrmGenerarContrato.btnSeleccionarPropiedad_Click` usa `new SeleccionarPropiedad(false)`. **Pendiente (2026-10-30):** agregar el flag `soloOperables` para mostrar solo propiedades `Disponible`/`Reservada` al generar contratos (`AGENT.md`, sección 12).
- `ProgramarVisita.AbrirAltaCliente` (`líneas 87-105`) abre `RegistrarCliente(dni)` en modo modal y lee `ClienteRegistrado`.
- `ProgramarVisita.btnConsultarDisponibilidad_Click` abre `ConsultarDisponibilidad(_propiedad)` y copia fecha/horas/disponibilidad.

### 3.4 Formularios de doble modo

`RegistrarCliente`:

- Constructor sin parámetros → `_modoAbm = true`; muestra grilla, serialización y botones ABM (`RegistrarCliente.cs:25-37`).
- Constructor `RegistrarCliente(string dni)` → `_modoAbm = false`; oculta ABM/serialización, achica el formulario y precarga el DNI (`líneas 38-47`).
- En modo ABM, guardar recarga la grilla y limpia; en modo modal, setea `DialogResult.OK` y cierra (`líneas 78-91`).

`SeleccionarPropiedad`:

- Constructor `SeleccionarPropiedad(bool modoAbm = false)`.
- Modo `false`: catálogo + filtros + botón Seleccionar; formulario compacto.
- Modo `true`: muestra `gbABM`, oculta Seleccionar y carga combos de ABM (`SeleccionarPropiedad.cs:41-67`).
- **Pendiente (2026-10-30):** agregar `soloOperables` para que el modo selección de contratos filtre `Disponible`/`Reservada`; ABM y visitas conservan el catálogo completo.

`ConsultarDisponibilidad`:

- Recibe una `Propiedad577MC`; expone `FechaSeleccionada`, `HoraInicioSeleccionada`, `HoraFinSeleccionada`, `DisponibilidadConfirmada`.
- `btnAceptar_Click` solo habilita el OK si se consultó (`_consultado`) y hay disponibilidad (`ConsultarDisponibilidad.cs:141-158`).

## 4. Validación en UI

- Validaciones inline antes de llamar a BLL: campos vacíos, formato de DNI (7-8 dígitos), email con `MailAddress` o método propio, horas de inicio < fin, precios parseados con `decimal.TryParse`.
- Ejemplos: `RegistrarCliente.btnGuardar_Click` (`líneas 55-67`), `ProgramarVisita.ValidarDNI` (`líneas 107-120`), `GestionUsuario.EsEmailValido`/`EsDNIValido` (`líneas 289-299`).
- La BLL repite validaciones de existencia/duplicados; no hay un esquema central de validación.
- Los mensajes usan `MessageBox.Show(texto, titulo, botones, icono)`.

## 5. Grillas

- `DataGridView` con `DataSource` asignado a `List<Entidad>` o `DataTable`.
- Ejemplo: `RegistrarCliente.CargarClientes()` → `_clienteBLL.ObtenerTodos()` (`RegistrarCliente.cs:426-430`).
- Selección por `CellClick` o `SelectionChanged`; se lee `DataBoundItem` y se vuelca a los campos.
- `AuditoriaBitacora` usa `CellFormatting` para traducir criticidad/módulo y botones de impresión/PDF.
- No hay ordenamiento ni paginación del lado servidor.

## 6. Serialización XML

`RegistrarCliente` incluye un bloque de serialización/deserialización de clientes:

- `XmlSerializer(typeof(List<Cliente577MC>))`.
- `btnSerializar_Click` guarda selección en `.xml` con `SaveFileDialog` (`líneas 250-313`).
- `btnDeserializar_Click` levanta un `.xml` y muestra los clientes en un `ListBox` (`líneas 315-...`).
- Es una funcionalidad académica, no integrada al flujo de negocio.

## 7. Reportes

- `AuditoriaBitacora` usa `PrintDocument` + `PrintPreviewDialog`/`PrintDialog` para imprimir la grilla. El botón llamado `btnPDF` **no genera un PDF**: abre `PrintDialog` y manda el documento a la impresora (`AuditoriaBitacora.cs:194-204`).
- `ReportePdf577MC` es el motor compartido: renderiza A4 a 200 DPI con GDI+, lo comprime como JPEG calidad 95 y arma un PDF 1.4 sin librerías externas. Concentra encabezado con franja azul y logo, pie en tres columnas, secciones numeradas, paneles redondeados, píldoras, chips, avatares, notas, checks y código de barras Code 39.
- Tipografía **Segoe UI** con fallback automático a Verdana. Los textos largos se ajustan con `DibujarTextoAjustado` (reduce tamaño y recorta con elipsis si hace falta).
- `ReporteRN1577MC.Emitir(visita, cliente, direccion)` genera el comprobante de visita (hero con avatar y tiles, propiedad, timeline del estado, nota, recomendaciones y barcode) y `ReporteRN2577MC.Emitir(contrato)` la copia final (partes, propiedad con chips, términos con monto destacado, estado, barcode y firmas). Ambos usan la marca **Propiedades Calella** y el logo `Recursos\logo-propiedades-calella.jpg`.
- El logo se resuelve desde `AppDomain.CurrentDomain.BaseDirectory\Recursos` con fallback a `..\..\Recursos` y se copia al output desde el `.csproj`; si no existe, el reporte se genera sin logo.
- Salidas: `Downloads\Propiedades Calella\Comprobantes` (RN1) y `Downloads\Propiedades Calella\Contratos` (RN2).
- La vista previa usa `PrintDocument` + `PrintPreviewDialog`, pero renderiza el **mismo bitmap** que el PDF y lo escala a la hoja, así que es fiel al archivo. No hay visor PDF embebido ni se lanza el visor externo automáticamente.
- `FrmReimprimirContrato` permite regenerar el RN2 de contratos `Firmado` (botón o doble clic).

## 8. i18n en formularios

- Cada formulario implementa `public void actualizarIdioma()` y asigna `Text` a controles.
- `MenuPrincipal` reconstruye dinámicamente el submenú de idiomas (`CargarSubItemsIdioma`, `MenuPrincipal.cs:36`) y al hacer clic llama `CargarIdioma`.
- El cambio de idioma se persiste por usuario con `BLLUsuario577MC.GuardarIdioma` (`BLLUsuario577MC.cs:274`).
- Algunos formularios no traducen todo: `Configuracion_Inicial`, partes de `GestionRespaldo`, mensajes de `FrmGenerarContrato`.
- Los reportes traducen sus textos con `Reporte.*`, `ReporteRN1.*` y `ReporteRN2.*`; los formularios de CUN08 y reimpresión mantienen textos en español.
- El submenú de contratos sí se traduce en `MenuPrincipal.actualizarIdioma()` (`menuGenerarContrato`, `menuActualizarEstadoContrato`, `menuReimprimirContrato`).

## 9. Riesgos de UI verificados

| Riesgo | Evidencia | Impacto |
|---|---|---|
| Observadores no desuscriptos | solo 2 de 16 formularios se desuscriben | fuga de memoria y notificaciones a controles disposed |
| Permisos solo en el menú | `MenuPrincipal.configurarAcceso` | acceso por vías alternativas |
| `MessageBox` con `ex.Message` crudo | múltiples formularios | filtración de detalles SQL/stack |
| `Application.Restart()` tras restore | `GestionRespaldo.cs` | pierde estado y puede reiniciar en bucle si falla |
| ~~Ruta absoluta del logo~~ (resuelto) | logo portable desde `Recursos` + copia al output | ya no aplica |
| `Show()` sin `Owner` | menú y login | ventanas huérfanas si se cierra el padre |
| Textos corruptos | `A3`, `A±`, `A?` | UI con caracteres incorrectos |

## 10. Cómo agregar un formulario (receta)

1. Crear `NuevoForm.cs` y `NuevoForm.Designer.cs`; agregar `NuevoForm.resx` solo si el diseñador usa recursos.
2. Namespace `Servicios`; clase `public partial class NuevoForm : Form, IIdiomaObserver`.
3. En el constructor: `InitializeComponent()`, suscribir idioma, `actualizarIdioma()` y desuscribir en `FormClosed` si el formulario lo permite.
4. Implementar `actualizarIdioma()` con claves nuevas en `es.json` y `en.json`.
5. Incluir los archivos en `GUI_577MC.csproj` (`<Compile>` y, si hay `.resx`, `<EmbeddedResource>`).
6. Agregar el ítem de menú y su permiso en `MenuPrincipal` si aplica.
7. Seguir el patrón `try/catch` + `MessageBox` del resto.
8. No agregar lógica de datos en el formulario: llamar a una BLL.
