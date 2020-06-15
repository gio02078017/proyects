package app.epm.com.reporte_fraudes_presentation.presenters;

import android.content.Context;
import android.content.DialogInterface;

import com.epm.app.business_models.business_models.RepositoryError;

import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.ReporteDeFraudesBusinessLogic;
import app.epm.com.reporte_fraudes_presentation.R;
import app.epm.com.reporte_fraudes_presentation.view.views_activities.IRegisterReporteDeFraudesView;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.services.ServicesArcGIS;

/**
 * Created by mateoquicenososa on 4/04/17.
 */

public class RegisterReporteDeFraudesPresenter extends BasePresenter<IRegisterReporteDeFraudesView> {

    private ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic;

    public RegisterReporteDeFraudesPresenter(ReporteDeFraudesBusinessLogic reporteDeFraudesBusinessLogic) {
        this.reporteDeFraudesBusinessLogic = reporteDeFraudesBusinessLogic;
    }


    public void validateInternetSendrReporteDeFraude(ReporteDeFraude reporteDeFraude, ServicesArcGIS servicesArcGIS, Context context) {
        if (getValidateInternet().isConnected()) {
            createThreadToSendReportFraude(reporteDeFraude, servicesArcGIS, context);
        } else {
            getView().showAlertDialogGeneralInformationOnUiThread(getView().getName(), R.string.text_validate_internet);
        }
    }

    public void createThreadToSendReportFraude(final ReporteDeFraude reporteDeFraude, final ServicesArcGIS servicesArcGIS, final Context context) {
        getView().showProgressDIalog(R.string.text_wait_please);
        Thread thread = new Thread(() -> sendReportFraude(reporteDeFraude, servicesArcGIS, context));
        thread.start();
    }

    public void sendReportFraude(final ReporteDeFraude reporteDeFraude, final ServicesArcGIS servicesArcGIS, final Context context) {

        final String numberReport = reporteDeFraudesBusinessLogic.sendReportFraudeArcgis(reporteDeFraude, servicesArcGIS, context);
        if (numberReport == null) {
            getView().showAlertDialogGeneralInformationOnUiThread(R.string.title_error, R.string.message_error_send_fraude);
        } else {
            sendEmail(reporteDeFraude, numberReport);
        }
    }

    public void sendEmail( ReporteDeFraude reporteDeFraude, String numberReport) {

        ParametrossReporteDeFraudes parametrossReporteDeFraudes = new ParametrossReporteDeFraudes();
        parametrossReporteDeFraudes.setCorreoElectronico(reporteDeFraude.getUserEmail());
        parametrossReporteDeFraudes.setNombre(reporteDeFraude.getUserName());
        parametrossReporteDeFraudes.setTipoServicio(reporteDeFraude.getTypeService().getValue());
        parametrossReporteDeFraudes.setNumeroRadicado(numberReport);
        try {
            reporteDeFraudesBusinessLogic.sendEmailTheRegister(parametrossReporteDeFraudes);
        } catch (RepositoryError repositoryError) {

        } finally {
            getView().showAlertDialogSendReportFraude(numberReport, reporteDeFraude.getTypeService().getName());
            getView().dismissProgressDialog();
        }

    }
}