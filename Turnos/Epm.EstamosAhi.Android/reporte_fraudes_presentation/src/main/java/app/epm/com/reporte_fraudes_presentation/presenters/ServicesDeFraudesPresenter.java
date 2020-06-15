package app.epm.com.reporte_fraudes_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.reporte_fraudes_presentation.R;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IServicesDeFraudesView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

public class ServicesDeFraudesPresenter extends BasePresenter<IServicesDeFraudesView> {

    private final ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic;


    public ServicesDeFraudesPresenter(ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogicInstance) {
        this.reporteDeFraudesBusinessLogic = reporteDeFraudesBusinessLogicInstance;
    }

    public void validateInternetToGetServicesKML() {
        if (getValidateInternet().isConnected()) {
            createThreadGetServicesKML();
        } else {
            getView().showAlertDialogTryAgain(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadGetServicesKML() {
        getView().showProgressDIalog(R.string.text_services);
        Thread thread = new Thread(this::getServicesKML);
        thread.start();
    }

    public void getServicesKML() {
        try {
            ServiciosMapa serviciosMapa = reporteDeFraudesBusinessLogic.getServicioKML();
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
