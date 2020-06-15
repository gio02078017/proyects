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

import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.presenters.RegisterPresenter;
import app.epm.com.security_presentation.view.views_activities.IRegisterView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 21/11/16.
 */
@RunWith(MockitoJUnitRunner.class)
public class RegisterPresenterTest {
    @Mock
    IRegisterView registerView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ISecurityRepository securityRepository;

    @Mock
    ICustomSharedPreferences customSharedPreferences;

    RegisterPresenter registerPresenter;

    SecurityBusinessLogic securityBusinessLogic;

    @Before
    public void setUp() throws Exception {
        securityBusinessLogic = Mockito.spy(new SecurityBusinessLogic(securityRepository));
        registerPresenter = Mockito.spy(new RegisterPresenter(securityBusinessLogic));
        registerPresenter.inject(registerView, validateInternet);
    }

    private Usuario getRegisterUsuarioRequest() {
        Usuario registerUsuarioRequest = new Usuario();
        registerUsuarioRequest.setCorreoElectronico("quiceno1127@gmail.com");
        registerUsuarioRequest.setNombres("Mateo");
        registerUsuarioRequest.setApellido("Quiceno Sosa");
        registerUsuarioRequest.setIdTipoIdentificacion(1);
        registerUsuarioRequest.setNumeroIdentificacion("1017253093");
        registerUsuarioRequest.setContrasenia("Mqs1017253093.");
        registerUsuarioRequest.setAceptoTerminosyCondiciones(true);
        return registerUsuarioRequest;
    }

    @Test
    public void methodValidateFieldsToRegisterWithEmptyEmailShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setCorreoElectronico(Constants.EMPTY_STRING);
        registerPresenter.validateFieldsToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
    }

    @Test
    public void methodValidateFieldsToRegisterWithEmptyNameShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setNombres(Constants.EMPTY_STRING);
        registerPresenter.validateFieldsToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
    }

    @Test
    public void methodValidateFieldsToRegisterWithEmptyLastNameShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setApellido(Constants.EMPTY_STRING);
        registerPresenter.validateFieldsToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
    }

    @Test
    public void methodValidateFieldsToRegisterWithEmptyTypeDocumentShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setIdTipoIdentificacion(0);
        registerPresenter.validateFieldsToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
    }

    @Test
    public void methodValidateFieldsToRegisterWithEmptyNumberDocumentShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setNumeroIdentificacion(Constants.EMPTY_STRING);
        registerPresenter.validateFieldsToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
    }

    @Test
    public void methodValidateFieldsToRegisterWithEmptyPasswordShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setContrasenia(Constants.EMPTY_STRING);
        registerPresenter.validateFieldsToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
    }

    @Test
    public void methodValidateFieldsToRegisterWithEmptyAcceptTermsAndConditionsShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setAceptoTerminosyCondiciones(false);
        registerPresenter.validateFieldsToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_false_terms_and_conditions);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_password);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
    }

    @Test
    public void methodtttttWithoutAtAndDomainShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setCorreoElectronico("llll");
        registerPresenter.validateEmail(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodValidateEmailWithoutAtShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setCorreoElectronico("llll.com");
        registerPresenter.validateEmail(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodValidateEmailWithoutDomainShouldShowAlertDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerUsuarioRequest.setCorreoElectronico("llll@");
        registerPresenter.validateEmail(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodValidateEmailWithCorrectEmailShouldCallMethodValidateInternet() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerPresenter.validateEmail(registerUsuarioRequest);
        verify(registerPresenter).validateInternetToRegister(registerUsuarioRequest);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerPresenter.validateInternetToRegister(registerUsuarioRequest);

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_incorrect_email);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToRegister() {
        when(validateInternet.isConnected()).thenReturn(true);
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerPresenter.validateInternetToRegister(registerUsuarioRequest);

        verify(registerPresenter).createThreadToregister(registerUsuarioRequest);
        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetTheAcceptTermsAndConditionsWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        registerPresenter.validateInternetToTermsAndConditionsRegister();

        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetTheAcceptTermsAndConditionsWithoutConnectionShoudlCallMethodStartTermsAndConditions() {
        when(validateInternet.isConnected()).thenReturn(true);
        registerPresenter.validateInternetToTermsAndConditionsRegister();

        verify(registerView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, R.string.text_validate_internet);
        verify(registerView).startTermsAndConditions();
    }

    @Test
    public void methodCreateThreadToRegisterShouldShowProgressDialog() {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        registerPresenter.createThreadToregister(registerUsuarioRequest);

        verify(registerView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodRegisterShouldCallMethodRegisterInSecurityBL() throws RepositoryError {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.register(registerUsuarioRequest)).thenReturn(mensaje);
        registerPresenter.register(registerUsuarioRequest);
        verify(securityBusinessLogic).register(registerUsuarioRequest);
    }

    @Test
    public void methodRegisterShouldCallVerifyRegister() throws RepositoryError {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.register(registerUsuarioRequest)).thenReturn(mensaje);
        registerPresenter.register(registerUsuarioRequest);
        verify(registerPresenter).verifyRegister(mensaje);
    }

    @Test
    public void methodVerifyRegisterShoulShowAnAlertDialogWhenIsCorrect() throws RepositoryError {
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(1);
        registerPresenter.verifyRegister(mensaje);
        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, mensaje.getText());
        verify(registerView).startFragmentIniciarSesion();
    }

    @Test
    public void methodVerifyRegisterShoulShowAnAlertDialogWhenIsIncorrect() throws RepositoryError {
        Mensaje mensaje = new Mensaje();
        registerPresenter.verifyRegister(mensaje);
        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, mensaje.getText());
    }

    @Test
    public void methodRegisterShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(securityRepository.register(registerUsuarioRequest)).thenThrow(repositoryError);
        registerPresenter.register(registerUsuarioRequest);
        verify(registerView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
    }

    @Test
    public void methodRegisterShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(securityRepository.register(registerUsuarioRequest)).thenThrow(repositoryError);
        registerPresenter.register(registerUsuarioRequest);
        verify(registerView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }

    @Test
    public void methodRegisterShouldCallMethodHideProgressDialog() throws RepositoryError {
        Usuario registerUsuarioRequest = getRegisterUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(securityRepository.register(registerUsuarioRequest)).thenReturn(mensaje);
        registerPresenter.register(registerUsuarioRequest);
        verify(registerView).dismissProgressDialog();
    }
}
