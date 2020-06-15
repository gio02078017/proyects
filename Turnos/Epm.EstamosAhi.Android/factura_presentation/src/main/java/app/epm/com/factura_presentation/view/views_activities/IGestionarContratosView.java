package app.epm.com.factura_presentation.view.views_activities;

import java.util.List;

import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 20/12/16.
 */

public interface IGestionarContratosView extends IBaseView{

    void loadContratosInscritos(List<DataContratos> datosContratos);

    void showAlertDialogTryAgain(String title, int message, int positive, int negative);

    void showAlertDialogTryAgain(int title, String message, int positive, int negative);

    void sendFacturaResponse();

    void showAlertDialogSaveInformation(String name, int message);
}
