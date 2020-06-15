package app.epm.com.security_presentation.presenters;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.security_domain.profile.ProfileBL;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.view.views_activities.IMyProfileView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 25/11/16.
 */

public class MyProfilePresenter extends BasePresenter<IMyProfileView> {

    ProfileBL profileBL;

    public MyProfilePresenter(ProfileBL profileBL) {
        this.profileBL = profileBL;
    }


    /**
     * Valida los datos obligatorios del usuario
     *
     * @param usuario datos del usuario
     */
    public void validateFieldsProfileRequired(Usuario usuario) {
        int message = -1;
        if (usuario.getNombres().isEmpty()) {
            message = R.string.text_empty_name;
            showAlertDialogGeneralInformation(message);
            return;
        }
        if (usuario.getApellido().isEmpty()) {
            message = R.string.text_empty_lastname;
            showAlertDialogGeneralInformation(message);
            return;
        }
        if (usuario.getIdTipoIdentificacion() == 0) {
            message = R.string.text_empty_type_document_profile;
            showAlertDialogGeneralInformation(message);
            return;
        }
        if (usuario.getNumeroIdentificacion().isEmpty()) {
            message = R.string.text_empty_number_document;
            showAlertDialogGeneralInformation(message);
            return;
        }
        if (message == -1) {
            validateEmail(usuario);
        }
    }

    public void showAlertDialogGeneralInformation(int message){
        getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, message);
    }

    /**
     * Valida el correo alternativo
     * @param usuario informacion del usuario
     */
    public void validateEmail(Usuario usuario) {
        if(!usuario.getCorreoAlternativo().isEmpty()){
            if(usuario.getCorreoAlternativo().matches(Constants.REGULAR_EXPRESSION_CORRECT_EMAIL)) {
                validateInternetToUpdateProfile(usuario);
            }else {
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_invalid_fields, R.string.text_invalid_email);
            }
        }else {
            validateInternetToUpdateProfile(usuario);
        }
        }

    /**
     * Permite validar el internet
     * @param usuario informacion del usuario
     */
    public void validateInternetToUpdateProfile(Usuario usuario) {
        if (getValidateInternet().isConnected()) {
            createThreadToUpdateProfile(usuario);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    /**
     *Permite crear el hilo para el metodo que consume el servicio
     * @param usuario informacion del usuario
     */
    public void createThreadToUpdateProfile(final Usuario usuario) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> updateProfile(usuario));
        thread.start();
    }

    /**
     * Permite ir al bl para comunicarse con el servicio
     * @param usuario informacion del usuario
     */
    public void updateProfile(Usuario usuario) {
        try {
            Mensaje mensaje = profileBL.updateProfile(usuario);
            getView().showAlertDialogGeneralInformationOnUiThread(usuario.getNombres(),mensaje.getText());
            getView().loadUserInfoInActivity();
        } catch (RepositoryError repositoryError) {
            if(repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE){
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            }else{
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
            }
        }finally {
            getView().dismissProgressDialog();
        }
    }
}

