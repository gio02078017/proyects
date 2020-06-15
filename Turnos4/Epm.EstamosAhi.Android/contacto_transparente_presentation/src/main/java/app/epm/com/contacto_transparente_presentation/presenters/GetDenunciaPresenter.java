package app.epm.com.contacto_transparente_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IGetDenunciaView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by Jquinterov on 3/14/2017.
 */

public class GetDenunciaPresenter extends BasePresenter<IGetDenunciaView> {

    private ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic;

    public GetDenunciaPresenter(ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic) {
        this.contactoTransparenteBusinessLogic = contactoTransparenteBusinessLogic;
    }

    public void validateInternetToGetIncidente(String codigoIncidente) {
        if (getValidateInternet().isConnected()) {
            createThreadToGetIncidente(codigoIncidente);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToGetIncidente(final String codigoIncidente) {
        getView().showProgressDIalog(R.string.text_please_wait);
        new Thread(() -> getIncidente(codigoIncidente)).start();
    }

    public void getIncidente(String codigoIncidente) {
        try {
            Incidente incidente = contactoTransparenteBusinessLogic.getIncidente(codigoIncidente);
            getView().callActivityDescribesAndSentDataForIntent(incidente);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else if (repositoryError.getMessage().contains(Constants.NOT_FOUND)) {
                getView().showAlertDialogOnUiThread(getView().getName(), repositoryError.getMessage());
            } else {
                getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }
    }
}
