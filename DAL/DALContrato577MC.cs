using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALContrato577MC
    {
        DALAcceso577MC acceso = new DALAcceso577MC();

        public bool ExisteContrato(string nroContrato)
        {
            string query = "SELECT 1 FROM Contrato WHERE NroContrato = @nroContrato";

            var parametros = new Dictionary<string, object>
            {
                { "@nroContrato", nroContrato }
            };

            DataTable dt = acceso.executeDataTable(query, parametros);

            return dt.Rows.Count > 0;
        }

        public int ObtenerSiguienteSecuencial(DateTime fechaEmision)
        {
            string prefijo = "C-" + fechaEmision.ToString("yyyyMMdd") + "-";

            string query = @"SELECT ISNULL(MAX(TRY_CAST(SUBSTRING(NroContrato, 12, 4) AS int)), 0) + 1
                             FROM Contrato
                             WHERE NroContrato LIKE @prefijo + '%'";

            var parametros = new Dictionary<string, object>
            {
                { "@prefijo", prefijo }
            };

            object resultado = acceso.executeScalar(query, parametros);

            return Convert.ToInt32(resultado);
        }

        public int InsertarContrato(string nroContrato, DateTime fechaEmision, string dniPropietario, string dniCliente,
            int codigoPropiedad, string tipoContrato, DateTime fechaInicio, DateTime fechaFin, decimal montoAcordado,
            string formaPago, decimal depositoGarantia, string estado, long dvh)
        {
            string query = @"INSERT INTO Contrato (NroContrato, FechaEmision, DNIPropietario, DNICliente, CodigoPropiedad,
                                                   TipoContrato, FechaInicio, FechaFin, MontoAcordado, FormaPago,
                                                   DepositoGarantia, Estado, DVH)
                              VALUES (@nroContrato, @fechaEmision, @dniPropietario, @dniCliente, @codigoPropiedad,
                                      @tipoContrato, @fechaInicio, @fechaFin, @montoAcordado, @formaPago,
                                      @depositoGarantia, @estado, @dvh)";

            var parametros = new Dictionary<string, object>
            {
                { "@nroContrato", nroContrato },
                { "@fechaEmision", fechaEmision },
                { "@dniPropietario", dniPropietario },
                { "@dniCliente", dniCliente },
                { "@codigoPropiedad", codigoPropiedad },
                { "@tipoContrato", tipoContrato },
                { "@fechaInicio", fechaInicio },
                { "@fechaFin", fechaFin },
                { "@montoAcordado", montoAcordado },
                { "@formaPago", formaPago },
                { "@depositoGarantia", depositoGarantia },
                { "@estado", estado },
                { "@dvh", dvh }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public DataTable ObtenerPorNro(string nroContrato)
        {
            string query = @"SELECT NroContrato, FechaEmision, DNIPropietario, DNICliente, CodigoPropiedad,
                                    TipoContrato, FechaInicio, FechaFin, MontoAcordado, FormaPago,
                                    DepositoGarantia, Estado, DVH
                             FROM Contrato
                             WHERE NroContrato = @nroContrato";

            var parametros = new Dictionary<string, object>
            {
                { "@nroContrato", nroContrato }
            };

            return acceso.executeDataTable(query, parametros);
        }

        public DataTable ObtenerTodos()
        {
            string query = @"SELECT NroContrato, FechaEmision, DNIPropietario, DNICliente, CodigoPropiedad,
                                    TipoContrato, FechaInicio, FechaFin, MontoAcordado, FormaPago,
                                    DepositoGarantia, Estado, DVH
                             FROM Contrato
                             ORDER BY NroContrato";

            return acceso.executeDataTable(query);
        }

        public string ObtenerContratoPendiente(int idPropiedad)
        {
            string query = @"SELECT TOP 1 NroContrato
                             FROM Contrato
                             WHERE CodigoPropiedad = @idPropiedad AND Estado = 'Pendiente'
                             ORDER BY FechaEmision DESC, NroContrato DESC";

            var parametros = new Dictionary<string, object>
            {
                { "@idPropiedad", idPropiedad }
            };

            object resultado = acceso.executeScalar(query, parametros);

            return resultado == null || resultado == DBNull.Value ? null : resultado.ToString();
        }

        public DataTable ObtenerPendientes()
        {
            string query = @"SELECT c.NroContrato, c.FechaEmision, c.DNIPropietario, c.DNICliente, c.CodigoPropiedad,
                                    c.TipoContrato, c.FechaInicio, c.FechaFin, c.MontoAcordado, c.FormaPago,
                                    c.DepositoGarantia, c.Estado, c.DVH,
                                    p.Direccion AS PropiedadDireccion
                             FROM Contrato c
                             LEFT JOIN Propiedad p ON p.Id = c.CodigoPropiedad
                             WHERE c.Estado = 'Pendiente'
                             ORDER BY c.FechaEmision, c.NroContrato";

            return acceso.executeDataTable(query);
        }

        public DataTable ObtenerFirmados()
        {
            string query = @"SELECT c.NroContrato, c.FechaEmision, c.DNIPropietario, c.DNICliente, c.CodigoPropiedad,
                                    c.TipoContrato, c.FechaInicio, c.FechaFin, c.MontoAcordado, c.FormaPago,
                                    c.DepositoGarantia, c.Estado, c.DVH,
                                    p.Direccion AS PropiedadDireccion
                             FROM Contrato c
                             LEFT JOIN Propiedad p ON p.Id = c.CodigoPropiedad
                             WHERE c.Estado = 'Firmado'
                             ORDER BY c.FechaEmision DESC, c.NroContrato";

            return acceso.executeDataTable(query);
        }

        public int ActualizarEstado(string nroContrato, string estado, long dvh)
        {
            string query = @"UPDATE Contrato
                             SET Estado = @estado, DVH = @dvh
                             WHERE NroContrato = @nroContrato AND Estado = 'Pendiente'";

            var parametros = new Dictionary<string, object>
            {
                { "@nroContrato", nroContrato },
                { "@estado", estado },
                { "@dvh", dvh }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public int ActualizarEstadoConPropiedad(string nroContrato, string estadoContrato, long dvhContrato,
            int idPropiedad, string estadoPropiedad, long dvhPropiedad)
        {
            string query = @"SET XACT_ABORT ON;
                             BEGIN TRY
                                 BEGIN TRANSACTION;

                                 UPDATE Contrato
                                 SET Estado = @estadoContrato, DVH = @dvhContrato
                                 WHERE NroContrato = @nroContrato AND Estado = 'Pendiente';

                                 IF @@ROWCOUNT = 0
                                     THROW 50001, 'El contrato ya no se encuentra en estado Pendiente.', 1;

                                 UPDATE Propiedad
                                 SET Estado = @estadoPropiedad, DVH = @dvhPropiedad
                                 WHERE Id = @idPropiedad AND Estado IN ('Disponible', 'Reservada');

                                 IF @@ROWCOUNT = 0
                                     THROW 50002, 'La propiedad ya no se encuentra disponible para firmar el contrato.', 1;

                                 COMMIT TRANSACTION;
                             END TRY
                             BEGIN CATCH
                                 IF @@TRANCOUNT > 0
                                     ROLLBACK TRANSACTION;

                                 THROW;
                             END CATCH";

            var parametros = new Dictionary<string, object>
            {
                { "@nroContrato", nroContrato },
                { "@estadoContrato", estadoContrato },
                { "@dvhContrato", dvhContrato },
                { "@idPropiedad", idPropiedad },
                { "@estadoPropiedad", estadoPropiedad },
                { "@dvhPropiedad", dvhPropiedad }
            };

            return acceso.executeNonQuery(query, parametros);
        }

        public void ActualizarDVH(string nroContrato, long dvh)
        {
            string query = "UPDATE Contrato SET DVH = @dvh WHERE NroContrato = @nroContrato";

            var parametros = new Dictionary<string, object>
            {
                { "@dvh", dvh },
                { "@nroContrato", nroContrato }
            };

            acceso.executeNonQuery(query, parametros);
        }
    }
}
