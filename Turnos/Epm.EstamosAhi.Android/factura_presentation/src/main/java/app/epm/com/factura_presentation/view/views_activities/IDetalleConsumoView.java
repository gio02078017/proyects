package app.epm.com.factura_presentation.view.views_activities;

import java.util.List;

import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 14/12/16.
 */

public interface IDetalleConsumoView extends IBaseView{

    void loadDetalleFacturas(List<ServicioFacturaResponse> listaServicioFacturas);

    void showAlertDialogToLoadAgain(String title, int message);

    void showAlertDialogToLoadAgain(int title, String message);
}
