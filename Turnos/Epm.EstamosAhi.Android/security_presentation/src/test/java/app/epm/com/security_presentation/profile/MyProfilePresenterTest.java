package app.epm.com.security_presentation.profile;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.security_domain.profile.IProfileRepository;
import app.epm.com.security_domain.profile.ProfileBL;
import app.epm.com.security_presentation.R;
import app.epm.com.security_presentation.presenters.MyProfilePresenter;
import app.epm.com.security_presentation.view.views_activities.IMyProfileView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 25/11/16.
 */

@RunWith(MockitoJUnitRunner.class)
public class MyProfilePresenterTest {

    @Mock
    IMyProfileView myProfileView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IProfileRepository profileRepository;

    MyProfilePresenter myProfilePresenter;

    ProfileBL profileBL;

    @Before
    public void setUp() throws Exception{
        profileBL = Mockito.spy(new ProfileBL(profileRepository));
        myProfilePresenter = Mockito.spy(new MyProfilePresenter(profileBL));
        myProfilePresenter.inject(myProfileView, validateInternet);
    }

    private Usuario getProfileUsuarioRequest(){
        Usuario  profileUsuarioRequest = new Usuario();
        profileUsuarioRequest.setNombres("leidy carolina");
        profileUsuarioRequest.setApellido("uluaga");
        profileUsuarioRequest.setIdTipoIdentificacion(1);
        profileUsuarioRequest.setNumeroIdentificacion("23242424");
        profileUsuarioRequest.setIdTipoPersona(1);
        profileUsuarioRequest.setTelefono("313131212");
        profileUsuarioRequest.setCelular("231231414");
        profileUsuarioRequest.setDireccion("dsds");
        profileUsuarioRequest.setIdTipoVivienda(1);
        profileUsuarioRequest.setPais("colombia");
        profileUsuarioRequest.setFechaNacimiento("2016-11-11");
        profileUsuarioRequest.setIdGenero(1);
        profileUsuarioRequest.setCorreoAlternativo("cz97@live.com");
        return profileUsuarioRequest;
    }

    @Test
    public void methodvalidateFieldsProfileRequiredWithEmptyNameShouldShowAlertDialog(){
        Usuario  registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setNombres(Constants.EMPTY_STRING);
        myProfilePresenter.validateFieldsProfileRequired(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document_profile);
    }

    @Test
    public void methodvalidateFieldsProfileRequiredWithEmptyLastNameShouldShowAlertDialog(){
        Usuario  registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setApellido(Constants.EMPTY_STRING);
        myProfilePresenter.validateFieldsProfileRequired(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document_profile);
    }

    @Test
    public void methodvalidateFieldsProfileRequiredWithEmptyNumberDocumentShouldShowAlertDialog(){
        Usuario  registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setNumeroIdentificacion(Constants.EMPTY_STRING);
        myProfilePresenter.validateFieldsProfileRequired(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document_profile);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
    }

    @Test
    public void methodValidateFieldsProfileRequiredWithEptyIdentificationShouldShowAlertDialog(){
        Usuario  registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setIdTipoIdentificacion(0);
        myProfilePresenter.validateFieldsProfileRequired(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document_profile);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
    }


    @Test
    public void methodValidateFieldsProfileRequiredWithAllParametersShouldCallMethodValidateEmail(){
        Usuario  registerUsuarioRequest = getProfileUsuarioRequest();
        myProfilePresenter.validateFieldsProfileRequired(registerUsuarioRequest);

        verify(myProfilePresenter).validateEmail(registerUsuarioRequest);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_type_document_profile);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_number_document);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_name);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_empty_fields, R.string.text_empty_lastname);
    }

    @Test
    public void methodValidateEmailWithoutAtAndDomainShouldShowAlertDialog(){
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setCorreoAlternativo("wewe");
        myProfilePresenter.validateEmail(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_invalid_fields, R.string.text_invalid_email);
        verify(myProfilePresenter, never()).validateInternetToUpdateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodValidateEmailWithoutAtShouldShowAlertDialog(){
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setCorreoAlternativo("wewe.com");
        myProfilePresenter.validateEmail(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_invalid_fields, R.string.text_invalid_email);
        verify(myProfilePresenter, never()).validateInternetToUpdateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodValidateEmailWithoutDomainShouldShowAlertDialog(){
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setCorreoAlternativo("wewe@");
        myProfilePresenter.validateEmail(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_invalid_fields, R.string.text_invalid_email);
        verify(myProfilePresenter, never()).validateInternetToUpdateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodVValidateEmailwithAllCorrectShoulCallMethodValidateInternetToUpdateProfile(){
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        registerUsuarioRequest.setCorreoAlternativo(Constants.EMPTY_STRING);
        myProfilePresenter.validateEmail(registerUsuarioRequest);

        verify(myProfilePresenter).validateInternetToUpdateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodValidateEmailWithCorrectEmailShouldCallMethodValidateInternet(){
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        myProfilePresenter.validateEmail(registerUsuarioRequest);
        Assert.assertTrue(!registerUsuarioRequest.getCorreoAlternativo().isEmpty());
        verify(myProfilePresenter).validateInternetToUpdateProfile(registerUsuarioRequest);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_invalid_fields, R.string.text_invalid_email);
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialog(){
        when(validateInternet.isConnected()).thenReturn(false);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        myProfilePresenter.validateEmail(registerUsuarioRequest);

        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(myProfileView.getName(), R.string.text_validate_internet);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_invalid_fields, R.string.text_invalid_email);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToUpdateProfile(){
        when(validateInternet.isConnected()).thenReturn(true);
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        myProfilePresenter.validateInternetToUpdateProfile(registerUsuarioRequest);

        verify(myProfilePresenter).createThreadToUpdateProfile(registerUsuarioRequest);
        verify(myProfileView, never()).showAlertDialogGeneralInformationOnUiThread(myProfileView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodCreateThreadToUpdateProfileShouldShowProgressDialog(){
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        myProfilePresenter.createThreadToUpdateProfile(registerUsuarioRequest);

        verify(myProfileView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodUpdateProfileShouldCallMethodProfileInProfileBL() throws RepositoryError {
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(profileRepository.updateProfile(registerUsuarioRequest)).thenReturn(mensaje);
        myProfilePresenter.updateProfile(registerUsuarioRequest);
        verify(profileBL).updateProfile(registerUsuarioRequest);
    }

    @Test
    public void methodUpdateProfileShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(profileRepository.updateProfile(registerUsuarioRequest)).thenThrow(repositoryError);
        myProfilePresenter.updateProfile(registerUsuarioRequest);
        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(R.string.title_appreciated_user, repositoryError.getMessage());
    }

    @Test
    public void methodUpdateProfileShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(profileRepository.updateProfile(registerUsuarioRequest)).thenThrow(repositoryError);
        myProfilePresenter.updateProfile(registerUsuarioRequest);
        verify(myProfileView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }


    @Test
    public void methodUpdateProfileShouldCallMethodHideProgressDialog() throws RepositoryError {
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        when(profileRepository.updateProfile(registerUsuarioRequest)).thenReturn(mensaje);
        myProfilePresenter.updateProfile(registerUsuarioRequest);
        verify(myProfileView).dismissProgressDialog();
    }

    @Test
    public void methodUpdateProfileShouldCallShowAlertDialog() throws RepositoryError {
        Usuario registerUsuarioRequest = getProfileUsuarioRequest();
        Mensaje mensaje = new Mensaje();
        mensaje.setText("");
        when(profileRepository.updateProfile(registerUsuarioRequest)).thenReturn(mensaje);
        myProfilePresenter.updateProfile(registerUsuarioRequest);
        String title = registerUsuarioRequest.getNombres();
        verify(myProfileView).showAlertDialogGeneralInformationOnUiThread(title,mensaje.getText());

    }


}
