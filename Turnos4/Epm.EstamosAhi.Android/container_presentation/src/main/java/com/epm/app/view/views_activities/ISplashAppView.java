package com.epm.app.view.views_activities;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 24/11/16.
 */

public interface ISplashAppView extends IBaseView {

    /**
     * Crea alerta en hilo con mensaje int.
     *
     * @param title title.
     * @param message message int.
     */
    void showAlertDialogToLoadAgainOnUiThread(int title, int message);

    /**
     * Crea alerta en hilo con mensaje string.
     *
     * @param title title.
     * @param message message string.
     */
    void showAlertDialogToLoadAgainOnUiThread(int title, String message);

    /**
     * Obtiene date.
     *
     * @return date.
     */
    String getCurrentDate();

    /**
     * Valida si el usuario esta logueado.
     */
    void validateUserIfHaveBeenLogin();

    /**
     * Oculta progressBar.
     */
    void hideProgressBarOnUiThread();

    void setDataUser(Authoken authoken);

    void validateUser();
}
