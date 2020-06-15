package app.epm.com.utilities.custom_controls;

import android.app.Activity;
import android.content.DialogInterface;
import androidx.appcompat.app.AlertDialog;
import android.view.View;

/**
 * Created by josetabaresramirez on 18/11/16.
 */

public class CustomAlertDialog {

    private Activity activity;
    private AlertDialog alertDialog;

    public CustomAlertDialog(Activity activity) {
        this.activity = activity;
    }


    public void showAlertDialog(int title, int message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton, View view) {
        showAlertDialog(activity.getResources().getString(title), activity.getResources().getString(message), cancelable, textPositiveButton, onClickListenerPositiveButton, 0, null, view);
    }

    public void showAlertDialog(int title, String message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton, View view) {
        showAlertDialog(activity.getResources().getString(title), message, cancelable, textPositiveButton, onClickListenerPositiveButton, 0, null, view);
    }

    public void showAlertDialog(String title, String message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton,View view) {
        showAlertDialog(title, message, cancelable, textPositiveButton, onClickListenerPositiveButton, 0, null, view);
    }

    public void showAlertDialog(String title, int message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton,View view) {
        showAlertDialog(title, activity.getResources().getString(message), cancelable, textPositiveButton, onClickListenerPositiveButton, 0, null, view);
    }

    public void showAlertDialog(int title, int message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton, int textNegativeButton, DialogInterface.OnClickListener onClickListenerNegativeButton, View view) {
        showAlertDialog(activity.getResources().getString(title), activity.getResources().getString(message), cancelable, textPositiveButton, onClickListenerPositiveButton, textNegativeButton, onClickListenerNegativeButton, view);
    }

    public void showAlertDialog(int title, String message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton, int textNegativeButton, DialogInterface.OnClickListener onClickListenerNegativeButton, View view) {
        showAlertDialog(activity.getResources().getString(title), message, cancelable, textPositiveButton, onClickListenerPositiveButton, textNegativeButton, onClickListenerNegativeButton, view);
    }

    public void showAlertDialog(String title, int message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton, int textNegativeButton, DialogInterface.OnClickListener onClickListenerNegativeButton, View view) {
        showAlertDialog(title, activity.getResources().getString(message), cancelable, textPositiveButton, onClickListenerPositiveButton, textNegativeButton, onClickListenerNegativeButton, view);
    }

    public void showAlertDialog(String title, String message, boolean cancelable, int textPositiveButton, DialogInterface.OnClickListener onClickListenerPositiveButton, int textNegativeButton, DialogInterface.OnClickListener onClickListenerNegativeButton, View view) {
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        builder.setTitle(title);
        if (view != null) {
            builder.setView(view);
        }
        builder.setMessage(message);
        builder.setCancelable(cancelable);

        if (onClickListenerPositiveButton != null) {
            builder.setPositiveButton(textPositiveButton, onClickListenerPositiveButton);
        }
        if (onClickListenerNegativeButton != null) {
            builder.setNegativeButton(textNegativeButton, onClickListenerNegativeButton);
        }
        alertDialog = builder.show();

    }

    public AlertDialog getAlertDialog() {
        return alertDialog;
    }
}
