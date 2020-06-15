package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IInscribirContratoFacturaDigitalView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 20/12/16.
 */

public class InscribirContratoFacturaDigitalPresenter extends BasePresenter<IInscribirContratoFacturaDigitalView> {
    private FacturaBL facturaBL;

    /**
     * Constructor
     *
     * @param facturaBL Clase con lógica de negocio.
     */
    public InscribirContratoFacturaDigitalPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }

    /**
     * Valida la conexión a internet.
     *
     * @param inscribirContrato inscribirContrato.
     */
    public void validateInternetInscribirContratoFacturaDigital(GestionContrato inscribirContrato) {
        if (getValidateInternet().isConnected()) {
            createThreadInscribirContratoFacturaDigital(inscribirContrato);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     *
     * @param inscribirContrato inscribirContrato.
     */
    public void createThreadInscribirContratoFacturaDigital(final GestionContrato inscribirContrato) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> inscribirContrato(inscribirContrato));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     *
     * @param inscribirContrato inscribirContrato.
     */
    public void inscribirContrato(GestionContrato inscribirContrato) {
        try {
            Mensaje mensaje = facturaBL.inscribirContrato(inscribirContrato);
            if (mensaje.getCode() == Constants.CONTRATO_INSCRITO) {
                getView().showAlertDialogToShowMessageFacturaInscritaSuccesOnUiThread(R.string.text_contrato_inscrito);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, mensaje.getText());
            }
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
