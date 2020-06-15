package app.epm.com.contacto_transparente_presentation.view.views_activities;

import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by Jquinterov on 3/10/2017.
 */

public interface IHomeContactoTransparenteView extends IBaseView {

    void showAlertDialogLoadAgainOnUiThread(String name, int message);

    void callActivityDescribesAndSentDataForIntent(List<GrupoInteres> grupoInteresList);

    void showAlertDialogLoadAgainOnUiThread(int title, String message);
}
