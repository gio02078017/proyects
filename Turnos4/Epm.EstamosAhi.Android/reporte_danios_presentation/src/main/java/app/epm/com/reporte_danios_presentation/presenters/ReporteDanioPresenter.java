package app.epm.com.reporte_danios_presentation.presenters;

import android.content.Context;

import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.reporte_danios_presentation.R;
import app.epm.com.reporte_danios_presentation.view.views_activities.IReporteDanioView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.services.ServicesArcGIS;

public class ReporteDanioPresenter extends BasePresenter<IReporteDanioView> {

    private final DaniosBL daniosBL;

    public ReporteDanioPresenter(DaniosBL daniosBL) {
        this.daniosBL = daniosBL;
    }

    public void validateInternetToSendReportDanio(ReportDanio reportDanio, ServicesArcGIS servicesArcGIS, Context context) {
        if (getValidateInternet().isConnected()) {
            createThreadToSendReportDanio(reportDanio, servicesArcGIS, context);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), app.epm.com.security_presentation.R.string.text_validate_internet);
        }
    }

    public void createThreadToSendReportDanio(final ReportDanio reportDanio, final ServicesArcGIS servicesArcGIS, final Context context) {
        getView().showProgressDIalog(app.epm.com.security_presentation.R.string.text_please_wait);
        Thread thread = new Thread(() -> sendReportDanio(reportDanio, servicesArcGIS, context));
        thread.start();
    }


    public void sendReportDanio(final ReportDanio reportDanio, final ServicesArcGIS servicesArcGIS, final Context context) {
        String numberReport = daniosBL.sendReportDanioArcgis(reportDanio, servicesArcGIS, context);
        if (numberReport == null) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, R.string.message_error_send_danios);
        } else {
            if (!getView().getEmail().isEmpty()) {
                sendEmail(getView().getEmail(), reportDanio.getTipoServicio().getName(), numberReport);
            }
            getView().showAlertDialogSendReportDanioSuccessful(numberReport);
        }
        getView().dismissProgressDialog();
    }


    public void sendEmail(String email, String typeService, String numberReport) {
        ParametrosEnviarCorreo parametrosEnviarCorreo = new ParametrosEnviarCorreo();
        parametrosEnviarCorreo.setCorreoElectronico(email);
        parametrosEnviarCorreo.setNombreServicio(typeService);
        parametrosEnviarCorreo.setNumeroRadicado(numberReport);
        try {
            daniosBL.sendEmail(parametrosEnviarCorreo);
        } catch (RepositoryError repositoryError) {

        }
    }
}