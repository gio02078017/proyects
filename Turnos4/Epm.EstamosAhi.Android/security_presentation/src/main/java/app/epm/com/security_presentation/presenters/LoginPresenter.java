package app.epm.com.security_presentation.presenters;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.view.views_activities.ILoginView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class LoginPresenter extends BasePresenter<ILoginView> {

    private SecurityBusinessLogic securityBusinessLogic;
    private String registrationIdOneSignal;
    private ICustomSharedPreferences customSharedPreferences;
    /**
     * Constructor.
     *
     * @param securityBusinessLogic Clase con lógica de negocio.
     */
    public LoginPresenter(SecurityBusinessLogic securityBusinessLogic) {
        this.securityBusinessLogic = securityBusinessLogic;
    }

    public void inject(ILoginView loginView, IValidateInternet validateInternet, ICustomSharedPreferences iCustomSharedPreferences) {
        setView(loginView);
        setValidateInternet(validateInternet);
        this.customSharedPreferences = iCustomSharedPreferences;
    }

    /**
     * Valida información de los campos.
     *
     * @param usuarioRequest usuarioRequest
     */
    public void validateFieldsToLogin(UsuarioRequest usuarioRequest) {
        if (usuarioRequest.getCorreoElectronico().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
            return;
        }
        if (usuarioRequest.getContrasenia().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
            return;
        }
        validateEmail(usuarioRequest);
    }

    /**
     * Valida información de caracteres del correo.
     *
     * @param usuarioRequest usuarioRequest.
     */
    public void validateEmail(UsuarioRequest usuarioRequest) {
        if (usuarioRequest.getCorreoElectronico().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)) {
            validateInternetToLogin(usuarioRequest);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        }
    }



    /**
     * Valida la conexión a internet.
     *
     * @param usuarioRequest usuarioRequest.
     */
    public void validateInternetToLogin(UsuarioRequest usuarioRequest) {
        if (getValidateInternet().isConnected()) {
            createThreadToLogin(usuarioRequest);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     *
     * @param usuarioRequest usuarioRequest.
     */
    public void createThreadToLogin(final UsuarioRequest usuarioRequest) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> login(usuarioRequest));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     *
     * @param usuarioRequest usuarioRequest.
     */
    public void login(UsuarioRequest usuarioRequest) {
        try {
            Usuario usuario = securityBusinessLogic.login(usuarioRequest,customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
            getView().saveToken(usuario.getToken());
            getView().startLanding(usuario);
        } catch (RepositoryError repositoryError) {
            if(repositoryError.getIdError() == Constants.FORBIDEN_ERROR_CODE){
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_active_account, R.string.text_active_account);
            }else if(repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE){
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_email_or_password_incorrect);
            }else{
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    /**
     * Valida informacion de campo para resetear contraseña.
     *
     * @param resetPasswordRequest resetPasswordRequest.
     */
    public void validateFieldToResetPassword(EmailUsuarioRequest resetPasswordRequest) {
        if (resetPasswordRequest.getCorreoElectronico().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
            return;
        }
        validateEmailToResetPassword(resetPasswordRequest);
    }

    /**
     *Valida información de caracteres del correo para resetear contraseña.
     *
     * @param resetPasswordRequest resetPasswordRequest.
     */
    public void validateEmailToResetPassword(EmailUsuarioRequest resetPasswordRequest) {
        if (resetPasswordRequest.getCorreoElectronico().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)){
            validateInternetToResetPassword(resetPasswordRequest);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        }
    }

    /**
     * Valida la conexión a internet para resetear contraseña.
     *
     * @param resetPasswordRequest resetPasswordRequest.
     */
    public void validateInternetToResetPassword(EmailUsuarioRequest resetPasswordRequest) {
        if (getValidateInternet().isConnected()) {
            createThreadToResetPassword(resetPasswordRequest);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio resetear contraseña.
     *
     * @param resetPasswordRequest resetPasswordRequest.
     */
    public void createThreadToResetPassword(final EmailUsuarioRequest resetPasswordRequest) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> resetPassword(resetPasswordRequest));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio resetear contraseña.
     *
     * @param resetPasswordRequest resetPasswordRequest.
     */
    public void resetPassword(EmailUsuarioRequest resetPasswordRequest) {
        try {
            Mensaje mensaje = securityBusinessLogic.resetPassword(resetPasswordRequest);
            resetPasswordResult(mensaje);
        } catch (RepositoryError repositoryError) {
            if(repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE){
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            }else{
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }

    /**
     * Muestra mensaje devuelto por el servicio resetear contraseña.
     *
     * @param mensaje mensaje
     */
    public void resetPasswordResult(Mensaje mensaje) {
        if (mensaje.getCode() == Constants.ONE) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_reset_password);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, mensaje.getText());
        }
    }



}
