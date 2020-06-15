package app.epm.com.reporte_fraudes_presentation;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.reporte_fraudes_domain.reporte_fraudes.IReporteDeFraudesRepository;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.reporte_fraudes_presentation.presenters.UbicacionDeFraudePresenter;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IUbicacionDeFraudeView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 19/04/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class UbicacionDeFraudePresenterTest {

    @Mock
    IReporteDeFraudesRepository reporteDeFraudesRepository;

    @Mock
    IUbicacionDeFraudeView ubicacionDeFraudeView;

    @Mock
    IValidateInternet validateInternet;

    ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic;

    UbicacionDeFraudePresenter ubicacionDeFraudePresenter;

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    @Before
    public void setUp() throws Exception {
        reporteDeFraudesBusinessLogic = Mockito.spy(new ReporteDeFraudesBusinessLogic(reporteDeFraudesRepository));
        ubicacionDeFraudePresenter = Mockito.spy(new UbicacionDeFraudePresenter(reporteDeFraudesBusinessLogic));
        ubicacionDeFraudePresenter.inject(ubicacionDeFraudeView, validateInternet);
    }

    private ParametrosCobertura getParametrosCobertura() {
        ParametrosCobertura parametrosCobertura = new ParametrosCobertura();
        parametrosCobertura.setTipoServicio(ETipoServicio.Agua.getValue());
        parametrosCobertura.setMunicipio("Bello");
        parametrosCobertura.setDepartamento("ANTIOQUIA");
        return parametrosCobertura;
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadGetListMunicipalities() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(validateInternet.isConnected()).thenReturn(true);
        ubicacionDeFraudePresenter.validateInternetGetListMunicipalities(parametrosCobertura);
        verify(ubicacionDeFraudePresenter).createThreadGetListMunicipalities(parametrosCobertura);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAlertDialog() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(validateInternet.isConnected()).thenReturn(false);
        ubicacionDeFraudePresenter.validateInternetGetListMunicipalities(parametrosCobertura);
        verify(ubicacionDeFraudeView).showAlertDialogGeneralInformationOnUiThread(ubicacionDeFraudeView.getName(), R.string.text_validate_internet);
        verify(ubicacionDeFraudePresenter, never()).createThreadGetListMunicipalities(parametrosCobertura);
    }

    @Test
    public void methodCreateThreadGetListMunicipalitiesShouldshowProgressDialog() {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        ubicacionDeFraudePresenter.createThreadGetListMunicipalities(parametrosCobertura);
        verify(ubicacionDeFraudeView).showProgressDIalog(R.string.text_wait_please);
    }

    @Test
    public void methodCreateThreadGetListMunicipalitiesShouldCallMethodGetListMunicipalities() throws RepositoryError {
        List<String> listMunicipalities = new ArrayList<>();
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(reporteDeFraudesRepository.getMunicipalitiesWithCoverage(parametrosCobertura)).thenReturn(listMunicipalities);
        ubicacionDeFraudePresenter.getListMunicipalities(parametrosCobertura);
        verify(reporteDeFraudesBusinessLogic).getMunicipalitiesWithCoverage(parametrosCobertura);
    }

    @Test
    public void methodGetListMunicipalitiesShouldReturnTrueIfBelloIsInListMunicipalities() throws RepositoryError {
        List<String> listMunicipalities = new ArrayList<>();
        listMunicipalities.add("BELLO");
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(reporteDeFraudesRepository.getMunicipalitiesWithCoverage(parametrosCobertura)).thenReturn(listMunicipalities);
        Assert.assertTrue(ubicacionDeFraudePresenter.getListMunicipalities(parametrosCobertura));
    }

    @Test
    public void methodGetListMunicipalitiesShouldReturnFalseIfBelloIsNotInListMunicipalities() throws RepositoryError {
        List<String> listMunicipalities = new ArrayList<>();
        listMunicipalities.add("SABANETA");
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(reporteDeFraudesRepository.getMunicipalitiesWithCoverage(parametrosCobertura)).thenReturn(listMunicipalities);
        Assert.assertFalse(ubicacionDeFraudePresenter.getListMunicipalities(parametrosCobertura));
    }

    @Test
    public void methodGetListMunicipalitiesShouldThrowsWhenRepositoryThrowsAnError() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(reporteDeFraudesRepository.getMunicipalitiesWithCoverage(parametrosCobertura)).thenThrow(repositoryError);
        expectedException.expect(RepositoryError.class);
        expectedException.expectMessage(Constants.DEFAUL_ERROR);
        ubicacionDeFraudePresenter.getListMunicipalities(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipalitiesWithCoverageShouldThrowsWhenRepositoryThrowsAnError() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(ubicacionDeFraudePresenter.getListMunicipalities(parametrosCobertura)).thenThrow(repositoryError);
        ubicacionDeFraudePresenter.getMunicipalitiesWithCoverage(parametrosCobertura);
        verify(ubicacionDeFraudeView).showAlertDialogGeneralInformationOnUiThread(ubicacionDeFraudeView.getName(), repositoryError.getMessage());
    }

    @Test
    public void methodGetMunicipalitiesWithCoverageShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        RepositoryError repositoryError = new RepositoryError("");
        repositoryError.setIdError(Constants.UNAUTHORIZED_ERROR_CODE);
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(ubicacionDeFraudePresenter.getListMunicipalities(parametrosCobertura)).thenThrow(repositoryError);
        ubicacionDeFraudePresenter.getMunicipalitiesWithCoverage(parametrosCobertura);
        verify(ubicacionDeFraudeView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
    }
}