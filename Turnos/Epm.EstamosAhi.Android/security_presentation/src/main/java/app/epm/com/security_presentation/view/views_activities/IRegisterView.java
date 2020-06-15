package app.epm.com.security_presentation.view.views_activities;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 21/11/16.
 */

public interface IRegisterView extends IBaseView {

    /**
     * Inicia la actividad de Términos y condiciones.
     */
    void startTermsAndConditions();

    /**
     * Inicia el fragmento de Iniciar sesión.
     */
    void startFragmentIniciarSesion();

    void cleanFields();
}
