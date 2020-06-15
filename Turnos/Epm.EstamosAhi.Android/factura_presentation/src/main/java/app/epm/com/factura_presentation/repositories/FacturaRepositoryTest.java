package app.epm.com.factura_presentation.repositories;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ItemGeneral;

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

/**
 * Created by ocadavid on 16/12/2016.
 */
public class FacturaRepositoryTest implements IFacturaRepository {

    @Override
    public List<FacturaResponse> consultFacturaPorReferenteDePago(String number) throws RepositoryError {
        return null;
    }

    @Override
    public List<FacturaResponse> consultFacturaPorContrato(String number) throws RepositoryError {
        return null;
    }

    @Override
    public List<FacturaResponse> consultFacturasPorUsuario(String email) throws RepositoryError {
        return  null;
    }

    @Override
    public EstadoFacturaResponse validateFacturasPendientes(String numberReferente) throws RepositoryError {
        return null;
    }

    @Override
    public List<ItemGeneral> consultEntidadesFinancieras() throws RepositoryError {
        return null;
    }

    @Override
    public List<ServicioFacturaResponse> consultDetalleFacturas(String numberFactura) throws RepositoryError {
        return null;
    }

    @Override
    public List<HistoricoFacturaResponse> consultHistorico(String numberContrato) throws RepositoryError {
        return null;
    }

    @Override
    public Mensaje inscribirContrato(GestionContrato inscribirContrato) throws RepositoryError {
        return null;
    }

    @Override
    public InformacionPSE datosPagar(DataPagar dataPagar) throws RepositoryError {
        return null;
    }

    @Override
    public Mensaje comprobantePago(ComprobantePago comprobantePago) throws RepositoryError {
        return null;
    }

    @Override
    public TransaccionPSEResponse procesarInformacionPSE(ProcesarInformacionPSE procesarInformacionPSE) throws RepositoryError {
        return null;
    }

    @Override
    public Mensaje updateContratos(GestionContrato infoContratos) throws RepositoryError {
        return null;
    }

    @Override
    public List<DataContratos> consultContratosInscritos(String email) throws RepositoryError {
        return null;
    }

    @Override
    public List<ServicioFacturaResponse> detailFactura(String numberFactura) throws RepositoryError {
        return null;
    }
}