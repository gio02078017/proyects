package app.epm.com.reporte_fraudes_presentation.view.views_activities;

import com.epm.app.business_models.business_models.ServiciosMapa;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by ocadavid on 11/04/2017.
 */

public interface IServicesDeFraudesView extends IBaseView {

    void enableServices(ServiciosMapa servicesMap);

    void showAlertDialogTryAgain(String title, int message);

    void showAlertDialogTryAgain(int title_error, String message);
}
