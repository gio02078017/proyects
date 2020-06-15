package app.epm.com.reporte_fraudes_presentation;

import android.content.Context;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;

import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.IReporteDeFraudesRepository;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.reporte_fraudes_presentation.presenters.RegisterReporteDeFraudesPresenter;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IRegisterReporteDeFraudesView;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.services.ServicesArcGIS;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class RegisterReporteDeFraudePresenterTest {

    @Mock
    IRegisterReporteDeFraudesView registerReporteDeFraudesView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IReporteDeFraudesRepository reporteDeFraudesRepository;

    RegisterReporteDeFraudesPresenter registerReporteDeFraudesPresenter;

    ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic;

    ReporteDeFraude reporteDeFraude = new ReporteDeFraude();

    ServicesArcGIS servicesArcGIS;

    Context context;

    String numberRadicado = "123";

    @Before
    public void setUp() throws Exception {
        reporteDeFraudesBusinessLogic = Mockito.spy(new ReporteDeFraudesBusinessLogic(reporteDeFraudesRepository));
        registerReporteDeFraudesPresenter = Mockito.spy(new RegisterReporteDeFraudesPresenter(reporteDeFraudesBusinessLogic));
        registerReporteDeFraudesPresenter.inject(registerReporteDeFraudesView, validateInternet);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodSendReportFraude() {
        when(validateInternet.isConnected()).thenReturn(true);
        registerReporteDeFraudesPresenter.validateInternetSendrReporteDeFraude(reporteDeFraude, servicesArcGIS, context);
        verify(registerReporteDeFraudesPresenter).createThreadToSendReportFraude(reporteDeFraude, servicesArcGIS, context);
        verify(registerReporteDeFraudesView, never()).showAlertDialogGeneralInformationOnUiThread(registerReporteDeFraudesView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWithOutConnectionShouldShowAnAlertDialog() {
        when(validateInternet.isConnected()).thenReturn(false);
        registerReporteDeFraudesPresenter.validateInternetSendrReporteDeFraude(reporteDeFraude, servicesArcGIS, context);
        verify(registerReporteDeFraudesView).showAlertDialogGeneralInformationOnUiThread(registerReporteDeFraudesView.getName(), R.string.text_validate_internet);
        verify(registerReporteDeFraudesPresenter, never()).createThreadToSendReportFraude(reporteDeFraude, servicesArcGIS, context);
    }

    @Test
    public void methodCreateThreadToSendReportFraudeShouldShowAProgressDialog() {
        registerReporteDeFraudesPresenter.createThreadToSendReportFraude(reporteDeFraude, servicesArcGIS, context);
        verify(registerReporteDeFraudesView).showProgressDIalog(R.string.text_wait_please);
    }

    @Test
    public void methodSendReportFraudeArcgisShouldCallMethodSendReportFraudeInRepositoryAndReturnNull() {
        numberRadicado = null;
        reporteDeFraude.setTypeService(ETipoServicio.Agua);
        when(reporteDeFraudesRepository.sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context)).thenReturn(numberRadicado);
        registerReporteDeFraudesPresenter.sendReportFraude(reporteDeFraude, servicesArcGIS, context);
        verify(reporteDeFraudesRepository).sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context);
        Assert.assertTrue(numberRadicado == null);
        verify(registerReporteDeFraudesView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error, R.string.message_error_send_fraude);
        verify(registerReporteDeFraudesView, never()).showAlertDialogSendReportFraude("123", reporteDeFraude.getTypeService().getName());
    }

    @Test
    public void methodSendReportFraudeArcgisShouldCallMethodSendReportFraudeInRepositoryAndReturnNumebrReport() {
        reporteDeFraude.setUserName("leidy");
        reporteDeFraude.setUserEmail("lcz97@live.com");
        reporteDeFraude.setTypeService(ETipoServicio.Agua);
        when(reporteDeFraudesRepository.sendReportFraudeArcgis(reporteDeFraude, null, null)).thenReturn(numberRadicado);
        registerReporteDeFraudesPresenter.sendReportFraude(reporteDeFraude, null, null);
        verify(reporteDeFraudesRepository).sendReportFraudeArcgis(reporteDeFraude, null, null);
        Assert.assertTrue(numberRadicado != null);
        verify(reporteDeFraudesBusinessLogic).sendReportFraudeArcgis(reporteDeFraude, null, null);

        verify(registerReporteDeFraudesPresenter).sendEmail(reporteDeFraude,numberRadicado);

        verify(registerReporteDeFraudesView).showAlertDialogSendReportFraude(numberRadicado, reporteDeFraude.getTypeService().getName());
    }

    @Test
    public void methodSendEmailShouldCallMethodSenEmailTheRegisterInRepository() throws RepositoryError {
        reporteDeFraude.setUserName("leidy");
        reporteDeFraude.setUserEmail("lcz97@live.com");
        reporteDeFraude.setTypeService(ETipoServicio.Agua);
        ParametrossReporteDeFraudes parametrossReporteDeFraudes = new ParametrossReporteDeFraudes();
        parametrossReporteDeFraudes.setNombre("leidy");
        parametrossReporteDeFraudes.setCorreoElectronico("lcz97@live.com");
        parametrossReporteDeFraudes.setTipoServicio(1);
        parametrossReporteDeFraudes.setNumeroRadicado(numberRadicado);
        registerReporteDeFraudesPresenter.sendReportFraude(reporteDeFraude, null, null);

        registerReporteDeFraudesPresenter.sendEmail(reporteDeFraude,numberRadicado);
        verify(registerReporteDeFraudesPresenter).sendEmail(reporteDeFraude,numberRadicado);

    }
}