package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.facturadomain.business_models.HistoricoFacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IHistoricoView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 19/12/16.
 */

public class HistoricoPresenter extends BasePresenter<IHistoricoView> {

    FacturaBL facturaBL;

    public HistoricoPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }

    /**
     * Consulta las facturas historicas de un contrato.
     *
     * @param numberContrato
     */
    public void consultHistorico(final String numberContrato) {
        if (getValidateInternet().isConnected()) {
            createThreadToConsultHistorico(numberContrato);
        } else {
            getView().showAlertDialogtryAgain(getView().getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar);
        }
    }


    /**
     * Crea hilo para realizar petición al servicio de ConsultHistorico.
     *
     * @param numberContrato
     */
    public void createThreadToConsultHistorico(final String numberContrato) {
        getView().showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
        Thread thread = new Thread(() -> consultHistoricoBL(numberContrato));
        thread.start();
    }

    /**
     * Consulta las facturas historicas de un contrato en el repositorio.
     *
     * @param numberContrato
     */
    public void consultHistoricoBL(final String numberContrato) {
        try {
            List<HistoricoFacturaResponse> listaHistoricoFacturas = facturaBL.consulHistorico(numberContrato);
            getView().loadHistorico(listaHistoricoFacturas);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                String message = (repositoryError.getIdError() == Constants.NOT_FOUND_ERROR_CODE) ? Constants.HISTORICO_MESSAGE : repositoryError.getMessage();
                getView().showAlertDialogtryAgain(R.string.title_error, message, R.string.text_intentar, R.string.text_cancelar);
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}