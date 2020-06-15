package app.epm.com.factura_presentation.helpers;

import androidx.annotation.Nullable;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.dto.ItemGeneralDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.google.common.base.Function;
import com.google.common.collect.Lists;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.ComprobantePago;
import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.DataPagar;
import app.epm.com.facturadomain.business_models.DetalleServicioFactura;
import app.epm.com.facturadomain.business_models.EstadoFacturaResponse;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.FacturasPorTransaccion;
import app.epm.com.facturadomain.business_models.HistoricoFacturaResponse;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.facturadomain.business_models.ProcesarInformacionPSE;
import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.facturadomain.business_models.TransaccionPSEResponse;
import app.epm.com.factura_presentation.dto.ComprobantePagoDTO;
import app.epm.com.factura_presentation.dto.DataContratosDTO;
import app.epm.com.factura_presentation.dto.DataPagarDTO;
import app.epm.com.factura_presentation.dto.DetalleServicioFacturaDTO;
import app.epm.com.factura_presentation.dto.EstadoFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.FacturaResponseDTO;
import app.epm.com.factura_presentation.dto.FacturasPorTransaccionDTO;
import app.epm.com.factura_presentation.dto.HistoricoFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.InformacionPseDTO;
import app.epm.com.factura_presentation.dto.GestionContratoDTO;
import app.epm.com.factura_presentation.dto.ProcesarInformacionPseDTO;
import app.epm.com.factura_presentation.dto.ServicioFacturaResponseDTO;
import app.epm.com.factura_presentation.dto.TransaccionPSEResponseDTO;
import app.epm.com.security_presentation.dto.EmailUsuarioDTO;
import app.epm.com.utilities.helpers.BaseMapper;

/**
 * Created by ocadavid on 15/12/2016.
 */

public class Mapper extends BaseMapper {


    /**
     * Mapea la lista List<FacturaResponseDTO>  a List<FacturaResponse>.
     *
     * @param facturaResponseDTO
     * @return
     */
    public static List<FacturaResponse> convertListFacturaResponseDTOToDomain(List<FacturaResponseDTO> facturaResponseDTO) {
        ArrayList<FacturaResponse> facturas = new ArrayList<>();

        for (FacturaResponseDTO factDTO : facturaResponseDTO) {
            FacturaResponse facResponse = new FacturaResponse();
            facResponse.setDescripcionContrato(factDTO.getDescripcionContrato());
            facResponse.setId(factDTO.getId());
            facResponse.setDocumentoReferencia(factDTO.getDocumentoReferencia());
            facResponse.setFechaCreacion(factDTO.getFechaCreacion());
            facResponse.setFechaRecargo(factDTO.getFechaRecargo());
            facResponse.setFechaVencimiento(factDTO.getFechaVencimiento());
            facResponse.setNumeroContrato(factDTO.getNumeroContrato());
            facResponse.setNumeroFactura(factDTO.getNumeroFactura());
            facResponse.setValorFactura(factDTO.getValorFactura());
            facResponse.setUrl(factDTO.getUrl());
            facResponse.setEstadoPagoFactura(factDTO.getEstadoPagoFactura());
            facResponse.setFacturaVencida(factDTO.getFacturaVencida());
            facResponse.setCode(factDTO.getCodigo());
            facResponse.setText(factDTO.getTexto());

            facturas.add(facResponse);
        }

        return facturas;
    }

    /**
     * Mapea la entidad EstadoFacturaResponseDTO  a EstadoFacturaResponse.
     *
     * @param estadoFacturaResponseDTO
     * @return
     */
    public static EstadoFacturaResponse convertEstadoFacturaResponseDTOToDomain(EstadoFacturaResponseDTO estadoFacturaResponseDTO) {
        EstadoFacturaResponse estadoFacturaResponse = new EstadoFacturaResponse();
        estadoFacturaResponse.setEstado(estadoFacturaResponseDTO.isEstado());
        return estadoFacturaResponse;
    }

    /**
     * Mapea la lista List<ItemGeneralDTO> a  List<ItemGeneralResponse>.
     *
     * @param itemGeneralDTOs
     * @return
     */
    public static List<ItemGeneral> convertListItemGeneralDTOToDomain(List<ItemGeneralDTO> itemGeneralDTOs) {
        ArrayList<ItemGeneral> lista = new ArrayList<>();

        for (ItemGeneralDTO itemGeneralDTO : itemGeneralDTOs) {
            ItemGeneral itemGeneralResponse = new ItemGeneral();
            itemGeneralResponse.setId(itemGeneralDTO.getId());
            itemGeneralResponse.setCodigo(itemGeneralDTO.getCodigo());
            itemGeneralResponse.setDescripcion(itemGeneralDTO.getDescripcion());

            lista.add(itemGeneralResponse);
        }

        return lista;
    }

    /**
     * Mapea la lista List<ServicioFacturaResponseDTO> a List<ServicioFacturaResponse>.
     *
     * @param servicioFacturaResponseDTOs
     * @return
     */
    public static List<ServicioFacturaResponse> convertListServicioFacturaResponseDTOToDomain(
            List<ServicioFacturaResponseDTO> servicioFacturaResponseDTOs) {
        ArrayList<ServicioFacturaResponse> listaServicio = new ArrayList<ServicioFacturaResponse>();

        for (ServicioFacturaResponseDTO servicioFactura : servicioFacturaResponseDTOs) {
            ServicioFacturaResponse servicioFacturaResponse = new ServicioFacturaResponse();
            servicioFacturaResponse.setServicio(servicioFactura.getServicio());
            servicioFacturaResponse.setConsumo(servicioFactura.getConsumo());
            servicioFacturaResponse.setValorAPagar(servicioFactura.getValorAPagar());
            servicioFacturaResponse.setUnidadDeMedida(servicioFactura.getUnidadDeMedida());

            ArrayList<DetalleServicioFactura> listaDetalle = new ArrayList<DetalleServicioFactura>();

            for (DetalleServicioFacturaDTO detalle : servicioFactura.getDetalleServicio()) {
                DetalleServicioFactura detalleServicioFactura = new DetalleServicioFactura();
                detalleServicioFactura.setValor(detalle.getValor());
                detalleServicioFactura.setDescripcion(detalle.getDescripcion());

                listaDetalle.add(detalleServicioFactura);
            }

            servicioFacturaResponse.setDetalleServicio(listaDetalle);
            listaServicio.add(servicioFacturaResponse);
        }

        return listaServicio;
    }

    /**
     * Mapea la lista List<HistoricoFacturaResponseDTO> a List<HistoricoFacturaResponse>.
     *
     * @param historicoFacturaResponseDTOs
     * @return
     */
    public static List<HistoricoFacturaResponse> convertListHistoricoFacturaResponseDTOToDomain(
            List<HistoricoFacturaResponseDTO> historicoFacturaResponseDTOs) {

        ArrayList<HistoricoFacturaResponse> listaHistorico = new ArrayList<HistoricoFacturaResponse>();

        for (HistoricoFacturaResponseDTO historicoFacturaResponseDTO : historicoFacturaResponseDTOs) {
            HistoricoFacturaResponse historicoFacturaResponse = new HistoricoFacturaResponse();
            historicoFacturaResponse.setBancoPagoFactura(historicoFacturaResponseDTO.getBancoPagoFactura());
            historicoFacturaResponse.setAnio(historicoFacturaResponseDTO.getAnio());
            historicoFacturaResponse.setEstadoPagoFactura(historicoFacturaResponseDTO.isEstadoPagoFactura());
            historicoFacturaResponse.setFechaPagoFactura(historicoFacturaResponseDTO.getFechaPagoFactura());
            historicoFacturaResponse.setFechaRecargo(historicoFacturaResponseDTO.getFechaRecargo());
            historicoFacturaResponse.setFechaVencimiento(historicoFacturaResponseDTO.getFechaVencimiento());
            historicoFacturaResponse.setMes(historicoFacturaResponseDTO.getMes());
            historicoFacturaResponse.setNombreMes(historicoFacturaResponseDTO.getNombreMes());
            historicoFacturaResponse.setNumeroFactura(historicoFacturaResponseDTO.getNumeroFactura());
            historicoFacturaResponse.setValorFactura(historicoFacturaResponseDTO.getValorFactura());
            historicoFacturaResponse.setUrlFacturaDigital(historicoFacturaResponseDTO.getUrlFacturaDigital());

            listaHistorico.add(historicoFacturaResponse);
        }

        return listaHistorico;
    }

    public static EmailUsuarioDTO convertStringEmailToEmailUsuarioDTO(String email) {
        EmailUsuarioDTO emailUsuarioDTO = new EmailUsuarioDTO();
        emailUsuarioDTO.setCorreoElectronico(email);
        return emailUsuarioDTO;
    }

    public static GestionContratoDTO convertInscribirContratoDTOToDomain(GestionContrato inscribirContrato) {
        GestionContratoDTO inscribirContratoDTO = new GestionContratoDTO();
        inscribirContratoDTO.setCorreoElectronico(inscribirContrato.getCorreoElectronico());
        inscribirContratoDTO.setContratos(convertDataContratosToDTO(inscribirContrato.getContratos()));
        return inscribirContratoDTO;
    }

    public static List<DataContratosDTO> convertDataContratosToDTO(List<DataContratos> dataContratos) {
        return Lists.transform(dataContratos, new Function<DataContratos, DataContratosDTO>() {
            @Nullable
            @Override
            public DataContratosDTO apply(DataContratos dataContratos) {
                DataContratosDTO dataContratosDTO = new DataContratosDTO();
                dataContratosDTO.setDescripcion(dataContratos.getDescripcion());
                dataContratosDTO.setNumero(dataContratos.getNumero());
                dataContratosDTO.setRecibirFacturaDigital(dataContratos.isRecibirFacturaDigital());
                dataContratosDTO.setOperacion(dataContratos.getOperacion());
                return dataContratosDTO;
            }
        });
    }

    public static DataPagarDTO convertDataPagarDTOToDomain(DataPagar dataPagar) {
        DataPagarDTO dataPagarDTO = new DataPagarDTO();
        dataPagarDTO.setEntidadFinanciera(dataPagar.getEntidadFinanciera());
        dataPagarDTO.setIdTipoPersona(dataPagar.getIdTipoPersona());
        dataPagarDTO.setIdTipoDocumento(dataPagar.getIdTipoDocumento());
        dataPagarDTO.setNumeroDocumento(dataPagar.getNumeroDocumento());
        dataPagarDTO.setFacturasPagar(convertListFacturaResponseDTOToDomainSendPagar(dataPagar.getFacturasPagar()));
        dataPagarDTO.setDireccionIp(dataPagar.getDireccionIp());
        return dataPagarDTO;
    }

    public static List<FacturaResponseDTO> convertListFacturaResponseDTOToDomainSendPagar(List<FacturaResponse> facturaResponses) {

        return Lists.transform(facturaResponses, new Function<FacturaResponse, FacturaResponseDTO>() {
            @Nullable
            @Override
            public FacturaResponseDTO apply(FacturaResponse facturaResponse) {
                FacturaResponseDTO facResponseDTO = new FacturaResponseDTO();
                facResponseDTO.setDescripcionContrato(facturaResponse.getDescripcionContrato());
                facResponseDTO.setId(facturaResponse.getId());
                facResponseDTO.setDocumentoReferencia(facturaResponse.getDocumentoReferencia());
                facResponseDTO.setFechaCreacion(facturaResponse.getFechaCreacion());
                facResponseDTO.setFechaRecargo(facturaResponse.getFechaRecargo());
                facResponseDTO.setFechaVencimiento(facturaResponse.getFechaVencimiento());
                facResponseDTO.setNumeroContrato(facturaResponse.getNumeroContrato());
                facResponseDTO.setNumeroFactura(facturaResponse.getNumeroFactura());
                facResponseDTO.setValorFactura(facturaResponse.getValorFactura());
                facResponseDTO.setUrl(facturaResponse.getUrl());
                facResponseDTO.setEstadoPagoFactura(facturaResponse.isEstadoPagoFactura());
                facResponseDTO.setFacturaVencida(facturaResponse.isFacturaVencida());
                facResponseDTO.setCodigo(facturaResponse.getCode());
                facResponseDTO.setTexto(facturaResponse.getText());
                return facResponseDTO;
            }
        });
    }

    public static InformacionPSE convertInformacionPSEDomainToDTO(List<InformacionPseDTO> informacionPseDTO) {
        final InformacionPSE informacionPSE = new InformacionPSE();
        informacionPSE.setIdTransaccion(informacionPseDTO.get(0).getIdTransaccion());
        informacionPSE.setCodigoTrazabilidad(informacionPseDTO.get(0).getCodigoTrazabilidad());
        informacionPSE.setUrlEntidadFinanciera(informacionPseDTO.get(0).getURLEntidadFinanciera());
        informacionPSE.setEstadoTransaccion(informacionPseDTO.get(0).getEstadoTransaccion());
        informacionPSE.setTieneFacturasVencidas(informacionPseDTO.get(0).isTieneFacturasVencidas());
        informacionPSE.setUrlRetorno(informacionPseDTO.get(0).getURLRetorno());
        informacionPSE.setFacturasPorTransaccion(convertArrayListFacturasPorTransaccionDomainToDTO(
                informacionPseDTO.get(0).getFacturasPorTransaccion()));
        informacionPSE.setDireccionIPPersona(informacionPseDTO.get(0).getDireccionIPPersona());
        informacionPSE.setFechaTransaccion(informacionPseDTO.get(0).getFechaTransaccion());
        return informacionPSE;
    }

    public static ArrayList<FacturasPorTransaccion> convertArrayListFacturasPorTransaccionDomainToDTO(
            ArrayList<FacturasPorTransaccionDTO> facturasPorTransaccionDTO) {
        final ArrayList<FacturasPorTransaccion> facturasPorTransaccions = new ArrayList<>();

        for (FacturasPorTransaccionDTO transaccionDTO : facturasPorTransaccionDTO) {
            FacturasPorTransaccion facturasPorTransaccion = new FacturasPorTransaccion();
            facturasPorTransaccion.setDocumentoReferencia(transaccionDTO.getDocumentoReferencia());
            facturasPorTransaccion.setIdFacturaPorTransaccion(transaccionDTO.getIdFacturaPorTransaccion());
            facturasPorTransaccion.setIdFactura(transaccionDTO.getIdFactura());
            facturasPorTransaccion.setFechaCreacion(transaccionDTO.getFechaCreacion());
            facturasPorTransaccions.add(facturasPorTransaccion);
        }
        return facturasPorTransaccions;
    }

    public static ArrayList<FacturasPorTransaccionDTO> convertArrayListFacturasPorTransaccionDTOToDomain(
            ArrayList<FacturasPorTransaccion> facturasPorTransaccion) {
        final ArrayList<FacturasPorTransaccionDTO> facturasPorTransaccionDTOs = new ArrayList<>();

        for (FacturasPorTransaccion porTransaccion : facturasPorTransaccion) {
            FacturasPorTransaccionDTO facturasPorTransaccionDTO = new FacturasPorTransaccionDTO();
            facturasPorTransaccionDTO.setDocumentoReferencia(porTransaccion.getDocumentoReferencia());
            facturasPorTransaccionDTO.setIdFacturaPorTransaccion(porTransaccion.getIdFacturaPorTransaccion());
            facturasPorTransaccionDTO.setIdFactura(porTransaccion.getIdFactura());
            facturasPorTransaccionDTO.setFechaCreacion(porTransaccion.getFechaCreacion());
            facturasPorTransaccionDTOs.add(facturasPorTransaccionDTO);
        }
        return facturasPorTransaccionDTOs;
    }


    public static ComprobantePagoDTO convertComprobantePagoDTOToDomain(ComprobantePago comprobantePago) {
        ComprobantePagoDTO comprobantePagoDTO = new ComprobantePagoDTO();
        comprobantePagoDTO.setCorreos(comprobantePago.getCorreos());
        comprobantePagoDTO.setValorTotalPago(comprobantePago.getValorTotalPago());
        comprobantePagoDTO.setIdTransaccion(comprobantePago.getIdTransaccion());
        comprobantePagoDTO.setCodigoTrazabilidad(comprobantePago.getCodigoTrazabilidad());
        comprobantePagoDTO.setEstadoTransaccion(comprobantePago.getEstadoTransaccion());
        comprobantePagoDTO.setNombreEntidadFinanciera(comprobantePago.getNombreEntidadFinanciera());
        comprobantePagoDTO.setDireccionIp(comprobantePago.getDireccionIp());
        comprobantePagoDTO.setFechaTransaccion(comprobantePago.getFechaTransaccion());
        comprobantePagoDTO.setFacturasPorTransaccion(convertListFacturasPorTransaccionDTOToDomain(
                comprobantePago.getFacturasPorTransaccion()));
        return comprobantePagoDTO;
    }

    public static List<FacturasPorTransaccionDTO> convertListFacturasPorTransaccionDTOToDomain(
            List<FacturasPorTransaccion> facturasPorTransaccion) {

        return Lists.transform(facturasPorTransaccion, new Function<FacturasPorTransaccion, FacturasPorTransaccionDTO>() {
            @Nullable
            @Override
            public FacturasPorTransaccionDTO apply(FacturasPorTransaccion facturasPorTransaccion) {
                FacturasPorTransaccionDTO facturasPorTransaccionDTO = new FacturasPorTransaccionDTO();
                facturasPorTransaccionDTO.setDocumentoReferencia(facturasPorTransaccion.getDocumentoReferencia());
                facturasPorTransaccionDTO.setIdFacturaPorTransaccion(facturasPorTransaccion.getIdFacturaPorTransaccion());
                facturasPorTransaccionDTO.setIdFactura(facturasPorTransaccion.getIdFactura());
                facturasPorTransaccionDTO.setFechaCreacion(facturasPorTransaccion.getFechaCreacion());
                return facturasPorTransaccionDTO;
            }
        });
    }

    public static Mensaje convertMensajeDTOToDomain(MensajeDTO mensajeDTO) {
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(mensajeDTO.getCodigo());
        mensaje.setText(mensajeDTO.getTexto());
        return mensaje;
    }

    public static Mensaje convertListMensajeDTOToDomain(List<MensajeDTO> mensajeDTOList) {
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(mensajeDTOList.get(0).getCodigo());
        mensaje.setText(mensajeDTOList.get(0).getTexto());
        return mensaje;
    }

    public static RepositoryError convertMensajeDTOToRepositoryError(MensajeDTO mensaje) {
        RepositoryError repositoryError = new RepositoryError(mensaje.getTexto());
        repositoryError.setIdError(mensaje.getCodigo());
        return repositoryError;

    }

    public static ProcesarInformacionPseDTO convertProcesarInformacionPSEDTOToDomain(
            ProcesarInformacionPSE procesarInformacionPSE) {
        ProcesarInformacionPseDTO procesarInformacionPseDTO = new ProcesarInformacionPseDTO();
        procesarInformacionPseDTO.setEstadoTransaccion(procesarInformacionPSE.getEstadoTransaccion());
        procesarInformacionPseDTO.setCodigoTrazabilidad(procesarInformacionPSE.getCodigoTrazabilidad());
        procesarInformacionPseDTO.setIdTransaccion(procesarInformacionPSE.getIdTransaccion());
        procesarInformacionPseDTO.setEntidadFinanciera(procesarInformacionPSE.getEntidadFinanciera());
        procesarInformacionPseDTO.setFacturasPorTransaccion(convertArrayListFacturasPorTransaccionDTOToDomain(
                procesarInformacionPSE.getFacturasPorTransaccion()));
        return procesarInformacionPseDTO;
    }

    public static TransaccionPSEResponse convertTransaccionPSEResponseDTOToDomain(TransaccionPSEResponseDTO transaccionPSEResponseDTO) {
        TransaccionPSEResponse transaccionPSEResponse = new TransaccionPSEResponse();
        transaccionPSEResponse.setEstadoTransaccion(transaccionPSEResponseDTO.getEstadoTransaccion());
        transaccionPSEResponse.setNombreEstadoTransaccion(transaccionPSEResponseDTO.getNombreEstadoTransaccion());
        return transaccionPSEResponse;
    }

    public static List<DataContratos> convertDataContratosDTOToDomain(List<DataContratosDTO> dataContratosDTOs) {
        return new ArrayList<>(Lists.transform(dataContratosDTOs, new Function<DataContratosDTO, DataContratos>() {
            @Nullable
            @Override
            public DataContratos apply(DataContratosDTO dataContratosDTO) {
                DataContratos dataContratos = new DataContratos();
                dataContratos.setDescripcion(dataContratosDTO.getDescripcion());
                dataContratos.setNumero(dataContratosDTO.getNumero());
                dataContratos.setRecibirFacturaDigital(dataContratosDTO.isRecibirFacturaDigital());
                dataContratos.setOperacion(dataContratosDTO.getOperacion());
                return dataContratos;
            }
        }));
    }

    public static List<ServicioFacturaResponse> convertServicioFacturaResponseDTOToDomain(
            List<ServicioFacturaResponseDTO> servicioFacturaResponseDTO) {
        List<ServicioFacturaResponse> servicioFacturaResponses = new ArrayList<>();
        for (ServicioFacturaResponseDTO servicioFacturaResponseDTOs : servicioFacturaResponseDTO) {
            ServicioFacturaResponse servicioFacturaResponse = new ServicioFacturaResponse();
            servicioFacturaResponse.setServicio(servicioFacturaResponseDTOs.getServicio());
            servicioFacturaResponse.setConsumo(servicioFacturaResponseDTOs.getConsumo());
            servicioFacturaResponse.setValorAPagar(servicioFacturaResponseDTOs.getValorAPagar());
            servicioFacturaResponse.setUnidadDeMedida(servicioFacturaResponseDTOs.getUnidadDeMedida());
            servicioFacturaResponse.setDetalleServicio(convertListDetailServicesDTOToDomain(
                    servicioFacturaResponseDTOs.getDetalleServicio()));
            servicioFacturaResponses.add(servicioFacturaResponse);
        }
        return servicioFacturaResponses;
    }

    public static List<DetalleServicioFactura> convertListDetailServicesDTOToDomain(List<DetalleServicioFacturaDTO> detalleServicioDTO) {
        List<DetalleServicioFactura> detalleServicioFacturas = new ArrayList<>();
        for (DetalleServicioFacturaDTO detalleServicioFacturaDTO : detalleServicioDTO) {
            DetalleServicioFactura detalleServicioFactura = new DetalleServicioFactura();
            detalleServicioFactura.setId(detalleServicioFacturaDTO.getId());
            detalleServicioFactura.setDescripcion(detalleServicioFacturaDTO.getDescripcion());
            detalleServicioFactura.setValor(detalleServicioFacturaDTO.getValor());
            detalleServicioFacturas.add(detalleServicioFactura);
        }
        return detalleServicioFacturas;
    }
}