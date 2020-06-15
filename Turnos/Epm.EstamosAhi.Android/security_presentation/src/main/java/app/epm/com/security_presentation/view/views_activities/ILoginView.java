package app.epm.com.security_presentation.view.views_activities;


import com.epm.app.business_models.business_models.Usuario;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public interface ILoginView extends IBaseView {

    /**
     * Guarda el token.
     *
     * @param token token.
     */
    void saveToken(String token);

    /**
     * Inicia la actividad landing.
     *
     * @param usuario usuario.
     */
    void startLanding(Usuario usuario);

    void cleanFields();
}
