package app.epm.com.reporte_danios_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.view.views_activities.IServiciosDanioView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 2/02/17.
 */

public class ServiciosDaniosPresenter extends BasePresenter<IServiciosDanioView> {

    private DaniosBL daniosBL;


    public ServiciosDaniosPresenter(DaniosBL daniosBL) {
        this.daniosBL = daniosBL;
    }



    public void validateInternetToGetServicesKML() {
        if (!(getValidateInternet().isConnected())) {
            getView().showAlertDialogTryAgain(getView().getName(), R.string.text_validate_internet);
        } else {
            createThreadGetServicesKML();
        }

    }

    public void createThreadGetServicesKML() {
        getView().showProgressDIalog(R.string.text_services);
        Thread thread = new Thread(this::getServicesKML);
        thread.start();
    }

    public void getServicesKML() {
        try {
            ServiciosMapa serviciosMapa = daniosBL.getServicesKML();
            getView().enableServices(serviciosMapa);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogTryAgain(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }

    }
}
