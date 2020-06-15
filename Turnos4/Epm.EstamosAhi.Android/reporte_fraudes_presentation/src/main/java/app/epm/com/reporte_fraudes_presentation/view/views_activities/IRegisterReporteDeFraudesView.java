package app.epm.com.reporte_fraudes_presentation.view.views_activities;

import app.epm.com.utilities.view.views_acitvities.IBaseView;

/**
 * Created by mateoquicenososa on 4/04/17.
 */

public interface IRegisterReporteDeFraudesView extends IBaseView {

    void showAlertDialogSendReportFraude(String numberReport, String service);
}
