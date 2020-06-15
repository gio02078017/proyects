package app.epm.com.security_presentation.presenters;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.view.views_activities.IRegisterView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by mateoquicenososa on 21/11/16.
 */

public class RegisterPresenter extends BasePresenter<IRegisterView> {

    private SecurityBusinessLogic securityBusinessLogic;

    /**
     * Constructor
     *
     * @param securityBusinessLogic Clase con lógica de negocio.
     */
    public RegisterPresenter(SecurityBusinessLogic securityBusinessLogic) {
        this.securityBusinessLogic = securityBusinessLogic;
    }

    /**
     * Valida información de los campos.
     *
     * @param usuario usuario.
     */
    public void validateFieldsToRegister(Usuario usuario) {
        if (usuario.getCorreoElectronico().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
            return;
        }
        if (usuario.getNombres().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
            return;
        }
        if (usuario.getApellido().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
            return;
        }
        if (usuario.getIdTipoIdentificacion() == 0) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
            return;
        }
        if (usuario.getNumeroIdentificacion().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
            return;
        }
        if (usuario.getContrasenia().isEmpty()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
            return;
        }
        if (!usuario.isAceptoTerminosyCondiciones()) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
            return;
        }
        validateEmail(usuario);
    }

    /**
     * Valida información de caracteres del correo.
     *
     * @param usuario usuario.
     */
    public void validateEmail(Usuario usuario) {
        if (usuario.getCorreoElectronico().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)) {
            validateInternetToRegister(usuario);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        }
    }

    /**
     * Valida la conexión a internet.
     *
     * @param usuario registerUsuarioRequest
     */
    public void validateInternetToRegister(Usuario usuario) {
        if (getValidateInternet().isConnected()) {
            createThreadToregister(usuario);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     *
     * @param usuario registerUsuarioRequest
     */
    public void createThreadToregister(final Usuario usuario) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> register(usuario));
        thread.start();
    }


    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     *
     * @param usuario registerUsuarioRequest
     */
    public void register(Usuario usuario) {
        try {
            Mensaje mensaje = securityBusinessLogic.register(usuario);
            verifyRegister(mensaje);
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
     * Valida internet para iniciar los terminos y condiciones.
     */
    public void validateInternetToTermsAndConditionsRegister() {
        if (getValidateInternet().isConnected()) {
            getView().startTermsAndConditions();
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        }
    }

    /**
     * Muestra mensaje del registro devuelto por el servicio registro.
     *
     * @param mensaje mensaje
     */
    public void verifyRegister(Mensaje mensaje) {
        if (mensaje.getCode() == 1) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, mensaje.getText());
            getView().startFragmentIniciarSesion();
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, mensaje.getText());
        }
    }
}
