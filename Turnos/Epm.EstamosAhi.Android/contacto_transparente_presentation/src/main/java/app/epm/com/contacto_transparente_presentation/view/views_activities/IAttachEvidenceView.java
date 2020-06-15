package app.epm.com.contacto_transparente_presentation.view.views_activities;

import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by leidycarolinazuluagabastidas on 15/03/17.
 */

public interface IAttachEvidenceView extends IBaseView {

    void sendAttach(ItemUI itemUI);

    void showAlertDialogToGoHome(int title, ItemUI message);
}
