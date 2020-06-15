package app.epm.com.factura_presentation.view.views_activities;

import com.epm.app.business_models.business_models.Mensaje;

import app.epm.com.facturadomain.business_models.TransaccionPSEResponse;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 11/01/17.
 */

public interface IComprobantePagoView extends IBaseView {

    /**
     * Abre la alerta cuando se envia el correo del comprobante de pago.
     */
    void showAlertDialogSendComprobantePago(Mensaje mensaje);


    /**
     * Guarda la respuesta de la transacción de PSE.
     * @param transaccionPSEResponse transaccionPSEResponse.
     */
    void saveTransaccionPSEResponse(TransaccionPSEResponse transaccionPSEResponse);

    void showAlertDialogGeneralLoadAgain(int titleError, String message);

    void showAlertDialogGeneralLoadAgain(String name, int text_validate_internet);
}
