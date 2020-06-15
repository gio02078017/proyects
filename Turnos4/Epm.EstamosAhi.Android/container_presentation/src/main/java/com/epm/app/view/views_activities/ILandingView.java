package com.epm.app.view.views_activities;


import android.content.DialogInterface;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 30/11/16.
 */

public interface ILandingView extends IBaseView {

    void setDataUser(Authoken authoken);

    void showAlertDialogTryAgain (int title, String text, int positive, int negative, boolean loginGuest);

    void startLoginWhenToCloseSessionOnUiThread();

    void showAlertDialogTryAgain(int title, int message, int positive, int negative, boolean loginGuest);

    void startEspacioPromocional(InformacionEspacioPromocional informacionEspacioPromocional);

}
