package app.epm.com.reporte_danios_presentation.danios;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.reporte_danios_domain.danios.danios.IDaniosRepository;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.presenters.ServiciosDaniosPresenter;
import app.epm.com.reporte_danios_presentation.view.views_activities.IServiciosDanioView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 23/02/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class ServiciosDanioPresenterTest {

    ServiciosDaniosPresenter serviciosDaniosPresenter;
    DaniosBL daniosBL;

    @Mock
    IDaniosRepository daniosRepository;

    @Mock
    IServiciosDanioView serviciosDanioView;

    @Mock
    IValidateInternet validateInternet;

    @Before
    public void setUp() throws Exception{
        daniosBL = Mockito.spy(new DaniosBL(daniosRepository));
        serviciosDaniosPresenter = Mockito.spy( new ServiciosDaniosPresenter(daniosBL));
        serviciosDaniosPresenter.inject(serviciosDanioView, validateInternet);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadGetServicesKML(){
        when(validateInternet.isConnected()).thenReturn(true);
        serviciosDaniosPresenter.validateInternetToGetServicesKML();
        verify(serviciosDaniosPresenter).createThreadGetServicesKML();
        verify(serviciosDanioView, never()).showAlertDialogTryAgain(serviciosDanioView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog(){
        when(validateInternet.isConnected()).thenReturn(false);
        serviciosDaniosPresenter.validateInternetToGetServicesKML();
        verify(serviciosDanioView).showAlertDialogTryAgain(serviciosDanioView.getName(), R.string.text_validate_internet);
        verify(serviciosDaniosPresenter, never()).createThreadGetServicesKML();
    }

    @Test
    public void methodCreateThreadGetServicesKMLShouldShowProgressDialog(){
        serviciosDaniosPresenter.createThreadGetServicesKML();
        verify(serviciosDanioView).showProgressDIalog(R.string.text_services);
    }

    @Test
    public void methodGetServicesKMLShouldCallMethodGetServicesKML() throws RepositoryError {
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        when(daniosRepository.getServicesKML()).thenReturn(serviciosMapa);
        serviciosDaniosPresenter.getServicesKML();
        verify(daniosBL).getServicesKML();
        verify(serviciosDanioView).dismissProgressDialog();
    }

    @Test
    public void methodGetServicesKMLShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        when(daniosRepository.getServicesKML()).thenThrow(repositoryError);
        serviciosDaniosPresenter.getServicesKML();
        verify(serviciosDanioView).showAlertDialogTryAgain(R.string.title_error, repositoryError.getMessage());
        verify(serviciosDanioView, never()).enableServices(serviciosMapa);
        verify(serviciosDanioView).dismissProgressDialog();
    }

    @Test
    public void methodGetServicesKMLShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        RepositoryError repositoryError = new RepositoryError("");
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(daniosRepository.getServicesKML()).thenThrow(repositoryError);
        serviciosDaniosPresenter.getServicesKML();
        verify(serviciosDanioView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(serviciosDanioView, never()).enableServices(serviciosMapa);
        verify(serviciosDanioView).dismissProgressDialog();
    }

    @Test
    public void methodGetServicesKMLShouldCallMethodIntentServices() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        when(daniosRepository.getServicesKML()).thenReturn(serviciosMapa);
        serviciosDaniosPresenter.getServicesKML();
        verify(serviciosDanioView).enableServices(serviciosMapa);
        verify(serviciosDanioView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
        verify(serviciosDanioView).dismissProgressDialog();
    }

}