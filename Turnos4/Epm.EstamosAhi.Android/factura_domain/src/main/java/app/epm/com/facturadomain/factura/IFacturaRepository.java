package app.epm.com.facturadomain.factura;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.facturadomain.business_models.ComprobantePago;
import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.DataPagar;
import app.epm.com.facturadomain.business_models.EstadoFacturaResponse;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.HistoricoFacturaResponse;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.facturadomain.business_models.ProcesarInformacionPSE;
import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.facturadomain.business_models.TransaccionPSEResponse;

/**
 * Created by ocadavid on 15/12/2016.
 */

public interface IFacturaRepository {

    /**
     * Consulta la factura por referente de pago.
     *
     * @param number
     * @return
     * @throws RepositoryError
     */
    List<FacturaResponse> consultFacturaPorReferenteDePago(String number) throws RepositoryError;

    /**
     * Consulta la factura por contrato.
     *
     * @param number
     * @return
     * @throws RepositoryError
     */
    List<FacturaResponse> consultFacturaPorContrato(String number) throws RepositoryError;

    /**
     * Consulta las facturas pertenecientes a un usuario registrado.
     *
     * @param email
     * @return
     * @throws RepositoryError
     */
    List<FacturaResponse> consultFacturasPorUsuario(String email) throws RepositoryError;

    /**
     * Valida si una factura se encuentra en estado pendiente.
     *
     * @param numberReferente
     * @return
     * @throws RepositoryError
     */
    EstadoFacturaResponse validateFacturasPendientes(String numberReferente) throws RepositoryError;

    /**
     * Obtiene la lista de entidades financieras.
     *
     * @return
     * @throws RepositoryError
     */
    List<ItemGeneral> consultEntidadesFinancieras() throws RepositoryError;

    /**
     * Obtiene la información de los servicios de la factura.
     *
     * @param numberFactura
     * @return
     * @throws RepositoryError
     */
    List<ServicioFacturaResponse> consultDetalleFacturas(String numberFactura) throws RepositoryError;

    /**
     * Permite consultar la información histórica de facturas asociadas a un contrato.
     *
     * @param numberContrato
     * @return
     * @throws RepositoryError
     */
    List<HistoricoFacturaResponse> consultHistorico(String numberContrato) throws RepositoryError;

    /**
     * Obtiene mensaje de inscribir contrato.
     *
     * @param inscribirContrato inscribirContrato.
     * @return Mensaje.
     * @throws RepositoryError
     */
    Mensaje inscribirContrato(GestionContrato inscribirContrato) throws RepositoryError;

    /**
     * Obtiene la información de PSE para pagar.
     *
     * @param dataPagar dataPagar.
     * @return InformacionPSE.
     * @throws RepositoryError
     */
    InformacionPSE datosPagar(DataPagar dataPagar) throws RepositoryError;

    /**
     * Obtiene mensaje de comprobante de pago
     *
     * @param comprobantePago comprobantePago.
     * @throws RepositoryError
     */
    Mensaje comprobantePago(ComprobantePago comprobantePago) throws RepositoryError;

    /**
     * Procesa la inforamción de PSE.
     *
     * @param procesarInformacionPSE procesarInformacionPSE.
     * @return TransaccionPSEResponse.
     * @throws RepositoryError
     */
    TransaccionPSEResponse procesarInformacionPSE(ProcesarInformacionPSE procesarInformacionPSE) throws RepositoryError;

    /**
     * Actualiza los contratos inscritos.
     *
     * @param infoContratos Información de los contratos.
     * @throws RepositoryError
     */
    Mensaje updateContratos(GestionContrato infoContratos) throws RepositoryError;

    /**
     * Consulta los contratos inscritos por usuario.
     *
     * @param email
     * @return
     * @throws RepositoryError
     */
    List<DataContratos> consultContratosInscritos(String email) throws RepositoryError;

    /**
     * Consulta el detalle de la factura
     * @param numberFactura
     * @return
     * @throws RepositoryError
     */
    List<ServicioFacturaResponse> detailFactura(String numberFactura) throws RepositoryError;
}