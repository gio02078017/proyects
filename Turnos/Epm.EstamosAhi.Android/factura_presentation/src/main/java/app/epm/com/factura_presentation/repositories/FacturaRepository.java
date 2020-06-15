package app.epm.com.factura_presentation.repositories;

import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.ItemGeneralDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ItemGeneral;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

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
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.dto.ComprobantePagoDTO;
import app.epm.com.factura_presentation.dto.DataContratosDTO;
import app.epm.com.factura_presentation.dto.DataPagarDTO;
import app.epm.com.factura_presentation.dto.EstadoFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.FacturaResponseDTO;
import app.epm.com.factura_presentation.dto.HistoricoFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.InformacionPseDTO;
import app.epm.com.factura_presentation.dto.GestionContratoDTO;
import app.epm.com.factura_presentation.dto.ProcesarInformacionPseDTO;
import app.epm.com.factura_presentation.dto.ServicioFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.TransaccionPSEResponseDTO;
import app.epm.com.factura_presentation.helpers.Mapper;
import app.epm.com.factura_presentation.services.IFacturaServices;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.services.ServicesFactory;
import retrofit.RetrofitError;

/**
 * Created by ocadavid on 15/12/2016.
 */

public class FacturaRepository implements IFacturaRepository {
    private IFacturaServices facturaServices;
    private Gson gson;

    public FacturaRepository(ICustomSharedPreferences customSharedPreferences) {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        facturaServices = (IFacturaServices) servicesFactory.getInstance(IFacturaServices.class);
        gson = new GsonBuilder().disableHtmlEscaping().create();
    }

    /**
     * Consulta la factura por referente de pago.
     *
     * @param number
     * @return
     * @throws RepositoryError
     */
    @Override
    public List<FacturaResponse> consultFacturaPorReferenteDePago(String number) throws RepositoryError {
        try {
            List<FacturaResponseDTO> facturaResponseDTO = facturaServices.consultarFacturaPorReferenteDePago(number);
            return Mapper.convertListFacturaResponseDTOToDomain(facturaResponseDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Consulta la factura por contrato.
     *
     * @param number
     * @return
     * @throws RepositoryError
     */
    @Override
    public List<FacturaResponse> consultFacturaPorContrato(String number) throws RepositoryError {
        try {
            List<FacturaResponseDTO> facturaResponseDTO = facturaServices.consultarFacturaPorContrato(number);
            return Mapper.convertListFacturaResponseDTOToDomain(facturaResponseDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Consulta las facturas pertenecientes a un usuario registrado.
     *
     * @param email
     * @return
     * @throws RepositoryError
     */
    @Override
    public List<FacturaResponse> consultFacturasPorUsuario(String email) throws RepositoryError {
        try {
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(Mapper.convertStringEmailToEmailUsuarioDTO(email)));
            List<FacturaResponseDTO> facturaResponseDTO = facturaServices.consultarFacturasPorUsuario(dataDTO);
//            if (facturaResponseDTO == null || facturaResponseDTO.get(0).getDocumentoReferencia() == null) {
//                facturaResponseDTO.clear();
//            }
            return Mapper.convertListFacturaResponseDTOToDomain(facturaResponseDTO);
        } catch (Exception retrofitError) {
            throw retrofitError;
        }
    }

    /**
     * Valida si una factura se encuentra en estado pendiente.
     *
     * @param numberReferente
     * @return
     * @throws RepositoryError
     */
    @Override
    public EstadoFacturaResponse validateFacturasPendientes(String numberReferente) throws RepositoryError {
        try {
            EstadoFacturaResponseDTO estadoFacturaResponseDTO = facturaServices.validarFacturasPendientes(numberReferente);
            return Mapper.convertEstadoFacturaResponseDTOToDomain(estadoFacturaResponseDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Obtiene la lista de entidades financieras.
     *
     * @return
     * @throws RepositoryError
     */
    @Override
    public List<ItemGeneral> consultEntidadesFinancieras() throws RepositoryError {
        try {
            List<ItemGeneralDTO> itemGeneralDTOs = facturaServices.consultarEntidadesFinancieras();
            return Mapper.convertListItemGeneralDTOToDomain(itemGeneralDTOs);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Obtiene la información de los servicios de la factura.
     *
     * @param numberFactura
     * @return
     * @throws RepositoryError
     */
    public List<ServicioFacturaResponse> consultDetalleFacturas(String numberFactura) throws RepositoryError {
        try {
            List<ServicioFacturaResponseDTO> servicioFacturaResponseDTOs = facturaServices.consultarDetalleFacturas(numberFactura);
            return Mapper.convertListServicioFacturaResponseDTOToDomain(servicioFacturaResponseDTOs);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Permite consultar la información histórica de facturas asociadas a un contrato.
     *
     * @param numberContrato
     * @return
     * @throws RepositoryError
     */
    public List<HistoricoFacturaResponse> consultHistorico(String numberContrato) throws RepositoryError {
        try {
            List<HistoricoFacturaResponseDTO> historicoFacturaResponseDTOs = facturaServices.consultarHistorico(numberContrato);
            return Mapper.convertListHistoricoFacturaResponseDTOToDomain(historicoFacturaResponseDTOs);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Mensaje inscribirContrato(GestionContrato inscribirContrato) throws RepositoryError {
        try {
            GestionContratoDTO inscribirContratoDTO = Mapper.convertInscribirContratoDTOToDomain(inscribirContrato);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(inscribirContratoDTO));
            MensajeDTO mensajeDTO = facturaServices.inscribirContrato(dataDTO);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public InformacionPSE datosPagar(DataPagar dataPagar) throws RepositoryError {
        try {
            DataPagarDTO dataPagarDTO = Mapper.convertDataPagarDTOToDomain(dataPagar);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(dataPagarDTO));
            List<InformacionPseDTO> informacionPseDTO = facturaServices.datosPagar(dataDTO);
            return Mapper.convertInformacionPSEDomainToDTO(informacionPseDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Mensaje comprobantePago(ComprobantePago comprobantePago) throws RepositoryError {
        try {
            ComprobantePagoDTO comprobantePagoDTO = Mapper.convertComprobantePagoDTOToDomain(comprobantePago);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(comprobantePagoDTO));
            List<MensajeDTO> mensajeDTOList = facturaServices.crearComprobantePago(dataDTO);
            return Mapper.convertListMensajeDTOToDomain(mensajeDTOList);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public TransaccionPSEResponse procesarInformacionPSE(ProcesarInformacionPSE procesarInformacionPSE) throws RepositoryError {
        try {
            ProcesarInformacionPseDTO procesarInformacionPseDTO = Mapper.convertProcesarInformacionPSEDTOToDomain(procesarInformacionPSE);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(procesarInformacionPseDTO));
            TransaccionPSEResponseDTO transaccionPSEResponseDTO = facturaServices.transaccionPSERespuesta(dataDTO);
            return Mapper.convertTransaccionPSEResponseDTOToDomain(transaccionPSEResponseDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Actualiza la información de los contratos.
     *
     * @param infoContratos Información de los contratos.
     * @return
     * @throws RepositoryError
     */
    @Override
    public Mensaje updateContratos(GestionContrato infoContratos) throws RepositoryError {
        try {
            GestionContratoDTO inforContratosDTO = Mapper.convertInscribirContratoDTOToDomain(infoContratos);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(inforContratosDTO));
            MensajeDTO mensajeDTO = facturaServices.actualizarContratos(dataDTO);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Consulta los contratos inscritos de un usuario.
     *
     * @param email
     * @return
     * @throws RepositoryError
     */
    @Override
    public List<DataContratos> consultContratosInscritos(String email) throws RepositoryError {
        try {
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(Mapper.convertStringEmailToEmailUsuarioDTO(email)));
            List<DataContratosDTO> dataContratosDTOs = facturaServices.consultarContratosInscritos(dataDTO);
            return Mapper.convertDataContratosDTOToDomain(dataContratosDTOs);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    /**
     * Consulta el detalla de una factura
     *
     * @param numberFactura numero de factura
     * @return
     * @throws RepositoryError
     */
    @Override
    public List<ServicioFacturaResponse> detailFactura(String numberFactura) throws RepositoryError {
        try {
            List<ServicioFacturaResponseDTO> servicioFacturaResponseDTO = facturaServices.getDetailFactura(numberFactura);
            return Mapper.convertServicioFacturaResponseDTOToDomain(servicioFacturaResponseDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }

    }
}