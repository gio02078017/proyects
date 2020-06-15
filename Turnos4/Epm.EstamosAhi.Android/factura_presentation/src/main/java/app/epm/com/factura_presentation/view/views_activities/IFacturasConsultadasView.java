package app.epm.com.factura_presentation.view.views_activities;

import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 13/12/16.
 */

public interface IFacturasConsultadasView extends IBaseView {

    void setFacturaResponse(List<FacturaResponse> facturaResponseList);

    void showViewWithOutBill(List<FacturaResponse> facturaResponseList);

    void showAlertDialogTryAgain(String title, int message, int positive, int negative);

    void showAlertDialogTryAgain(int title, String message, int positive, int negative);

    void setValueBill(boolean estadoPendiente, int position);

    void goToPagoActivity();

    void openBrowser(boolean openBrowser);

    void showAlertDialogError(int position, int title, String message);
}
