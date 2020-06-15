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
import app.epm.com.utilities.helpers.Validations;

/**
 * Created by ocadavid on 15/12/2016.
 */

public class FacturaBL {
    private IFacturaRepository facturaRepository;

    public FacturaBL(IFacturaRepository facturaRepository) {
        this.facturaRepository = facturaRepository;
    }

    public List<FacturaResponse> consulFacturaPorReferenteDePago(String number) throws RepositoryError {
        Validations.validateNullParameter(number);
        return facturaRepository.consultFacturaPorReferenteDePago(number);
    }

    public List<FacturaResponse> consulFacturaPorContrato(String number) throws RepositoryError {
        Validations.validateNullParameter(number);
        return facturaRepository.consultFacturaPorContrato(number);
    }

    public List<FacturaResponse> consulFacturasPorUsuario(String email) throws RepositoryError {
        Validations.validateNullParameter(email);
        return facturaRepository.consultFacturasPorUsuario(email);
    }

    public EstadoFacturaResponse validateFacturasPendientes(String numberReferente) throws RepositoryError {
        Validations.validateNullParameter(numberReferente);
        return facturaRepository.validateFacturasPendientes(numberReferente);
    }

    public List<ItemGeneral> consulEntidadesFinancieras() throws RepositoryError {
        return facturaRepository.consultEntidadesFinancieras();
    }

    public List<ServicioFacturaResponse> consulDetalleFacturas(String numberFactura) throws RepositoryError {
        Validations.validateNullParameter(numberFactura);
        return facturaRepository.consultDetalleFacturas(numberFactura);
    }

    /**
     * Consulta las facturas historicas de un contrato en el repositorio.
     * @param numberFactura
     * @return
     * @throws RepositoryError
     */
    public List<HistoricoFacturaResponse> consulHistorico(String numberFactura) throws RepositoryError {
        Validations.validateNullParameter(numberFactura);
        return facturaRepository.consultHistorico(numberFactura);
    }

    public Mensaje inscribirContrato(GestionContrato inscribirContrato) throws RepositoryError {
        Validations.validateNullParameter(inscribirContrato);
        Validations.validateNullParameter(inscribirContrato.getCorreoElectronico(), inscribirContrato.getContratos());
        Validations.validateEmptyParameter(inscribirContrato.getCorreoElectronico());
        return facturaRepository.inscribirContrato(inscribirContrato);
    }

    public InformacionPSE datosPagar(DataPagar dataPagar) throws RepositoryError {
        Validations.validateNullParameter(dataPagar);
        Validations.validateNullParameter(dataPagar.getEntidadFinanciera(), dataPagar.getIdTipoPersona(),
                dataPagar.getIdTipoDocumento(), dataPagar.getNumeroDocumento(), dataPagar.getFacturasPagar());
        return facturaRepository.datosPagar(dataPagar);
    }

    public Mensaje comprobantePago(ComprobantePago comprobantePago) throws RepositoryError {
        Validations.validateNullParameter(comprobantePago);
        Validations.validateNullParameter(comprobantePago.getCorreos(), comprobantePago.getValorTotalPago(),
                comprobantePago.getIdTransaccion(), comprobantePago.getCodigoTrazabilidad(),
                comprobantePago.getEstadoTransaccion(), comprobantePago.getNombreEntidadFinanciera(),
                comprobantePago.getDireccionIp(), comprobantePago.getFacturasPorTransaccion());
        return facturaRepository.comprobantePago(comprobantePago);
    }

    public TransaccionPSEResponse procesarInformacionPSE(ProcesarInformacionPSE procesarInformacionPSE) throws RepositoryError {
        Validations.validateNullParameter(procesarInformacionPSE);
        Validations.validateNullParameter(procesarInformacionPSE.getCodigoTrazabilidad(), procesarInformacionPSE.getEntidadFinanciera(),
                procesarInformacionPSE.getEstadoTransaccion(), procesarInformacionPSE.getIdTransaccion(),
                procesarInformacionPSE.getFacturasPorTransaccion());
        return facturaRepository.procesarInformacionPSE(procesarInformacionPSE);
    }

    public Mensaje updateContratos(GestionContrato infoContratos) throws RepositoryError {
        Validations.validateNullParameter(infoContratos);
        Validations.validateNullParameter(infoContratos.getCorreoElectronico(), infoContratos.getContratos());
        Validations.validateEmptyParameter(infoContratos.getCorreoElectronico());
        return facturaRepository.updateContratos(infoContratos);
    }

    public List<DataContratos> consulContratosInscritos(String email) throws RepositoryError{
        Validations.validateNullParameter(email);
        return facturaRepository.consultContratosInscritos(email);
    }

    /**
     * Consulta el detalle de la factura en el repositorio.
     * @param numberFactura
     * @return
     * @throws RepositoryError
     */
    public List<ServicioFacturaResponse> getDetailFactura(String numberFactura) throws RepositoryError {
        Validations.validateNullParameter(numberFactura);
        Validations.validateEmptyParameter(numberFactura);
        return facturaRepository.detailFactura(numberFactura);
    }
}