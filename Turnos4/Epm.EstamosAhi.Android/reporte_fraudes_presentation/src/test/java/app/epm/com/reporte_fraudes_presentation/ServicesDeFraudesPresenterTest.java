package app.epm.com.reporte_fraudes_presentation;

import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.reporte_fraudes_domain.reporte_fraudes.IReporteDeFraudesRepository;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.reporte_fraudes_presentation.presenters.ServicesDeFraudesPresenter;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IServicesDeFraudesView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by ocadavid on 17/04/2017.
 */

@RunWith(MockitoJUnitRunner.class)
public class ServicesDeFraudesPresenterTest {

    @Mock
    IServicesDeFraudesView servicesDeFraudesView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IReporteDeFraudesRepository reporteDeFraudesRepository;

    ServicesDeFraudesPresenter servicesDeFraudesPresenter;

    ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic;

    @Before
    public void setUp() throws Exception {
        reporteDeFraudesBusinessLogic = Mockito.spy(new ReporteDeFraudesBusinessLogic(reporteDeFraudesRepository));
        servicesDeFraudesPresenter = Mockito.spy(new ServicesDeFraudesPresenter(reporteDeFraudesBusinessLogic));
        servicesDeFraudesPresenter.inject(servicesDeFraudesView, validateInternet);
    }

    @Test
    public void methodValidateInternetWithoutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        servicesDeFraudesPresenter.validateInternetToGetServicesKML();
        verify(servicesDeFraudesView).showAlertDialogGeneralInformationOnUiThread(servicesDeFraudesView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithConnectionShoudlCallMethodCreateThreadGetServicesKML() {
        when(validateInternet.isConnected()).thenReturn(true);
        servicesDeFraudesPresenter.validateInternetToGetServicesKML();
        verify(servicesDeFraudesPresenter).createThreadGetServicesKML();
        verify(servicesDeFraudesView, never()).showAlertDialogTryAgain(servicesDeFraudesView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodCreateThreadGetServicesKMLShouldShowProgressDialog() {
        servicesDeFraudesPresenter.createThreadGetServicesKML();
        verify(servicesDeFraudesView).showProgressDIalog(R.string.text_services);
    }

    @Test
    public void methodGetServicesKMLShouldCallMethodGetServicesKML() throws RepositoryError {
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        when(reporteDeFraudesRepository.getServicioKML()).thenReturn(serviciosMapa);
        servicesDeFraudesPresenter.getServicesKML();
        verify(reporteDeFraudesRepository).getServicioKML();
    }

    @Test
    public void methodGetServicesKMLShouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        when(reporteDeFraudesRepository.getServicioKML()).thenThrow(repositoryError);
        servicesDeFraudesPresenter.getServicesKML();
        verify(servicesDeFraudesView).showAlertDialogTryAgain(R.string.title_error, repositoryError.getMessage());
        verify(servicesDeFraudesView, never()).enableServices(serviciosMapa);
    }

    @Test
    public void methodGetServicesKMLShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        RepositoryError repositoryError = new RepositoryError("");
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        when(reporteDeFraudesRepository.getServicioKML()).thenThrow(repositoryError);
        servicesDeFraudesPresenter.getServicesKML();
        verify(servicesDeFraudesView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(servicesDeFraudesView, never()).enableServices(serviciosMapa);
    }

    @Test
    public void methodGetServicesKMLShouldCallMethodIntentServices() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        when(reporteDeFraudesRepository.getServicioKML()).thenReturn(serviciosMapa);
        servicesDeFraudesPresenter.getServicesKML();
        verify(servicesDeFraudesView).enableServices(serviciosMapa);
        verify(servicesDeFraudesView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, repositoryError.getMessage());
    }
}
