package app.epm.com.security_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;


import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.view.views_activities.IChangePasswordView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Clase intermediaria entre vista y el modelo del cambio de contraseña.
 * Created by ocadavid on 29/11/2016.
 */

public class ChangePasswordPresenter extends BasePresenter<IChangePasswordView>
{
    /**
     * Representa la clase del negocio.
     */
    private SecurityBusinessLogic securityBusinessLogic;

    /**
     * Constructor.
     * @param securityBusinessLogic Clase de negocio que contiene la lógica.
     */
    public ChangePasswordPresenter(SecurityBusinessLogic securityBusinessLogic) {
        this.securityBusinessLogic = securityBusinessLogic;
    }

    /**
     * Valida los campos.
     * @param usuarioRequest
     */
    public void validateFieldsToChangePassword(UsuarioRequest usuarioRequest) {
        if (usuarioRequest.getContrasenia().isEmpty() || usuarioRequest.getContraseniaNueva().isEmpty() ) {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_empty_password_current_password_new);
            return;
        }
        validateInternetToChangePassword(usuarioRequest);
    }

    /**
     * Valida la conexión a internet.
     * @param usuarioRequest
     */
    public void validateInternetToChangePassword(UsuarioRequest usuarioRequest) {
        if (getValidateInternet().isConnected()) {
            createThreadToChangePassword(usuarioRequest);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     * @param usuarioRequest
     */
    public void createThreadToChangePassword(final UsuarioRequest usuarioRequest) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> changePassword(usuarioRequest));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     * @param usuarioRequest
     */

    public void changePassword(UsuarioRequest usuarioRequest) {
        try {
            ChangePasswordResponse changePasswordResponse = securityBusinessLogic.changePassword(usuarioRequest);
            if (changePasswordResponse.getMensaje() == null) {
                getView().cleanFieldsTheChangePassword();
                getView().saveToken(changePasswordResponse.getToken());
                getView().showAlertDialogChangePasswordInformationOnUiThread(getView().getName(), R.string.message_successful_change_password);
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), changePasswordResponse.getMensaje().getText());
            }
        } catch (RepositoryError repositoryError) {
            if(repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE){
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            }else{
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}

