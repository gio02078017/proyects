package app.epm.com.factura_presentation.view.views_activities;

import com.epm.app.business_models.business_models.Mensaje;

import java.util.List;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 20/12/16.
 */

public interface IInscribirContratoFacturaDigitalView extends IBaseView {

    /**
     * Inicia alerta con mensaje de inscribir contrato.
     * @param title title.
     *
     */
    void showAlertDialogToShowMessageFacturaInscritaSuccesOnUiThread(int title);
}
