package app.epm.com.contacto_transparente_presentation.presenters;

import com.epm.app.business_models.business_models.RepositoryError;

import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IHomeContactoTransparenteView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by Jquinterov on 3/10/2017.
 */

public class HomeContactoTransparentePresenter extends BasePresenter<IHomeContactoTransparenteView> {

    private ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic;

    public HomeContactoTransparentePresenter(ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic) {
        this.contactoTransparenteBusinessLogic = contactoTransparenteBusinessLogic;
    }

    public void validateInternetToGetListInterestGroup() {
        if (getValidateInternet().isConnected()) {
            createThreadToGetListInterestGroup();
        } else {
            getView().showAlertDialogLoadAgainOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToGetListInterestGroup() {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(this::getGruposDeInteres);
        thread.start();
    }

    public void getGruposDeInteres() {
        try {
            List<GrupoInteres> grupoInteresList = contactoTransparenteBusinessLogic.getGruposDeInteres();
            getView().callActivityDescribesAndSentDataForIntent(grupoInteresList);
        } catch (RepositoryError repositoryError) {
            if (repositoryError.getIdError() == Constants.UNAUTHORIZED_ERROR_CODE) {
                getView().showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
            } else {
                getView().showAlertDialogLoadAgainOnUiThread(R.string.title_error, repositoryError.getMessage());
            }
        } finally {
            getView().dismissProgressDialog();
        }

    }
}
