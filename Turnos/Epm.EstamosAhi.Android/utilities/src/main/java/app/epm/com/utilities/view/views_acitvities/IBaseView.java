package app.epm.com.utilities.view.views_acitvities;

import android.content.DialogInterface;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public interface IBaseView {

    void showAlertDialogGeneralInformationOnUiThread(int title, int message);

    void showAlertDialogGeneralInformationOnUiThread(int title, String message);

    void showAlertDialogGeneralInformationOnUiThread(String title, String message);

    void showAlertDialogGeneralInformationOnUiThread(String title, int message);

    void showProgressDIalog(int text);

    void dismissProgressDialog();

    void showAlertDialogUnauthorized(int title, int message);

    String getName();

    String getEmail();

    void finishActivity();

    void validateShowAlertQualifyApp();
}
