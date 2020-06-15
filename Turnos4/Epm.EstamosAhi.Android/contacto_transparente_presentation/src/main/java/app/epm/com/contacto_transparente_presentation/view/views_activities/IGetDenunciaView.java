package app.epm.com.contacto_transparente_presentation.view.views_activities;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by Jquinterov on 3/14/2017.
 */

public interface IGetDenunciaView extends IBaseView {

    void getIncidente(String codigoIncidente);

    void callActivityDescribesAndSentDataForIntent(Incidente incidente);

    void showAlertDialogOnUiThread(String name, String message);

}
