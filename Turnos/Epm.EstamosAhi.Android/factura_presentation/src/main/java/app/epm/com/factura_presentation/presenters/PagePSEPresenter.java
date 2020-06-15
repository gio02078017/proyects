package app.epm.com.factura_presentation.presenters;

import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.factura_presentation.R;
import app.epm.com.factura_presentation.view.views_activities.IPagePSEView;
import app.epm.com.utilities.presenters.BasePresenter;

/**
 * Created by mateoquicenososa on 10/01/17.
 */

public class PagePSEPresenter extends BasePresenter<IPagePSEView> {

    private FacturaBL facturaBL;

    /**
     * Constructor
     *
     * @param facturaBL Clase con lógica de negocio.
     */
    public PagePSEPresenter(FacturaBL facturaBL) {
        this.facturaBL = facturaBL;
    }

    public void validateInternetToPagePSE() {
        if (getValidateInternet().isConnected()) {
            createThreadToPagePSE();
        } else {
            getView().showAlertDialogTryAgainIternet();
        }
    }

    public void createThreadToPagePSE() {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(this::startPagePSE);
        thread.start();
    }

    public void startPagePSE() {
        getView().openPagePSE();
    }
}