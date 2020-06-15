package app.epm.com.security_presentation.security;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.security_domain.business_models.ChangePasswordResponse;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.presenters.ChangePasswordPresenter;
import app.epm.com.security_presentation.view.views_activities.IChangePasswordView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by ocadavid on 29/11/2016.
 */


@RunWith(MockitoJUnitRunner.class)
public class ChangePasswordPresenterTest {

    ChangePasswordPresenter changePasswordPresenter;

    SecurityBusinessLogic securityBusinessLogic;

    @Mock
    ISecurityRepository securityRepository;

    @Mock
    IChangePasswordView changePasswordView;

    @Mock
    IValidateInternet validateInternet;

    @Before
    public void setUp() throws Exception {
        securityBusinessLogic = Mockito.spy(new SecurityBusinessLogic(securityRepository));
        changePasswordPresenter = Mockito.spy(new ChangePasswordPresenter(securityBusinessLogic));
        changePasswordPresenter.inject(changePasswordView, validateInternet);
    }

    private UsuarioRequest getUsuarioResponseInstance() {
        UsuarioRequest usuarioRequest = new UsuarioRequest();
        usuarioRequest.setCorreoElectronico("lcz97@live.com");
        usuarioRequest.setContrasenia("password");
        usuarioRequest.setContraseniaNueva("passwordNew");
        return usuarioRequest;
    }

    @Test
    public void methodValidateFieldsWithEmptyPasswordCurrentShouldShowAlertDialog() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setContrasenia(Constants.EMPTY_STRING);
        changePasswordPresenter.validateFieldsToChangePassword(usuarioRequest);

        verify(changePasswordView).showAlertDialogGeneralInformationOnUiThread(changePasswordView.getName(), R.string.text_empty_password_current_password_new);
        verify(changePasswordPresenter, never()).validateInternetToChangePassword(usuarioRequest);
    }

    @Test
    public void methodValidateFieldsWithEmptyPasswordNewShouldShowAlertDialog() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setContraseniaNueva(Constants.EMPTY_STRING);
        changePasswordPresenter.validateFieldsToChangePassword(usuarioRequest);

        verify(changePasswordView).showAlertDialogGeneralInformationOnUiThread(changePasswordView.getName(), R.string.text_empty_password_current_password_new);
        verify(changePasswordPresenter, never()).validateInternetToChangePassword(usuarioRequest);
    }

    @Test
    public void methodValidateFieldsToChangePasswordWithCorrectShouldCallMethodValidateInternet() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        changePasswordPresenter.validateFieldsToChangePassword(usuarioRequest);
        verify(changePasswordPresenter).validateInternetToChangePassword(usuarioRequest);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        changePasswordPresenter.validateInternetToChangePassword(usuarioRequest);
        verify(changePasswordView).showAlertDialogGeneralInformationOnUiThread(changePasswordView.getName(), R.string.text_validate_internet);
        verify(changePasswordPresenter, never()).createThreadToChangePassword(usuarioRequest);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToChangePassword() {
        when(validateInternet.isConnected()).thenReturn(true);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        changePasswordPresenter.validateInternetToChangePassword(usuarioRequest);
        verify(changePasswordPresenter).createThreadToChangePassword(usuarioRequest);
        verify(changePasswordView, never()).showAlertDialogGeneralInformationOnUiThread(changePasswordView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodCreateThreadToChangePasswordShouldShowProgressDialog() {
        changePasswordPresenter.createThreadToChangePassword(getUsuarioResponseInstance());
        verify(changePasswordView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodChangePasswordShouldCalMethodChangePasswordInSecurityBL() throws RepositoryError {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        changePasswordResponse.setMensaje(new Mensaje());
        when(securityRepository.changePassword(usuarioRequest)).thenReturn(changePasswordResponse);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(securityBusinessLogic).changePassword(usuarioRequest);
    }

    @Test
    public void methodChangePasswordShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.changePassword(usuarioRequest)).thenThrow(repositoryError);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(changePasswordView).showAlertDialogGeneralInformationOnUiThread(app.epm.com.security_presentation.R.string.title_error, repositoryError.getMessage());
    }

    @Test
    public void methodChangePasswordShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(securityRepository.changePassword(usuarioRequest)).thenThrow(repositoryError);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(changePasswordView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }

    @Test
    public void methodVerifyRegisterShoulShowAnAlertDialogWhenIsCorrect() throws RepositoryError {
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        Mensaje mensaje = null;
        changePasswordResponse.setMensaje(mensaje);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        when(securityRepository.changePassword(usuarioRequest)).thenReturn(changePasswordResponse);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(changePasswordView).showAlertDialogChangePasswordInformationOnUiThread(changePasswordView.getName(), R.string.message_successful_change_password);
    }

    @Test
    public void methodVerifyChangePasswordSaveToken() throws RepositoryError {
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        Mensaje mensaje = null;
        changePasswordResponse.setMensaje(mensaje);
        changePasswordResponse.setToken("Token");
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        when(securityRepository.changePassword(usuarioRequest)).thenReturn(changePasswordResponse);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(changePasswordView).saveToken(changePasswordResponse.getToken());
    }

    @Test
    public void methodVerifyChangePasswordCleanFieldsTheChangePassword() throws RepositoryError {
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        Mensaje mensaje = null;
        changePasswordResponse.setMensaje(mensaje);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        when(securityRepository.changePassword(usuarioRequest)).thenReturn(changePasswordResponse);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(changePasswordView).cleanFieldsTheChangePassword();
    }

    @Test
    public void methodVerifyRegisterShoulShowAnAlertDialogWhenIsIncorrect() throws RepositoryError {
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        Mensaje mensaje = new Mensaje();
        changePasswordResponse.setMensaje(mensaje);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        when(securityRepository.changePassword(usuarioRequest)).thenReturn(changePasswordResponse);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(changePasswordView).showAlertDialogGeneralInformationOnUiThread(changePasswordView.getName(), mensaje.getText());
    }

    @Test
    public void methodRegisterShouldCallMethodHideProgressDialog() throws RepositoryError {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        ChangePasswordResponse changePasswordResponse = new ChangePasswordResponse();
        Mensaje mensaje = new Mensaje();
        changePasswordResponse.setMensaje(mensaje);
        when(securityRepository.changePassword(usuarioRequest)).thenReturn(changePasswordResponse);
        changePasswordPresenter.changePassword(usuarioRequest);
        verify(changePasswordView).dismissProgressDialog();
    }
}

