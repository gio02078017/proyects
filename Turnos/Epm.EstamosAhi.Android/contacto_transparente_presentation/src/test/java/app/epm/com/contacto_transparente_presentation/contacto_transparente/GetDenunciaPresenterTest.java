package app.epm.com.contacto_transparente_presentation.contacto_transparente;

import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.runners.MockitoJUnitRunner;

import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.contacto_transparente_domain.contacto_transparente.IContactoTransparenteRepository;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.presenters.GetDenunciaPresenter;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IGetDenunciaView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by Jquinterov on 3/17/2017.
 */
@RunWith(MockitoJUnitRunner.class)
public class GetDenunciaPresenterTest {

    GetDenunciaPresenter getDenunciaPresenter;
    ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic;

    @Mock
    IContactoTransparenteRepository contactoTransparenteRepository;

    @Mock
    IGetDenunciaView consultView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ICustomSharedPreferences iCustomSharedPreferences;

    @Before
    public void setUp() throws Exception {
        contactoTransparenteBusinessLogic = Mockito.spy(new ContactoTransparenteBusinessLogic(contactoTransparenteRepository));
        getDenunciaPresenter = Mockito.spy(new GetDenunciaPresenter(contactoTransparenteBusinessLogic));
        getDenunciaPresenter.inject(consultView, validateInternet);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToGetConsultarIncidente() {
        String codigoIncidente = getCodigoIncidente();
        when(validateInternet.isConnected()).thenReturn(true);
        getDenunciaPresenter.validateInternetToGetIncidente(codigoIncidente);
        verify(getDenunciaPresenter).createThreadToGetIncidente(codigoIncidente);
        verify(consultView, never()).showAlertDialogGeneralInformationOnUiThread(consultView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog() {
        String codigoIncidente = getCodigoIncidente();
        when(validateInternet.isConnected()).thenReturn(false);
        getDenunciaPresenter.validateInternetToGetIncidente(codigoIncidente);
        verify(consultView).showAlertDialogGeneralInformationOnUiThread(consultView.getName(), R.string.text_validate_internet);
        verify(consultView).getName();
        verify(getDenunciaPresenter, never()).createThreadToGetIncidente(codigoIncidente);
    }

    @Test
    public void methodCreateThreadToGetIncidenteShouldShowProgressDialog() {
        String codigoIncidente = getCodigoIncidente();
        getDenunciaPresenter.createThreadToGetIncidente(codigoIncidente);
        verify(consultView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodGetIncidenteShouldCallGetIncidenteBL() throws RepositoryError {
        Incidente incidente = new Incidente();
        String codigoIncidente = getCodigoIncidente();
        when(contactoTransparenteRepository.getIncidente(codigoIncidente)).thenReturn(incidente);
        getDenunciaPresenter.getIncidente(codigoIncidente);
        verify(contactoTransparenteBusinessLogic).getIncidente(codigoIncidente);
        verify(consultView).dismissProgressDialog();
    }

    @Test
    public void methodGetIncidenteShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        Incidente incidente = new Incidente();
        String codigoIncidente = getCodigoIncidente();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(401);

        when(contactoTransparenteRepository.getIncidente(codigoIncidente)).thenThrow(repositoryError);
        getDenunciaPresenter.getIncidente(codigoIncidente);
        verify(consultView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(consultView).dismissProgressDialog();
    }

    @Test
    public void methodGetIncidenteShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        String codigoIncidente = getCodigoIncidente();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(contactoTransparenteRepository.getIncidente(codigoIncidente)).thenThrow(repositoryError);
        getDenunciaPresenter.getIncidente(codigoIncidente);
        Assert.assertFalse(repositoryError.getMessage().contains(Constants.NOT_FOUND));
        verify(consultView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(consultView, never()).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(consultView).dismissProgressDialog();
    }


    @Test
    public void methodGetIncidenteShouldShowAnAlertDialogWhenReturnAnExceptionIncident() throws RepositoryError {
        String codigoIncidente = getCodigoIncidente();
        RepositoryError repositoryError = new RepositoryError(Constants.NOT_FOUND);
        repositoryError.setIdError(0);
        when(contactoTransparenteRepository.getIncidente(codigoIncidente)).thenThrow(repositoryError);
        getDenunciaPresenter.getIncidente(codigoIncidente);
        Assert.assertTrue(repositoryError.getMessage().contains(Constants.NOT_FOUND));
        verify(consultView).showAlertDialogOnUiThread(consultView.getName(), repositoryError.getMessage());
        verify(consultView, never()).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(consultView).dismissProgressDialog();
    }

    @Test
    public void methodGetIncidenteShouldCallMethodHideProgressDialog() throws RepositoryError {
        Incidente incidente = new Incidente();
        String codigoIncidente = getCodigoIncidente();
        when(contactoTransparenteRepository.getIncidente(codigoIncidente)).thenReturn(incidente);
        getDenunciaPresenter.getIncidente(codigoIncidente);
        verify(contactoTransparenteBusinessLogic).getIncidente(codigoIncidente);
        verify(consultView).dismissProgressDialog();
    }

    private String getCodigoIncidente() {
        String codigoIncidente = "123";
        return codigoIncidente;
    }
}
