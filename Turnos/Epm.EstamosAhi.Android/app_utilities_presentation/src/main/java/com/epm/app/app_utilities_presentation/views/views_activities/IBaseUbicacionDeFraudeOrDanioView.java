package com.epm.app.app_utilities_presentation.views.views_activities;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 19/04/17.
 */

public interface IBaseUbicacionDeFraudeOrDanioView extends IBaseView {

    void loadUbicationOnUiThread(InformacionDeUbicacion informacionDeUbicacion);

    void showAlertWithOutAddress(String name, int message);


}
