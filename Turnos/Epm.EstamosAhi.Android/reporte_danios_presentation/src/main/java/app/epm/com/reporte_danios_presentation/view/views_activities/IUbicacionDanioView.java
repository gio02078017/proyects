package app.epm.com.reporte_danios_presentation.view.views_activities;

import com.epm.app.app_utilities_presentation.views.views_activities.IBaseUbicacionDeFraudeOrDanioView;

/**
 * Created by josetabaresramirez on 2/02/17.
 */

public interface IUbicacionDanioView extends IBaseUbicacionDeFraudeOrDanioView {

    void showAlertDialogToStartDescribeDanioActivityOnUiThread(String title, String message, boolean hasCoberturEnergia, boolean hasCoberturaIluminaria);

    void getListTypeDanio(boolean hasCoberturEnergia, boolean hasCoberturaIluminaria);
}
