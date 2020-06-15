package com.epm.app.menu_presentation.MenuFragment;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.menu_presentation.R;
import com.epm.app.menu_presentation.presenters.MenuFragmentPresenter;
import com.epm.app.menu_presentation.view.view_activity.IMenuFragmentView;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Matchers.eq;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by juan on 11/04/17.
 */
@RunWith(MockitoJUnitRunner.class)
public class MenuFragmentPresenterTest {
    @Mock
    ICustomSharedPreferences customSharedPreferences;

    @Mock
    ISecurityRepository securityRepository;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IMenuFragmentView menuFragmentView;

    @InjectMocks
    Usuario user;

    MenuFragmentPresenter menuFragmentPresenter;

    SecurityBusinessLogic securityBusinessLogic;

    @Before
    public void setUp() throws Exception {
        securityBusinessLogic = Mockito.spy(new SecurityBusinessLogic(securityRepository));
        menuFragmentPresenter = Mockito.spy(new MenuFragmentPresenter(securityBusinessLogic));
        menuFragmentPresenter.inject(menuFragmentView, validateInternet, customSharedPreferences);
    }

    /**
     * Start LogOut
     */

    private EmailUsuarioRequest getLogOutInstance() {
        EmailUsuarioRequest emailUsuarioRequest = new EmailUsuarioRequest();
        emailUsuarioRequest.setCorreoElectronico("jqpipe@gmail.com");
        return emailUsuarioRequest;
    }
    @Test
    public void methodValidateInternetWithoutConnectionShouldShowAlertDialog(){
        String name = "name";
        user.setNombres(name);
        when(validateInternet.isConnected()).thenReturn(false);
        EmailUsuarioRequest emailUsuarioRequest = getLogOutInstance();
        menuFragmentPresenter.validateInternetLogOut(emailUsuarioRequest,user);
        verify(menuFragmentView).showAlertDialogGeneralInformationOnUiThread(eq(name), eq(R.string.text_validate_internet));
    }

    @Test
    public void methodValidateInternetLogOutWithConnectionShouldCallMethodCreateThreadToLogOut() {
        when(validateInternet.isConnected()).thenReturn(true);
        EmailUsuarioRequest emailUsuarioRequest = getLogOutInstance();
        menuFragmentPresenter.validateInternetLogOut(emailUsuarioRequest,user);
        verify(menuFragmentPresenter).createThreadToLogOut(emailUsuarioRequest);
        verify(menuFragmentView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodethodCreateThreadToLogOutShouldShowProgressDialog() {
        menuFragmentPresenter.createThreadToLogOut(getLogOutInstance());
        verify(menuFragmentView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodLogOutShouldCallMethodLogOutInSecurityBL() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID);
        EmailUsuarioRequest emailUsuarioRequest = getLogOutInstance();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.logOut(emailUsuarioRequest, idOneSignal)).thenReturn(mensaje);
        menuFragmentPresenter.logOut(emailUsuarioRequest);
        verify(securityRepository).logOut(emailUsuarioRequest, customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
        verify(menuFragmentView).dismissProgressDialog();
    }

    @Test
    public void methodLoginShouldCallMethodStartLoginWhenToCloseSession() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID);
        EmailUsuarioRequest emailUsuarioRequest = getLogOutInstance();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.logOut(emailUsuarioRequest, idOneSignal)).thenReturn(mensaje);
        menuFragmentPresenter.logOut(emailUsuarioRequest);
        verify(menuFragmentView).startLoginWhenToCloseSessionOnUiThread();
        verify(menuFragmentView).dismissProgressDialog();
    }

    @Test
    public void methodLogOutShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID);
        EmailUsuarioRequest emailUsuarioRequest = getLogOutInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.logOut(emailUsuarioRequest, idOneSignal)).thenThrow(repositoryError);
        menuFragmentPresenter.logOut(emailUsuarioRequest);
        verify(menuFragmentView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
        verify(menuFragmentView).dismissProgressDialog();
    }

    @Test
    public void methodLogOutShouldCallMethodHideProgressDialog() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID);
        EmailUsuarioRequest emailUsuarioRequest = getLogOutInstance();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.logOut(emailUsuarioRequest, idOneSignal)).thenReturn(mensaje);
        menuFragmentPresenter.logOut(emailUsuarioRequest);
        verify(menuFragmentView).dismissProgressDialog();
    }

    /**
     * End LogOut.
     */


}
