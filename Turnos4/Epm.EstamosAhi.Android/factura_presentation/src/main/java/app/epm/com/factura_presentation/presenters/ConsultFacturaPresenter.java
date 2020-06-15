package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IConsultFacturaView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 12/12/16.
 */

public class ConsultFacturaPresenter extends BasePresenter<IConsultFacturaView> {

    /**
     * Representa la clase del negocio.
     */
    private FacturaBL facturaBL;

    public ConsultFacturaPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }

    /**
     * Consulta la factura por referente de pago.
     *
     * @param number
     */
    public void consulFacturaPorReferenteDePago(String number) {
        if (validateFieldsToConsultFactura(number)) {
            if (validateInternetToConsultFactura()) {
                createThreadToConsulFacturaPorReferenteDePago(number);
            }
        }
    }

    /**
     * Consulta la factura por contrato.
     *
     * @param number
     */
    public void consultFacturaPorContrato(String number) {
        if (validateFieldsToConsultFactura(number)) {
            if (validateInternetToConsultFactura()) {
                createThreadToConsulFacturaPorContrato(number);
            }
        }
    }

    /**
     * Valida que el campo para consultar la factura tenga valor.
     *
     * @param number
     * @return True si contiene información false en caso contrario.
     */
    public boolean validateFieldsToConsultFactura(String number) {
        if (number.isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_empty_fields, app.epm.com.security_presentation.R.string.text_digitar_numero);
            return false;
        }

        return true;
    }

    /**
     * Valida que exista conexión a internet.
     *
     * @return True si existe conexión a internet o false en caso contrario.
     */
    public boolean validateInternetToConsultFactura() {
        if (!getValidateInternet().isConnected()) {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), app.epm.com.security_presentation.R.string.text_validate_internet);
            return false;
        }
        return true;
    }

    /**
     * Crea hilo para realizar petición al servicio de ConsultarFacturaPorContrato.
     *
     * @param number
     */
    public void createThreadToConsulFacturaPorContrato(final String number) {
        getView().showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
        Thread thread = new Thread(() -> consulFacturaPorContratoBL(number));
        thread.start();
    }

    /**
     * Crea hilo para realizar petición al servicio de ConsulFacturaPorReferenteDePago.
     *
     * @param number
     */
    public void createThreadToConsulFacturaPorReferenteDePago(final String number) {
        getView().showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
        Thread thread = new Thread(() -> consulFacturaPorReferenteDePagoBL(number));
        thread.start();
    }

    /**
     * Consulta la factura por referente de pago en el repositorio.
     *
     * @param number
     */
    public void consulFacturaPorReferenteDePagoBL(String number) {
        try {
            List<FacturaResponse> facturasResponse = facturaBL.consulFacturaPorReferenteDePago(number);
            getView().startFacturasConsultadasActivity(facturasResponse);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.NOT_FOUND_ERROR_CODE) {
                getView().cleanFieldsTheConsulFactura();
                String tituloAlert = getView().getTitleFacturaNotExist(R.string.factura_lbl_alert_activity_consultar_factura_4, number, R.string.factura_lbl_alert_activity_consultar_factura_2);
                getView().showAlertDialogGeneralInformationOnUiThread(tituloAlert, R.string.factura_lbl_alert_activity_consultar_factura_5);
            } else if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            }else {
                getView().showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    /**
     * Consulta la factura por contrato en el repositorio.
     *
     * @param number
     */
    public void consulFacturaPorContratoBL(String number) {
        try {
            List<FacturaResponse> facturasResponse = facturaBL.consulFacturaPorContrato(number);
            getView().startFacturasConsultadasActivity(facturasResponse);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.NOT_FOUND_ERROR_CODE) {
                getView().cleanFieldsTheConsulFactura();
                String tituloAlert = getView().getTitleFacturaNotExist(R.string.factura_lbl_alert_activity_consultar_factura_1, number, R.string.factura_lbl_alert_activity_consultar_factura_2);
                getView().showAlertDialogGeneralInformationOnUiThread(tituloAlert, R.string.factura_lbl_alert_activity_consultar_factura_3);
            } else if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            }
            else {
                getView().showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}
