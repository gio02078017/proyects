package app.epm.com.security_presentation.view.views_activities;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by ocadavid on 29/11/2016.
 */

public interface IChangePasswordView extends IBaseView {

    /**
     * Guarda el token.
     *
     * @param token token.
     */
    void saveToken(String token);

    /**
     * Limpia los campos del cambio de contraseña.
     */
    void cleanFieldsTheChangePassword();

    /**
     * Muestra el mensaje de exitoso en change password.
     * @param name
     * @param message
     */
    void showAlertDialogChangePasswordInformationOnUiThread(String name, int message);
}
