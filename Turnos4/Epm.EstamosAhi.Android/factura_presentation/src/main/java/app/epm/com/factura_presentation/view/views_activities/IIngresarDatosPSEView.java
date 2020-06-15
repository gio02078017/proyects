package app.epm.com.factura_presentation.view.views_activities;

import java.util.List;

import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 6/01/17.
 */

public interface IIngresarDatosPSEView extends IBaseView{

    /**
     * Guarda la información de PSE.
     * @param informacionPSE informacionPSE.
     */
    void saveInformacionPSE(InformacionPSE informacionPSE);

    /**
     * Inicia la pagina de PSE.
     * @param facturasPagar
     */
    void startPagePSE(List<FacturaResponse> facturasPagar);
}
