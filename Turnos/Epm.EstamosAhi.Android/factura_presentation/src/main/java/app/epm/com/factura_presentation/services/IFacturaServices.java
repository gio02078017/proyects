package app.epm.com.factura_presentation.services;



import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.ItemGeneralDTO;
import com.epm.app.business_models.dto.MensajeDTO;

import java.util.List;

import app.epm.com.factura_presentation.dto.DataContratosDTO;
import app.epm.com.factura_presentation.dto.EstadoFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.FacturaResponseDTO;
import app.epm.com.factura_presentation.dto.HistoricoFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.InformacionPseDTO;
import app.epm.com.factura_presentation.dto.ServicioFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.TransaccionPSEResponseDTO;
import retrofit.http.Body;
import retrofit.http.GET;
import retrofit.http.POST;
import retrofit.http.Query;


/**
 * Created by ocadavid on 15/12/2016.
 */

public interface IFacturaServices {

    /**
     * Consulta la factura por referente de pago.
     *
     * @param number
     * @return
     */
    @GET("/Factura/ConsultaDeReferentesDePago")
    List<FacturaResponseDTO> consultarFacturaPorReferenteDePago(@Query("documentoReferencia") String number);

    /**
     * Consulta la factura por contrato.
     *
     * @param number
     * @return
     */
    @GET("/Factura/ConsultaDeContratos")
    List<FacturaResponseDTO> consultarFacturaPorContrato(@Query("numeroContrato") String number);

    /**
     * Consulta las facturas pertenecientes a un usuario registrado.
     *
     * @param dataDTO
     * @return
     */
    @POST("/Factura/ConsultarFacturasPorUsuario")
    List<FacturaResponseDTO> consultarFacturasPorUsuario(@Body DataDTO dataDTO);

    /**
     * Valida si una factura se encuentra en estado pendiente.
     *
     * @param numberReferencia
     * @return
     */
    @GET("/Factura/ValidarFacturasPendientes")
    EstadoFacturaResponseDTO validarFacturasPendientes(@Query("documentoReferencia") String numberReferencia);

    /**
     * Obtiene la lista de entidades financieras.
     *
     * @return
     */
    @GET("/Consultas/ConsultarEntidadesFinancieras")
    List<ItemGeneralDTO> consultarEntidadesFinancieras();

    /**
     * Obtiene la información de los servicios de la factura.
     *
     * @param numberFactura
     * @return
     */
    @GET("/Factura/ConsultarDetalleFacturas")
    List<ServicioFacturaResponseDTO> consultarDetalleFacturas(@Query("numeroFactura") String numberFactura);

    /**
     * Permite consultar la información histórica de facturas asociadas a un contrato.
     *
     * @param numberContrato
     * @return
     */
    @GET("/Factura/ConsultarHistorico")
    List<HistoricoFacturaResponseDTO> consultarHistorico(@Query("numeroContrato") String numberContrato);

    /**
     * Permite inscribir un contrato.
     *
     * @param dataDTO dataDTO.
     * @return MensajeDTO.
     */
    @POST("/Factura/InscribirContratos")
    MensajeDTO inscribirContrato(@Body DataDTO dataDTO);

    /**
     * Permite enviar los datos del usuario para pagar.
     *
     * @param dataDTO dataDTO.
     * @return List InformacionPseDTO.
     */
    @POST("/Factura/Pagar")
    List<InformacionPseDTO> datosPagar(@Body DataDTO dataDTO);

    /**
     * Procesa la información de PSE.
     * @param dataDTO dataDTO.
     * @return List TransaccionPSEResponseDT
     */
    @POST("/Factura/ProcesarInformacionPSE")
    TransaccionPSEResponseDTO transaccionPSERespuesta(@Body DataDTO dataDTO);

    /**
     * Permite crear el comprobante de pago.
     *
     * @param dataDTO dataDTO.
     * @return List MensajeDTO.
     */
    @POST("/Factura/CrearComprobantePago")
    List<MensajeDTO> crearComprobantePago(@Body DataDTO dataDTO);

    /**
     * Permite actualizar los contratos.
     * @param dataDTO dataDTO.
     * @return List MensajeDTO.
     */
    @POST("/Factura/ActualizarContratos")
    MensajeDTO actualizarContratos(@Body DataDTO dataDTO);

    /**
     * Permite consultar los contratos inscritos por usuario.
     * @param dataDTO dataDTO.
     * @return List DataContratosDTO.
     */
    @POST("/Factura/ConsultarContratosPorUsuario")
    List<DataContratosDTO> consultarContratosInscritos(@Body DataDTO dataDTO);

    @GET("/Factura/ConsultarDetalleFacturas")
    List<ServicioFacturaResponseDTO> getDetailFactura(@Query("numeroFactura") String numberFactura);
}