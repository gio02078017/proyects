package app.epm.com.contacto_transparente_presentation.contacto_transparente;

import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.runners.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.contacto_transparente_domain.contacto_transparente.IContactoTransparenteRepository;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.presenters.HomeContactoTransparentePresenter;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IHomeContactoTransparenteView;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 14/03/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class HomeContactoTransparentePresenterTest {

    HomeContactoTransparentePresenter homeContactoTransparentePresenter;
    ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic;


    @Mock
    IContactoTransparenteRepository contactoTransparenteRepository;

    @Mock
    IHomeContactoTransparenteView homeContactoTransparenteView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    ICustomSharedPreferences customSharedPreferences;

    @Before
    public void setUp() throws Exception{
        contactoTransparenteBusinessLogic = Mockito.spy(new ContactoTransparenteBusinessLogic(contactoTransparenteRepository));
        homeContactoTransparentePresenter = Mockito.spy(new HomeContactoTransparentePresenter(contactoTransparenteBusinessLogic));
        homeContactoTransparentePresenter.inject(homeContactoTransparenteView, validateInternet);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadToGetListInterestGroup(){
        when(validateInternet.isConnected()).thenReturn(true);
        homeContactoTransparentePresenter.validateInternetToGetListInterestGroup();
        verify(homeContactoTransparentePresenter).createThreadToGetListInterestGroup();
        verify(homeContactoTransparenteView, never()).showAlertDialogLoadAgainOnUiThread(homeContactoTransparenteView.getName(),R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog(){
        when(validateInternet.isConnected()).thenReturn(false);
        homeContactoTransparentePresenter.validateInternetToGetListInterestGroup();
        verify(homeContactoTransparenteView).showAlertDialogLoadAgainOnUiThread(homeContactoTransparenteView.getName(), R.string.text_validate_internet);
        verify(homeContactoTransparentePresenter,never()).createThreadToGetListInterestGroup();
    }

    @Test
    public void methodCreateThreadToGetListInterestGroupShouldShowProgressDialog(){
        homeContactoTransparentePresenter.createThreadToGetListInterestGroup();
        verify(homeContactoTransparenteView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodCreateThreadToGetListInterestGroupShouldCallMethodGetListInterestGroup() throws RepositoryError {
        List<GrupoInteres> grupoInteresList = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(contactoTransparenteRepository.getGruposDeInteres()).thenReturn(grupoInteresList);
        homeContactoTransparentePresenter.getGruposDeInteres();
        verify(contactoTransparenteBusinessLogic).getGruposDeInteres();
        verify(homeContactoTransparenteView).callActivityDescribesAndSentDataForIntent(grupoInteresList);
        verify(homeContactoTransparenteView, never()).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(homeContactoTransparenteView, never()).showAlertDialogLoadAgainOnUiThread(R.string.title_error,repositoryError.getMessage());
        verify(homeContactoTransparenteView).dismissProgressDialog();
    }

    @Test
    public void methodGetListInterestGroupShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        List<GrupoInteres> grupoInteresList = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(401);
        when(contactoTransparenteRepository.getGruposDeInteres()).thenThrow(repositoryError);
        homeContactoTransparentePresenter.getGruposDeInteres();
        verify(homeContactoTransparenteView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(homeContactoTransparenteView, never()).callActivityDescribesAndSentDataForIntent(grupoInteresList);
        verify(homeContactoTransparenteView, never()).showAlertDialogLoadAgainOnUiThread(R.string.title_error,repositoryError.getMessage());
        verify(homeContactoTransparenteView).dismissProgressDialog();
    }


    @Test
    public void methodGetListInterestGroupShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        List<GrupoInteres> grupoInteresList = new ArrayList<>();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(contactoTransparenteRepository.getGruposDeInteres()).thenThrow(repositoryError);
        homeContactoTransparentePresenter.getGruposDeInteres();
        verify(homeContactoTransparenteView).showAlertDialogLoadAgainOnUiThread(R.string.title_error,repositoryError.getMessage());
        verify(homeContactoTransparenteView, never()).callActivityDescribesAndSentDataForIntent(grupoInteresList);
        verify(homeContactoTransparenteView).dismissProgressDialog();
    }

    @Test
    public void methodGetListInterestGroupShouldCallMethodHideProgressDialog() throws RepositoryError {
        List<GrupoInteres> grupoInteresList = new ArrayList<>();
        when(contactoTransparenteRepository.getGruposDeInteres()).thenReturn(grupoInteresList);
        homeContactoTransparentePresenter.getGruposDeInteres();
        verify(homeContactoTransparenteView).dismissProgressDialog();
    }


}
