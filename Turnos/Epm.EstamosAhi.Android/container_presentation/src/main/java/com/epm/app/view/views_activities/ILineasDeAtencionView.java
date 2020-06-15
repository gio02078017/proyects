package com.epm.app.view.views_activities;

import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by root on 28/03/17.
 */

public interface ILineasDeAtencionView extends IBaseView {

    void showAlertDialogToLoadAgainOnUiThread(String name, int message);

    void showAlertDialogToLoadAgainOnUiThread(int title, String message);

    void consultLineasDeAtencion(List<LineaDeAtencion> lineaDeAtencions);

    void showAlerDialogLineasDeAtencion(LineaDeAtencion lineaDeAtencion);
}
