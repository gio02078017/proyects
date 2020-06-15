package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IDetalleConsumoView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 14/12/16.
 */

public class DetalleConsumoPresenter extends BasePresenter<IDetalleConsumoView> {
    FacturaBL facturaBL;

    public DetalleConsumoPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }


    /**
     * Valida la conexion a internet
     *
     * @param numberFactura numero de la factura
     */
    public void validateInternetToGetDetailFactura(final String numberFactura) {
        if (getValidateInternet().isConnected()) {
            createThreadToGetDetailFactura(numberFactura);
        } else {
            getView().showAlertDialogToLoadAgain(getView().getName(), R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio de detailFactura
     *
     * @param numberFactura numero de factura
     */
    public void createThreadToGetDetailFactura(final String numberFactura) {
        getView().showProgressDIalog(R.string.text_please_wait);
        new Thread(() -> getDetailFactura(numberFactura)).start();
    }

    /**
     * Consulta el detalle de la factura en el repositorio
     *
     * @param numberFactura
     */
    public void getDetailFactura(final String numberFactura) {
        try {
            List<ServicioFacturaResponse> servicioFacturaResponses = facturaBL.getDetailFactura(numberFactura);
            getView().loadDetalleFacturas(servicioFacturaResponses);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                String message = (repositoryError.getIdError() == Constants.NOT_FOUND_ERROR_CODE) ? Constants.DETALLE_MESSAGE : repositoryError.getMessage();
                getView().showAlertDialogToLoadAgain(R.string.title_error, message);
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}


