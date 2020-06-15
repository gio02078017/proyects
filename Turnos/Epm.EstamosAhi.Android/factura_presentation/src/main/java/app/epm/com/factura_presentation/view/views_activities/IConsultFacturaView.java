package app.epm.com.factura_presentation.view.views_activities;

import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 12/12/16.
 */

public interface IConsultFacturaView extends IBaseView {

    void cleanFieldsTheConsulFactura();

    String getTitleFacturaNotExist(int textoUno, String number, int textoDos);

    void startFacturasConsultadasActivity(List<FacturaResponse> facturasResponse);
}
