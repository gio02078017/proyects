package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.facturadomain.business_models.ComprobantePago;
import app.epm.com.facturadomain.business_models.ProcesarInformacionPSE;
import app.epm.com.facturadomain.business_models.TransaccionPSEResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IComprobantePagoView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 11/01/17.
 */

public class ComprobantePagoPresenter extends BasePresenter<IComprobantePagoView> {

    private FacturaBL facturaBL;

    /**
     * Constructor
     *
     * @param facturaBL Clase con lógica de negocio.
     */
    public ComprobantePagoPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }

    /**
     * Valida la conexión a internet.
     *
     * @param procesarInformacionPSE procesarInformacionPSE.
     */
    public void validateInternetProcesarInformacionPSE(ProcesarInformacionPSE procesarInformacionPSE) {
        if (getValidateInternet().isConnected()) {
            createThreadProcesarInformacionPSE(procesarInformacionPSE);
        } else {
            getView().showAlertDialogGeneralLoadAgain(getView().getName(), R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     *
     * @param procesarInformacionPSE procesarInformacionPSE.
     */
    public void createThreadProcesarInformacionPSE(final ProcesarInformacionPSE procesarInformacionPSE) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> procesarInformacionPSE(procesarInformacionPSE));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     *
     * @param procesarInformacionPSE procesarInformacionPSE.
     */
    public void procesarInformacionPSE(ProcesarInformacionPSE procesarInformacionPSE) {
        try {
            TransaccionPSEResponse transaccionPSEResponse = facturaBL.procesarInformacionPSE(procesarInformacionPSE);
            getView().saveTransaccionPSEResponse(transaccionPSEResponse);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogGeneralLoadAgain(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    /**
     * Valida la información del campo correo.
     *
     * @param comprobantePago comprobantePago.
     */
    public void validateEmailToSendComprobante(ComprobantePago comprobantePago) {
        if (comprobantePago.getCorreos().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        } else {
            validateParametersToSendComprobante(comprobantePago);
        }
    }

    /**
     * Valida información de caracteres del correo.
     *
     * @param comprobantePago comprobantePago.
     */
    public void validateParametersToSendComprobante(ComprobantePago comprobantePago) {
        if (comprobantePago.getCorreos().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)) {
            validateInternetToSendComprobante(comprobantePago);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        }
    }

    /**
     * Valida la conexión a internet.
     *
     * @param comprobantePago comprobantePago.
     */
    public void validateInternetToSendComprobante(ComprobantePago comprobantePago) {
        if (getValidateInternet().isConnected()) {
            createThreadToSendComprobante(comprobantePago);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     *
     * @param comprobantePago comprobantePago.
     */
    public void createThreadToSendComprobante(final ComprobantePago comprobantePago) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> comprobantePago(comprobantePago));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     *
     * @param comprobantePago comprobantePago.
     */
    public void comprobantePago(ComprobantePago comprobantePago) {
        try {
            Mensaje mensaje = facturaBL.comprobantePago(comprobantePago);
            getView().showAlertDialogSendComprobantePago(mensaje);
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