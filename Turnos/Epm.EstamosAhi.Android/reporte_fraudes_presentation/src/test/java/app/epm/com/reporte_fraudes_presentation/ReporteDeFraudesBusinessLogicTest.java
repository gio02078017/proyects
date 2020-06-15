package app.epm.com.reporte_fraudes_presentation;

import android.content.Context;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.IReporteDeFraudesRepository;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class ReporteDeFraudesBusinessLogicTest {
    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic;

    Context context;

    ServicesArcGIS servicesArcGIS;

    @Mock
    IReporteDeFraudesRepository reporteDeFraudesRepository;

    @Before
    public void setUp() throws Exception {
        reporteDeFraudesBusinessLogic = new ReporteDeFraudesBusinessLogic(reporteDeFraudesRepository);
    }

    private ParametrossReporteDeFraudes getParametrosReporteDeFraudes() {
        ParametrossReporteDeFraudes parametrosReporteDeFraudes = new ParametrossReporteDeFraudes();
        parametrosReporteDeFraudes.setCorreoElectronico("test@ig.");
        parametrosReporteDeFraudes.setNombre("test");
        parametrosReporteDeFraudes.setNumeroRadicado("1");
        parametrosReporteDeFraudes.setTipoServicio(1);
        return parametrosReporteDeFraudes;
    }

    private ParametrosCobertura getParametrosCobertura() {
        ParametrosCobertura parametrosCobertura = new ParametrosCobertura();
        parametrosCobertura.setTipoServicio(ETipoServicio.Agua.getValue());
        parametrosCobertura.setMunicipio("Bello");
        parametrosCobertura.setDepartamento("ANTIOQUIA");
        return parametrosCobertura;
    }

    @Test
    public void methodGetMunicipalitiesWithCoverageWithParameterNullShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        ParametrosCobertura parametrosCobertura = null;

        reporteDeFraudesBusinessLogic.getMunicipalitiesWithCoverage(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipalitiesWithCoverageWithDepartamentoNullShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setDepartamento(null);

        reporteDeFraudesBusinessLogic.getMunicipalitiesWithCoverage(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipalitiesWithCoverageWithDepartamentoEmptyShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);

        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setDepartamento("");

        reporteDeFraudesBusinessLogic.getMunicipalitiesWithCoverage(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipalitiesWithCoverageWithCorrectParametersShouldCalletMunicipalitiesWithCoverage() throws RepositoryError {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        reporteDeFraudesBusinessLogic.getMunicipalitiesWithCoverage(parametrosCobertura);
        verify(reporteDeFraudesRepository).getMunicipalitiesWithCoverage(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipalitiesWithCoverageWithCorrectParametersShouldReturnListMunicipalitiesFromRepository() throws RepositoryError {
        List<String> listMunicipalities = new ArrayList<>();
        listMunicipalities.add("BELLO");
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        when(reporteDeFraudesRepository.getMunicipalitiesWithCoverage(parametrosCobertura)).thenReturn(listMunicipalities);
        List<String> listMunicipalitiesResult = reporteDeFraudesBusinessLogic.getMunicipalitiesWithCoverage(parametrosCobertura);
        Assert.assertEquals(listMunicipalities, listMunicipalitiesResult);
    }

    @Test
    public void methodSendEmailTheRegisterWithResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        reporteDeFraudesBusinessLogic.sendEmailTheRegister(null);
    }

    @Test
    public void methodSendEmailTheRegisterWithEmailNullShouldReturnAnException() throws RepositoryError {
        ParametrossReporteDeFraudes parametrossReporteDeFraudes = getParametrosReporteDeFraudes();
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        parametrossReporteDeFraudes.setCorreoElectronico(null);
        reporteDeFraudesBusinessLogic.sendEmailTheRegister(parametrossReporteDeFraudes);
    }

    @Test
    public void methodSendEmailTheRegisterWithNameNullShouldReturnAnException() throws RepositoryError {
        ParametrossReporteDeFraudes parametrossReporteDeFraudes = getParametrosReporteDeFraudes();
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        parametrossReporteDeFraudes.setNombre(null);
        reporteDeFraudesBusinessLogic.sendEmailTheRegister(parametrossReporteDeFraudes);
    }

    @Test
    public void methodSendEmailTheRegisterWithNumberLocatedNullShouldReturnAnException() throws RepositoryError {
        ParametrossReporteDeFraudes parametrossReporteDeFraudes = getParametrosReporteDeFraudes();
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        parametrossReporteDeFraudes.setNumeroRadicado(null);
        reporteDeFraudesBusinessLogic.sendEmailTheRegister(parametrossReporteDeFraudes);
    }

    @Test
    public void methodSendEmailTheRegisterWithTypeServiceNullShouldReturnAnException() throws RepositoryError {
        ParametrossReporteDeFraudes parametrossReporteDeFraudes = getParametrosReporteDeFraudes();
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        parametrossReporteDeFraudes.setTipoServicio(null);
        reporteDeFraudesBusinessLogic.sendEmailTheRegister(parametrossReporteDeFraudes);
    }


    @Test
    public void  methodSendEmailTheRegisterWithCorrectParametersShouldCallMethodSendEmailTheRegisterInRepository() throws RepositoryError {
        ParametrossReporteDeFraudes parametrossReporteDeFraudes = getParametrosReporteDeFraudes();
        reporteDeFraudesBusinessLogic.sendEmailTheRegister(parametrossReporteDeFraudes);
        verify(reporteDeFraudesRepository).sendEmailTheRegister(parametrossReporteDeFraudes);
    }

    @Test
    public void methodSendEmailTheRegisterShouldReturnAMensajeWhenCallMethodSendEmailTheRegisterInRepository() throws RepositoryError {
        ParametrossReporteDeFraudes parametrossReporteDeFraudes = getParametrosReporteDeFraudes();
        Mensaje responseSendingEmail = new Mensaje();
        when(reporteDeFraudesRepository.sendEmailTheRegister(parametrossReporteDeFraudes)).thenReturn(responseSendingEmail);
        Mensaje result = reporteDeFraudesBusinessLogic.sendEmailTheRegister(parametrossReporteDeFraudes);
        Assert.assertEquals(responseSendingEmail, result);
    }

    @Test
    public void  methodSendReportFraudeArcgisWithCorrectParametersShouldCallMethodSendReportFraudeArcgisInRepository() throws RepositoryError {
        ReporteDeFraude reporteDeFraude = new ReporteDeFraude();
        reporteDeFraudesBusinessLogic.sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context);
        verify(reporteDeFraudesRepository).sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context);
    }

    @Test
    public void methodSendReportFraudeArcgisShouldReturnAStringWhenCallMethodSendReportFraudeArcgisInRepository() throws RepositoryError {
        ReporteDeFraude reporteDeFraude = new ReporteDeFraude();
        String responseSendingReportFraude = "test";
        when(reporteDeFraudesRepository.sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context)).thenReturn(responseSendingReportFraude);
        String result = reporteDeFraudesBusinessLogic.sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context);
        Assert.assertEquals(responseSendingReportFraude, result);
    }
}