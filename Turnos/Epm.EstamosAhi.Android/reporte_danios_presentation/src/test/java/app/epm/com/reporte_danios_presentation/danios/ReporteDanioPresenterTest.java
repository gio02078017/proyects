package app.epm.com.reporte_danios_presentation;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.esri.arcgisruntime.data.FeatureTemplate;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.reporte_danios_domain.danios.danios.IDaniosRepository;
import app.epm.com.reporte_danios_presentation.presenters.ReporteDanioPresenter;
import app.epm.com.reporte_danios_presentation.view.views_activities.IReporteDanioView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Matchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by Jquinterov on 3/7/2017.
 */

@RunWith(MockitoJUnitRunner.Silent.class)
public class ReporteDanioPresenterTest {
    ReporteDanioPresenter reporteDanioPresenter;
    DaniosBL daniosBL;
    ReportDanio reportDanio = new ReportDanio();
    String numberRadicado = "123";
    ServicesArcGIS servicesArcGIS;
    FeatureTemplate featureTemplate;

    @Mock
    IDaniosRepository daniosRepository;

    @Mock
    IReporteDanioView reporteDanioView;

    @Mock
    IValidateInternet validateInternet;

    @Before
    public void setUp() throws Exception {
        daniosBL = Mockito.spy(new DaniosBL(daniosRepository));
        reporteDanioPresenter = Mockito.spy(new ReporteDanioPresenter(daniosBL));
        reporteDanioPresenter.inject(reporteDanioView, validateInternet);
    }

    @Test
    public void methodSendReportDanioCallValidateInternetWithOutConnectionShoudlShowAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        reporteDanioPresenter.validateInternetToSendReportDanio(reportDanio, servicesArcGIS, null);
        verify(reporteDanioView).showAlertDialogGeneralInformationOnUiThread(eq(reporteDanioView.getName()), eq(R.string.text_validate_internet));
    }

    @Test
    public void methodSendReportDanioCallCreateThreadToSendReportDanioArcgis() {
        when(validateInternet.isConnected()).thenReturn(true);
        reporteDanioPresenter.validateInternetToSendReportDanio(reportDanio, servicesArcGIS, null);
        verify(reporteDanioPresenter).createThreadToSendReportDanio(reportDanio, servicesArcGIS, null);
    }

    @Test
    public void methodCreateThreadToSendReportDanioShouldShowProgressDialog() {
        reporteDanioPresenter.createThreadToSendReportDanio(reportDanio, servicesArcGIS, null);
        verify(reporteDanioView).showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
    }

    @Test
    public void methodSendReportDanioCallSendReportDanioArcgis() throws RepositoryError {
        when(validateInternet.isConnected()).thenReturn(true);
        when(daniosBL.sendReportDanioArcgis(reportDanio, servicesArcGIS, null)).thenReturn(numberRadicado);
        reporteDanioPresenter.validateInternetToSendReportDanio(reportDanio, servicesArcGIS, null);
        verify(reporteDanioPresenter).createThreadToSendReportDanio(reportDanio, servicesArcGIS, null);
    }

    @Test
    public void methodSendReportDanioShuldCallShowAlertDialogWhenEmailIsEmpty() throws RepositoryError {
        when(daniosBL.sendReportDanioArcgis(reportDanio, servicesArcGIS, null)).thenReturn(numberRadicado);
        when(reporteDanioView.getEmail()).thenReturn(Constants.EMPTY_STRING);
        reporteDanioPresenter.sendReportDanio(reportDanio, servicesArcGIS, null);
        verify(reporteDanioView).showAlertDialogSendReportDanioSuccessful(numberRadicado);
    }

    @Test
    public void methodSendEmailTheRegisterShouldReturnAMensajeWhenCallMethodSendEmailTheRegisterInRepository() throws RepositoryError {
        ParametrosEnviarCorreo parametrosEnviarCorreo = new ParametrosEnviarCorreo();
        parametrosEnviarCorreo.setCorreoElectronico("dadad");
        parametrosEnviarCorreo.setNombreServicio("sdsdsd");
        parametrosEnviarCorreo.setNumeroRadicado("121314");
        Mensaje responseSendingEmail = new Mensaje();
        when(daniosRepository.sendEmail(parametrosEnviarCorreo)).thenReturn(responseSendingEmail);
        Mensaje result = daniosBL.sendEmail(parametrosEnviarCorreo);
        Assert.assertEquals(responseSendingEmail, result);
    }

    @Test
    public void methodSendReportFraudeArcgisWithCorrectParametersShouldCallMethodSendReportFraudeArcgisInRepository() throws RepositoryError {
        daniosBL.sendReportDanioArcgis(reportDanio, servicesArcGIS, null);
        verify(daniosRepository).sendReportDanioArcgis(reportDanio, servicesArcGIS, null);
    }

    @Test
    public void methodSendReportFraudeArcgisShouldReturnAStringWhenCallMethodSendReportFraudeArcgisInRepository() throws RepositoryError {
        String responseSendingReportDanio = "test";
        when(daniosRepository.sendReportDanioArcgis(reportDanio, servicesArcGIS, null)).thenReturn(responseSendingReportDanio);
        String result = daniosBL.sendReportDanioArcgis(reportDanio, servicesArcGIS, null);
        Assert.assertEquals(responseSendingReportDanio, result);
    }

    @Test
    public void methodSendEmailShouldCallMethodSendEmailInBusinessLogic() throws RepositoryError {
        ParametrosEnviarCorreo parametrosEnviarCorreo = new ParametrosEnviarCorreo();
        parametrosEnviarCorreo.setCorreoElectronico("email");
        parametrosEnviarCorreo.setNombreServicio("service");
        parametrosEnviarCorreo.setNumeroRadicado("1234");
        Mensaje mensaje = new Mensaje();
        when(daniosBL.sendEmail(parametrosEnviarCorreo)).thenReturn(mensaje);
        reporteDanioPresenter.sendEmail(parametrosEnviarCorreo.getCorreoElectronico(),
                parametrosEnviarCorreo.getNombreServicio(), parametrosEnviarCorreo.getNumeroRadicado());
        verify(daniosBL).sendEmail(parametrosEnviarCorreo);
    }
}
