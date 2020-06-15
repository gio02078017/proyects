package app.epm.com.factura_presentation.view.views_activities;

import java.util.List;

import app.epm.com.facturadomain.business_models.HistoricoFacturaResponse;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 19/12/16.
 */

public interface IHistoricoView extends IBaseView {

    void openBrowser(boolean openBrowser);

    void showAlertDialogtryAgain(String title, int message, int positive, int negative);

    void showAlertDialogtryAgain(int title, String message, int positive, int negative);

    void loadHistorico(List<HistoricoFacturaResponse> listaHistoricoFacturas);
}
