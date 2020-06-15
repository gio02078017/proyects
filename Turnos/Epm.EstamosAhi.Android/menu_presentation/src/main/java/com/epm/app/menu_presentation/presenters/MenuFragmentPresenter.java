package com.epm.app.menu_presentation.presenters;

import android.content.DialogInterface;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.menu_presentation.R;
import com.epm.app.menu_presentation.view.view_activity.IMenuFragmentView;

import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by juan on 11/04/17.
 */

public class MenuFragmentPresenter extends BasePresenter<IMenuFragmentView> {

    private SecurityBusinessLogic securityBusinessLogic;
    private ICustomSharedPreferences customSharedPreferences;

    public MenuFragmentPresenter(SecurityBusinessLogic securityBusinessLogic) {
        this.securityBusinessLogic = securityBusinessLogic;
    }


    public void inject(IMenuFragmentView menuFragmentView, IValidateInternet validateInternet, ICustomSharedPreferences customSharedPreferences) {
        setView(menuFragmentView);
        setValidateInternet(validateInternet);
        this.customSharedPreferences = customSharedPreferences;
    }

    /**
     * Valida la conexión a internet.
     *
     * @param emailUsuarioRequest emailUsuarioRequest.
     */
    public void validateInternetLogOut(final EmailUsuarioRequest emailUsuarioRequest, Usuario user) {
        if (getValidateInternet().isConnected()){
            createThreadToLogOut(emailUsuarioRequest);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(user.getNombres(), R.string.text_validate_internet);
        }
    }

    /**
     * Crea hilo para realizar petición al servicio.
     *
     * @param emailUsuarioRequest emailUsuarioRequest.
     */
    public void createThreadToLogOut(final EmailUsuarioRequest emailUsuarioRequest) {
        getView().showProgressDIalog(R.string.text_please_wait);
        Thread thread = new Thread(() -> logOut(emailUsuarioRequest));
        thread.start();
    }

    /**
     * Llama a la clase del negocio para comunicarse con el servicio.
     *
     * @param emailUsuarioRequest emailUsuarioRequest.
     */
    public void logOut(EmailUsuarioRequest emailUsuarioRequest) {
        try{
            Mensaje mensaje = securityBusinessLogic.logOut(emailUsuarioRequest,customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
            getView().startLoginWhenToCloseSessionOnUiThread();
        } catch (RepositoryError repositoryError) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
        }
        finally {
            getView().dismissProgressDialog();
        }
    }
}
