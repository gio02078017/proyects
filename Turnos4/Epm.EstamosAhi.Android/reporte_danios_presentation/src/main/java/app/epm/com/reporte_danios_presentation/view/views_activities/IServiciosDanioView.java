package app.epm.com.reporte_danios_presentation.view.views_activities;

import com.epm.app.business_models.business_models.ServiciosMapa;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by leidycarolinazuluagabastidas on 2/02/17.
 */

public interface IServiciosDanioView extends IBaseView {

    void enableServices(ServiciosMapa serviciosMapa);

    void showAlertDialogTryAgain(String title, int message);

    void showAlertDialogTryAgain(int title_error, String message);
}
