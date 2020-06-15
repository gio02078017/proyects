package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.RepositoryError;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.facturadomain.business_models.EstadoFacturaResponse;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IFacturasConsultadasView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 13/12/16.
 */

public class FacturasConsultadasPresenter extends BasePresenter<IFacturasConsultadasView> {

    FacturaBL facturaBL;
    private ICustomSharedPreferences customSharedPreferences;

    public FacturasConsultadasPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }


    public void inject(IFacturasConsultadasView facturasConsultadasView, IValidateInternet validateInternet, ICustomSharedPreferences customSharedPreferences) {
        setView(facturasConsultadasView);
        setValidateInternet(validateInternet);
        this.customSharedPreferences = customSharedPreferences;
    }

    public void validateInternetToConsultBill(final String correoElectronico) {
        if (getValidateInternet().isConnected()) {
            createThreadToConsultBill(correoElectronico);
        } else {
            getView().showAlertDialogTryAgain(getView().getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar);
        }
    }


    public void createThreadToConsultBill(final String correoElectronico) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> consultBill(correoElectronico));
        thread.start();
    }

    public void consultBill(final String correoElectronico) {
        List<FacturaResponse> facturaResponseList = new ArrayList<>();
        try {
            facturaResponseList.addAll(facturaBL.consulFacturasPorUsuario(correoElectronico));
            if (facturaResponseList.isEmpty()) {
                getView().showViewWithOutBill(facturaResponseList);
            } else {
                getView().setFacturaResponse(facturaResponseList);
            }
        } catch (Exception repositoryError) {
            if (repositoryError.getMessage().contains("404")) {
                getView().showViewWithOutBill(facturaResponseList);
            } else if (repositoryError.getMessage().contains("401")) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else  {
                String message = ((repositoryError.getMessage().contains("timeout")) ? Constants.REQUEST_TIMEOUT_ERROR_MESSAGE: Constants.DEFAUL_ERROR);
                getView().showAlertDialogTryAgain(R.string.title_error, message, R.string.text_intentar, R.string.text_cancelar);
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    public void consultStatusPendingBill(int position, String referencia) {
        validateInternetTOConsultStatusPendingBill(position, referencia);
    }

    public void validateInternetTOConsultStatusPendingBill(int posiiton, String referencia) {
        if (getValidateInternet().isConnected()) {
            createThreadToConsultStatusPendingBill(posiiton, referencia);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToConsultStatusPendingBill(final int position, final String referencia) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> consultStatusPending(position, referencia));
        thread.start();
    }

    public void consultStatusPending(int position, String referencia) {

        try {
            EstadoFacturaResponse estadoFacturaResponse = facturaBL.validateFacturasPendientes(referencia);
            getView().setValueBill(estadoFacturaResponse.isEstado(), position);
        } catch (RepositoryError repositoryError) {
            getView().showAlertDialogError(position, R.string.title_error, repositoryError.getMessage());
        } finally {
            getView().dismissProgressDialog();
        }
    }

    public void validateInternetListEntityFinancial() {
        if (getValidateInternet().isConnected()) {
            createThreadTogetListEntityFinancial();
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }


    }

    public void createThreadTogetListEntityFinancial() {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(this::getListEntityFinancial);
        thread.start();

    }

    public void getListEntityFinancial() {
        try {
            List<ItemGeneral> itemGeneralResponseList = facturaBL.consulEntidadesFinancieras();
            customSharedPreferences.addSetArray(Constants.ENTITYFINANCIAL, itemGeneralResponseList);
            getView().goToPagoActivity();
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}
