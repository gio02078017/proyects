package app.epm.com.factura_presentation.presenters;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IGestionarContratosView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 20/12/16.
 */

public class GestionarContratosPresenter extends BasePresenter<IGestionarContratosView> {

    /**
     * Representa la clase del negocio.
     */
    private FacturaBL facturaBL;

    public GestionarContratosPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }

    /**
     * Actualiza la información de los contratos.
     *
     * @param getionContrato
     */
    public void updateContratos(final GestionContrato getionContrato) {
        if (getValidateInternet().isConnected()) {
                createThreadToUpdateContratos(getionContrato);
            }else{
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }


    /**
     * Crea hilo para realizar petición al servicio de UpdateContratos.
     * @param getionContrato
     */
    public void createThreadToUpdateContratos(final GestionContrato getionContrato) {
        getView().showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
        Thread thread = new Thread(() -> updateContratosBL(getionContrato));
        thread.start();
    }

    /**
     * Actualiza los contratos en el repositorio.
     * @param gestionContrato
     */
    public void updateContratosBL(GestionContrato gestionContrato) {
        try {
             Mensaje mensaje = facturaBL.updateContratos(gestionContrato);
            if(mensaje.getCode() == 74)
            {
                getView().showAlertDialogSaveInformation(getView().getName(), R.string.factura_gestionar_contratos_mensaje_actualizacion);
            }
            else {
                getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), mensaje.getText());
            }
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    /**
     * Consulta los contratos inscritos de un usuario.
     * @param email
     */
    public void consultContratosInscritos(final String email) {
        if (getValidateInternet().isConnected()) {
            createThreadToConsultContratosInscritos(email);
        }else{
            getView().showAlertDialogTryAgain(getView().getName(), R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio de ConsultContratosInscritos.
     * @param email
     */
    public void createThreadToConsultContratosInscritos(final String email) {

        getView().showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
        Thread thread = new Thread(() -> consultContratosInscritosBL(email));
        thread.start();
    }

    /**
     * Consulta los contratos inscritos en el repositorio.
     * @param email
     */
    public void consultContratosInscritosBL(final String email) {
        try {
            List<DataContratos> datosContratos = facturaBL.consulContratosInscritos(email);
            getView().loadContratosInscritos(datosContratos);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogTryAgain(R.string.title_error, repositoryError.getMessage(), R.string.text_intentar, R.string.text_cancelar);
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}
