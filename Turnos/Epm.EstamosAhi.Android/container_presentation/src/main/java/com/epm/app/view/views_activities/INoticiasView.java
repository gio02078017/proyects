package com.epm.app.view.views_activities;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public interface INoticiasView extends IBaseView {


    void showAlertDialogToLoadAgain(String name, int message);

    void showInformationNoticias(List<NoticiasEventos> noticiasEventos);

    void showAlertDialogToLoadAgain(int title_error, String message);
}
