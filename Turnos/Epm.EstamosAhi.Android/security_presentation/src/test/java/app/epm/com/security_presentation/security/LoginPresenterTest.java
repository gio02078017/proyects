package app.epm.com.security_presentation.security;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.presenters.LoginPresenter;
import app.epm.com.security_presentation.view.views_activities.ILoginView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by josetabaresramirez on 16/11/16.
 */
@RunWith(MockitoJUnitRunner.class)
public class LoginPresenterTest {

    @Mock
    ILoginView loginView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ISecurityRepository securityRepository;

    @Mock
    ICustomSharedPreferences customSharedPreferences;

    LoginPresenter loginPresenter;

    SecurityBusinessLogic securityBusinessLogic;

    @Before
    public void setUp() throws Exception {
        securityBusinessLogic = Mockito.spy(new SecurityBusinessLogic(securityRepository));
        loginPresenter = Mockito.spy(new LoginPresenter(securityBusinessLogic));
        loginPresenter.inject(loginView, validateInternet, customSharedPreferences);
    }

    private UsuarioRequest getUsuarioResponseInstance() {
        UsuarioRequest usuarioRequest = new UsuarioRequest();
        usuarioRequest.setCorreoElectronico("lcz97@live.com");
        usuarioRequest.setContrasenia("test");
        return usuarioRequest;
    }

    private EmailUsuarioRequest getResetPasswordInstance() {
        EmailUsuarioRequest resetPasswordRequest = new EmailUsuarioRequest();
        resetPasswordRequest.setCorreoElectronico("quiceno1127@gmail.com");
        return resetPasswordRequest;
    }

    @Test
    public void methodValidateFieldsToLoginWithEmptyEmailShouldShowAlertDialog() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setCorreoElectronico(Constants.EMPTY_STRING);
        loginPresenter.validateFieldsToLogin(usuarioRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(loginPresenter, never()).validateEmail(usuarioRequest);
    }

    @Test
    public void methodValidateFieldsToLoginWithEMptyPasswordShouldShowAlertDialog() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setContrasenia(Constants.EMPTY_STRING);
        loginPresenter.validateFieldsToLogin(usuarioRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(loginPresenter, never()).validateEmail(usuarioRequest);
    }

    @Test
    public void methodValidateFieldsToLoginWithCorrectParametersShouldValidateEmail() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        loginPresenter.validateFieldsToLogin(usuarioRequest);

        verify(loginPresenter).validateEmail(usuarioRequest);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
    }

    @Test
    public void methodValidateEmailWithoutAtAndDomainShouldShowAlertDialog() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setCorreoElectronico("llll");
        loginPresenter.validateEmail(usuarioRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(loginPresenter, never()).validateInternetToLogin(usuarioRequest);
    }

    @Test
    public void methodValidateEmailWithoutAtShouldShowAlertDialog() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setCorreoElectronico("llll.com");
        loginPresenter.validateEmail(usuarioRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(loginPresenter, never()).validateInternetToLogin(usuarioRequest);
    }

    @Test
    public void methodValidateEmailWithoutDomainShouldShowAlertDialog() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        usuarioRequest.setCorreoElectronico("llll@");
        loginPresenter.validateEmail(usuarioRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(loginPresenter, never()).validateInternetToLogin(usuarioRequest);
    }

    @Test
    public void methodValidateEmailWithCorrectEmailShouldCallMethodValidateInternet() {
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        loginPresenter.validateEmail(usuarioRequest);
        verify(loginPresenter).validateInternetToLogin(usuarioRequest);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        loginPresenter.validateInternetToLogin(usuarioRequest);
        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        verify(loginPresenter, never()).createThreadToLogin(usuarioRequest);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToLogin() {
        when(validateInternet.isConnected()).thenReturn(true);
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        loginPresenter.validateInternetToLogin(usuarioRequest);
        verify(loginPresenter).createThreadToLogin(usuarioRequest);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodCreateThreadToLoginShouldShowProgressDialog() {
        loginPresenter.createThreadToLogin(getUsuarioResponseInstance());
        verify(loginView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodLoginShouldCalMethodLoginInSecurityBL() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) ;
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        Usuario usuario = new Usuario();
        usuario.setToken("sgg65456");
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenReturn(usuario);
        loginPresenter.login(usuarioRequest);
        verify(securityBusinessLogic).login(usuarioRequest, idOneSignal);
    }

    @Test
    public void methodLoginShouldCallMethodSaveToken() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) ;
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        Usuario usuario = new Usuario();
        usuario.setToken("sgg65456");
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenReturn(usuario);
        loginPresenter.login(usuarioRequest);
        verify(loginView).saveToken(usuario.getToken());
    }

    @Test
    public void methodLoginShouldCallMethodStartLanding() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) ;
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        Usuario usuario = new Usuario();
        usuario.setToken("sgg65456");
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenReturn(usuario);
        loginPresenter.login(usuarioRequest);
        verify(loginView).startLanding(usuario);
    }

    @Test
    public void methodLoginShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) ;
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenThrow(repositoryError);
        loginPresenter.login(usuarioRequest);
        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
    }

    @Test
    public void methodLoginShouldShowAnAlertDialogWhenReturnAnException403() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) ;
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.FORBIDEN_ERROR_CODE);
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenThrow(repositoryError);
        loginPresenter.login(usuarioRequest);
        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_active_account, R.string.text_active_account);
    }

    @Test
    public void methodLoginShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) ;
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenThrow(repositoryError);
        loginPresenter.login(usuarioRequest);
        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_email_or_password_incorrect);
    }

    @Test
    public void methodLoginShouldCallMethodHideProgressDialog() throws RepositoryError {
        String idOneSignal = customSharedPreferences.getString(Constants.ONE_SIGNAL_ID) ;
        UsuarioRequest usuarioRequest = getUsuarioResponseInstance();
        Usuario usuario = new Usuario();
        usuario.setToken("sgg65456");
        when(securityRepository.login(usuarioRequest, idOneSignal)).thenReturn(usuario);
        loginPresenter.login(usuarioRequest);
        verify(loginView).dismissProgressDialog();
    }

    @Test
    public void methodValidateEmailToResetPasswordWithEmptyEmailShouldShowAlertDialog() {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        resetPasswordRequest.setCorreoElectronico(Constants.EMPTY_STRING);
        loginPresenter.validateFieldToResetPassword(resetPasswordRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(loginPresenter, never()).validateEmailToResetPassword(resetPasswordRequest);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
    }

    @Test
    public void methodValidateEmailToResetPasswordWithCorrectParametersShouldValidateEmail() {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        loginPresenter.validateFieldToResetPassword(resetPasswordRequest);

        verify(loginPresenter).validateEmailToResetPassword(resetPasswordRequest);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
    }

    @Test
    public void methodValidateEmailToResetPasswordWithoutTheRegularExpressionShouldShowAlertDialog() {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        resetPasswordRequest.setCorreoElectronico("llll");
        loginPresenter.validateEmailToResetPassword(resetPasswordRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
    }

    @Test
    public void methodValidateEmailToResetPasswordCorrectShouldCallMethodValidateInternetTheResetPassword() {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        loginPresenter.validateEmailToResetPassword(resetPasswordRequest);

        verify(loginPresenter).validateEmailToResetPassword(resetPasswordRequest);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
    }

    @Test
    public void methodCreateThreadToResetPasswordShouldShowProgressDialog() {
        loginPresenter.createThreadToResetPassword(getResetPasswordInstance());
        verify(loginView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodValidateInternetTheResetPasswordLoginWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        loginPresenter.validateInternetToResetPassword(resetPasswordRequest);

        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.text_olvidaste_contrasenia, R.string.text_resetear_contrasenia);
        verify(loginPresenter, never()).createThreadToResetPassword(resetPasswordRequest);

    }

    @Test
    public void methodValidateInternetTheResetPasswordLoginWithoutConnectionShoudlCalllMethodCreateThreadToresetPasswprd() {
        when(validateInternet.isConnected()).thenReturn(true);
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        loginPresenter.validateInternetToResetPassword(resetPasswordRequest);
        verify(loginPresenter).createThreadToResetPassword(resetPasswordRequest);
        verify(loginView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodResetPasswordShouldCalMethodResetPasswordInSecurityBL() throws RepositoryError {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.resetPassword(resetPasswordRequest)).thenReturn(mensaje);
        loginPresenter.resetPassword(resetPasswordRequest);
        verify(securityBusinessLogic).resetPassword(resetPasswordRequest);
    }

    @Test
    public void methodResetPasswordShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.resetPassword(resetPasswordRequest)).thenThrow(repositoryError);
        loginPresenter.resetPassword(resetPasswordRequest);
        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
    }

    @Test
    public void methodResetPasswordShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(securityRepository.resetPassword(resetPasswordRequest)).thenThrow(repositoryError);
        loginPresenter.resetPassword(resetPasswordRequest);
        verify(loginView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }

    @Test
    public void methodResetPasswordShouldCallResetPasswordResult() throws RepositoryError {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.resetPassword(resetPasswordRequest)).thenReturn(mensaje);
        loginPresenter.resetPassword(resetPasswordRequest);
        verify(loginPresenter).resetPasswordResult(mensaje);
    }

    @Test
    public void methodResetPasswordResultShoulShowAnAlertDialogWhenIsCorrect() throws RepositoryError {
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(1);
        loginPresenter.resetPasswordResult(mensaje);
        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_reset_password);
    }

    @Test
    public void methodResetPasswordResultShoulShowAnAlertDialogWhenIsIncorrect() throws RepositoryError {
        Mensaje mensaje = new Mensaje();
        loginPresenter.resetPasswordResult(mensaje);
        verify(loginView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, mensaje.getText());
    }

    @Test
    public void methodResetPasswordShouldCallMethodHideProgressDialog() throws RepositoryError {
        EmailUsuarioRequest resetPasswordRequest = getResetPasswordInstance();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.resetPassword(resetPasswordRequest)).thenReturn(mensaje);
        loginPresenter.resetPassword(resetPasswordRequest);
        verify(loginView).dismissProgressDialog();
    }
}
